import { getClient } from '@/scripts/postgres-helpers';
import { Client } from 'pg';

import { NextApiResponse, NextApiRequest } from 'next';
import {
  CREDITS_PER_MONTH,
  CREDITS_SPENDING_INTERVAL,
  TRANSACTION_SYSTEM_NOTE,
} from '@/utils/userTransactions';
import { BatchJobReqSchema } from '@/utils/schema';
import { partnerLookUp } from '@/utils/submit-data';
import UserService, { USER_ROLES } from '@/utils/users';

const handleUserTransactions = async (client: Client) => {
  const userExpiredTransactions = await client.query(`
  -- NOTE(David): brief description of what this query does
  -- #1 get only those user_ids where last unique transaction timestamp by user_id is greater then CREDITS_SPENDING_INTERVAL
  -- #2 add credits_for that user_id
  -- #3 filter out records for users that don't have enabled credits_spending
  SELECT 
    user_id, 
    highest_created_at, 
    (
      SELECT 
        SUM(amount) 
      FROM 
        user_transactions 
      WHERE 
        user_id = t1.user_id
    ) AS total_amount 
  FROM 
    (
      SELECT 
        user_id, 
        MAX(created_at) AS highest_created_at 
      FROM 
        user_transactions 
      GROUP BY 
        user_id
    ) t1 
  WHERE 
    CURRENT_TIMESTAMP - t1.highest_created_at >= INTERVAL '${CREDITS_SPENDING_INTERVAL} days' 
    AND (
      SELECT 
        use_credits_system 
      FROM 
        users 
      WHERE 
        id = t1.user_id
    );
  `);

  const userIdsToDecreaseCreditsMonthly: number[] = [];
  const userIdsToDisableCreditsSystem: number[] = [];

  for (const { total_amount, user_id } of userExpiredTransactions.rows) {
    if (total_amount - CREDITS_PER_MONTH < 0) {
      userIdsToDisableCreditsSystem.push(user_id);
    } else if (total_amount - CREDITS_PER_MONTH === 0) {
      userIdsToDecreaseCreditsMonthly.push(user_id);
      userIdsToDisableCreditsSystem.push(user_id);
    } else {
      userIdsToDecreaseCreditsMonthly.push(user_id);
    }
  }

  //#1 - decrease credits per month for users with enough credits, and update last_transaction_expiration
  if (userIdsToDecreaseCreditsMonthly.length) {
    await client.query(`
    INSERT INTO user_transactions(user_id, amount, note) VALUES
      ${userIdsToDecreaseCreditsMonthly
        .map(
          (userId, i) =>
            `('${userId}', '${-CREDITS_PER_MONTH}', '${TRANSACTION_SYSTEM_NOTE} - monthly subscription')${
              i === userIdsToDecreaseCreditsMonthly.length - 1 ? ';' : ','
            }`,
        )
        .join('\n')}
        `);

    //#2 - update last_transaction_expiration to current date + CREDITS_SPENDING_INTERVAL
    await client.query(`
    UPDATE users SET last_transaction_expiration = NOW() + INTERVAL '${CREDITS_SPENDING_INTERVAL} days' WHERE id IN (${userIdsToDecreaseCreditsMonthly
      .map(
        (userId, i) =>
          `'${userId}'${
            i !== userIdsToDecreaseCreditsMonthly.length - 1 ? ',' : ''
          }`,
      )
      .join('\n')});
      `);
  }

  //#3 - disable credits system for users without credits
  if (userIdsToDisableCreditsSystem.length) {
    await client.query(`
    UPDATE users SET use_credits_system = false WHERE id IN (${userIdsToDisableCreditsSystem
      .map(
        (userId, i) =>
          `'${userId}'${
            i !== userIdsToDisableCreditsSystem.length - 1 ? ',' : ''
          }`,
      )
      .join('\n')});
      `);
  }
};

const handler = async (req: NextApiRequest, res: NextApiResponse) => {
  if (req.method !== 'POST') {
    res.status(405).json({ message: 'Method not allowed' });
  }

  const parseResponse = BatchJobReqSchema.safeParse(req.body);
  if (!parseResponse.success) {
    return res.status(400).json({
      error: parseResponse.error.errors,
      message: 'Invalid request body',
    });
  }

  const { apiKey } = parseResponse.data;
  if (!apiKey) {
    const { role } = (await UserService.getUserByCookies(req.cookies)) ?? {};
    if (role !== USER_ROLES.ADMIN) {
      return res.status(401).json({
        message: 'You are unauthorized for this operation!',
      });
    }
  } else {
    const partner = await partnerLookUp(apiKey);
    if (!partner) {
      return res.status(401).json({
        message: 'Invalid api key',
      });
    }
  }

  const client = await getClient();
  await handleUserTransactions(client);

  await client.query(
    `UPDATE vc_firms vc SET latest_investment = (SELECT round_date
    FROM investments
    INNER JOIN investment_rounds
      ON investments.round_id = investment_rounds.id
    WHERE
      investments.vc_firm_id = vc.id
      ORDER BY round_date DESC
      LIMIT 1)`,
    [],
  );

  await client.query(
    `UPDATE vc_firms vc SET num_of_investments = (SELECT count(*)
    FROM investments
    WHERE investments.vc_firm_id = vc.id)`,
    [],
  );

  await client.query(
    `UPDATE vc_firms vc SET num_of_exits = (
    SELECT COUNT(company_id) FROM (
      SELECT DISTINCT t1.company_id, t2.vc_firm_id
      FROM investment_rounds AS t1 INNER JOIN investments AS t2
      ON t1.id = t2.round_id
      WHERE t1.company_id IS NOT NULL AND t2.vc_firm_id IS NOT NULL
        AND t1.round IN ('Acquisition', 'Token Round')
    ) AS t3 
    WHERE vc_firm_id = vc.id
    GROUP BY vc_firm_id)`,
    [],
  );

  await client.query(
    `UPDATE vc_firms vc SET investment_amount_total = (SELECT SUM(investment_rounds.amount)
    FROM investments
    INNER JOIN investment_rounds
      ON investments.round_id = investment_rounds.id
    WHERE
      investments.vc_firm_id = vc.id)`,
    [],
  );

  await client.query(
    `UPDATE vc_firms vc SET team_size = (SELECT count(*)
    FROM investors
    WHERE
    investors.vc_firm_id = vc.id)`,
    [],
  );

  // Reset invested companies of porfolio lists
  await client.query(
    `DELETE FROM follows WHERE list_id IN
    (SELECT id FROM lists WHERE "type" = 'portfolio')
    AND resource_type = 'companies'`,
    [],
  );

  // Add invested companies of vc_firms into follows table
  await client.query(
    `INSERT INTO follows (resource_type, list_id, resource_id, created_by_user_id)
    (
    SELECT 'companies', t1.id, t4.company_id, t1.created_by_id FROM (
      SELECT id, created_by_id FROM lists WHERE "type" = 'portfolio') AS t1
      INNER JOIN follows_vc_firms AS t2 ON t1.id = t2.list_id
      INNER JOIN investments AS t3 ON t2.resource_id = t3.vc_firm_id
      INNER JOIN investment_rounds AS t4 ON t3.round_id = t4.id
    WHERE t3.vc_firm_id IS NOT NULL AND t4.company_id IS NOT NULL
  ) ON CONFLICT DO NOTHING`,
    [],
  );

  res.send({ success: true });
};

export default handler;
