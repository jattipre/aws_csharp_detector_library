import { NextApiRequest, NextApiResponse } from 'next';
import core from 'puppeteer-core';
import pWaitFor from 'p-wait-for';
import { getOptions } from './_lib/options';

const isDev = !process.env.AWS_REGION;
let _browser: core.Browser | null;

const sniffer = async (req: NextApiRequest, res: NextApiResponse) => {
  const list: string[] = [];
  const browser = await getBrowser(isDev);
  const page = await browser.newPage();
  try {
    const reg = new RegExp(req.query.regular as string);
    page.on('request', interceptedRequest => {
      if (reg.test(interceptedRequest.url())) {
        list.push(interceptedRequest.url());
      }
      interceptedRequest.continue();
    });
    await page.setRequestInterception(true);
    page.goto(req.query.url as string).catch(() => null);
    await pWaitFor(() => list.length > 0, { timeout: 10000 });
    res.setHeader('Cache-Control', 's-maxage=300, stale-while-revalidate');
    res.status(200).json(list);
    await page.close();
  } catch (e) {
    if (!page.isClosed()) {
      await page.close();
    }
    console.error(e);
    res.status(500).end();
  }
};

async function getBrowser(isDev: boolean) {
  if (_browser) {
    return _browser;
  }
  const options = await getOptions(isDev);
  _browser = await core.launch(options);
  return _browser;
}

export default sniffer;
