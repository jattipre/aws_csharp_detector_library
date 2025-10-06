import express from 'express';
import pool from '../database/db';
import axios from "axios";
import {updateCartTotalAmount} from "../lib/update-cart-total-amount";
import {findOrCreateCart} from "../lib/find-or-create-cart";
import crypto from 'crypto';

const router = express.Router();

// Get cart by token
router.get('/cart', async (req, res) => {
    try {
        const token = req.cookies?.cartToken || req.headers['x-cart-token'];

        if (!token) {
            return res.json({totalAmount: 0, items: []});
        }

        const client = await pool.connect();

        const result = await client.query(`
            SELECT c.id as cart_id,
                   c."totalAmount",
                   c.token,
                   COALESCE(
                                   json_agg(
                                   jsonb_build_object(
                                           'id', ci.id,
                                           'quantity', ci.quantity,
                                           'createdAt', ci."createdAt",
                                           'productItem', jsonb_build_object(
                                                   'id', pi.id,
                                                   'price', pi.price,
                                                   'size', pi.size,
                                                   'pizzaType', pi."pizzaType",
                                                   'product', jsonb_build_object(
                                                           'id', p.id,
                                                           'name', p.name,
                                                           'imageUrl', p."imageUrl",
                                                           'categoryId', p."categoryId"
                                                              )
                                                          ),
                                           'ingredients', COALESCE(ci_ingredients.ingredients, '[]'::json)
                                   )
                                   ORDER BY ci."createdAt" DESC
                                           ) FILTER (WHERE ci.id IS NOT NULL),
                                   '[]'
                   )    as items
            FROM "Cart" c
                     LEFT JOIN "CartItem" ci ON c.id = ci."cartId"
                     LEFT JOIN "ProductItem" pi ON ci."productItemId" = pi.id
                     LEFT JOIN "Product" p ON pi."productId" = p.id
                     LEFT JOIN (SELECT ci_inner.id as cart_item_id,
                                       json_agg(
                                               jsonb_build_object(
                                                       'id', i.id,
                                                       'name', i.name,
                                                       'price', i.price,
                                                       'imageUrl', i."imageUrl"
                                               )
                                       )           as ingredients
                                FROM "CartItem" ci_inner
                                         LEFT JOIN "_CartItemToIngredient" citi ON ci_inner.id = citi."A"
                                         LEFT JOIN "Ingredient" i ON citi."B" = i.id
                                WHERE i.id IS NOT NULL
                                GROUP BY ci_inner.id) ci_ingredients ON ci.id = ci_ingredients.cart_item_id
            WHERE c.token = $1
            GROUP BY c.id, c."totalAmount", c.token
        `, [token]);

        client.release();

        if (result.rows.length === 0) {
            return res.json({totalAmount: 0, items: []});
        }

        res.json(result.rows[0]);
    } catch (error) {
        console.error('[CART_GET] Server error:', error);
        res.status(500).json({message: 'Could not retrieve cart'});
    }
});

// Update cart item quantity
router.patch('/cart/:id', async (req, res) => {
    try {
        const id = parseInt(req.params.id);
        const {quantity} = req.body;
        const token = req.cookies?.cartToken || req.headers['x-cart-token'];

        if (!token) {
            return res.status(404).json({message: 'Cart not found'});
        }

        if (!quantity || quantity < 0) {
            return res.status(400).json({message: 'Invalid quantity'});
        }

        const client = await pool.connect();

        // Check if cart item exists and belongs to the cart with this token
        const checkResult = await client.query(`
            SELECT ci.id
            FROM "CartItem" ci
                     JOIN "Cart" c ON ci."cartId" = c.id
            WHERE ci.id = $1
              AND c.token = $2
        `, [id, token]);

        if (checkResult.rows.length === 0) {
            client.release();
            return res.status(404).json({message: 'Cart item not found'});
        }

        // Update quantity
        if (quantity === 0) {
            await client.query('DELETE FROM "CartItem" WHERE id = $1', [id]);
        } else {
            await client.query(`
                UPDATE "CartItem"
                SET quantity = $1
                WHERE id = $2
            `, [quantity, id]);
        }

        client.release();

        // Recalculate cart total using your function
        await updateCartTotalAmount(token);

        // Return updated cart
        const response = await axios.get(`${req.protocol}://${req.get('host')}/api/cart`, {
            headers: {'x-cart-token': token}
        });

        res.json(response.data);
    } catch (error) {
        console.error('[CART_PATCH] Server error:', error);
        res.status(500).json({message: 'Could not update cart item'});
    }
});

// Delete cart item
router.delete('/cart/:id', async (req, res) => {
    try {
        const id = parseInt(req.params.id);
        const token = req.cookies?.cartToken || req.headers['x-cart-token'];

        if (!token) {
            return res.status(404).json({message: 'Cart not found'});
        }

        const client = await pool.connect();

        // Check if cart item exists and belongs to the cart with this token
        const checkResult = await client.query(`
            SELECT ci.id
            FROM "CartItem" ci
                     JOIN "Cart" c ON ci."cartId" = c.id
            WHERE ci.id = $1
              AND c.token = $2
        `, [id, token]);

        if (checkResult.rows.length === 0) {
            client.release();
            return res.status(404).json({message: 'Cart item not found'});
        }

        // Delete cart item
        await client.query('DELETE FROM "CartItem" WHERE id = $1', [id]);
        client.release();

        // Recalculate cart total using your function
        await updateCartTotalAmount(token);

        res.json({message: 'Item removed from cart'});
    } catch (error) {
        console.error('[CART_DELETE] Server error:', error);
        res.status(500).json({message: 'Could not remove cart item'});
    }
});

// Add item to cart
router.post('/cart', async (req, res) => {
    try {
        let token = req.cookies?.cartToken || req.headers['x-cart-token'];

        if (!token) {
            token = crypto.randomUUID();
        }

        const {productItemId, ingredients} = req.body;

        if (!productItemId) {
            return res.status(400).json({message: 'Product item ID is required'});
        }

        // Use your findOrCreateCart function
        const userCart = await findOrCreateCart(token);
        const client = await pool.connect();

        // Check for existing cart item with same product and ingredients
        let findCartItemQuery = `
            SELECT ci.id, ci.quantity
            FROM "CartItem" ci
            WHERE ci."cartId" = $1
              AND ci."productItemId" = $2
        `;

        let queryParams = [userCart.id, productItemId];

        // If ingredients are provided, check for exact match
        if (ingredients && ingredients.length > 0) {
            findCartItemQuery += `
                AND NOT EXISTS (
                    SELECT 1 FROM "_CartItemToIngredient" citi1
                    WHERE citi1."A" = ci.id
                    AND citi1."B" NOT IN (${ingredients.map((_: number, i: number) => `$${i + 3}`).join(',')})
                )
                AND (
                    SELECT COUNT(*) FROM "_CartItemToIngredient" citi2
                    WHERE citi2."A" = ci.id
                ) = $${ingredients.length + 3}
            `;
            queryParams = [...queryParams, ...ingredients, ingredients.length];
        } else {
            // No ingredients - find items with no ingredients
            findCartItemQuery += `
                AND NOT EXISTS (
                    SELECT 1 FROM "_CartItemToIngredient" citi
                    WHERE citi."A" = ci.id
                )
            `;
        }

        const existingItemResult = await client.query(findCartItemQuery, queryParams);

        let cartItemId;

        if (existingItemResult.rows.length > 0) {
            // Update existing item quantity
            const existingItem = existingItemResult.rows[0];
            cartItemId = existingItem.id;

            await client.query(`
                UPDATE "CartItem"
                SET quantity = quantity + 1
                WHERE id = $1
            `, [cartItemId]);
        } else {
            // Create new cart item
            const newItemResult = await client.query(`
                INSERT INTO "CartItem" ("cartId", "productItemId", quantity)
                VALUES ($1, $2, 1)
                RETURNING id
            `, [userCart.id, productItemId]);

            cartItemId = newItemResult.rows[0].id;

            // Add ingredients if provided
            if (ingredients && ingredients.length > 0) {
                for (const ingredientId of ingredients) {
                    await client.query(`
                        INSERT INTO "_CartItemToIngredient" ("A", "B")
                        VALUES ($1, $2)
                    `, [cartItemId, ingredientId]);
                }
            }
        }

        client.release();

        // Use your updateCartTotalAmount function
        const updatedCart = await updateCartTotalAmount(token);

        // Set cookie if token was generated
        if (!req.cookies?.cartToken && !req.headers['x-cart-token']) {
            res.cookie('cartToken', token, {
                httpOnly: true,
                secure: process.env.NODE_ENV === 'production',
                maxAge: 30 * 24 * 60 * 60 * 1000 // 30 days
            });
        }

        res.status(201).json(updatedCart);
    } catch (error) {
        console.error('[CART_POST] Server error:', error);
        res.status(500).json({message: 'Could not add item to cart'});
    }
});

export default router;