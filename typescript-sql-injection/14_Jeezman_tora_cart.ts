import { Request, Response, NextFunction } from 'express';
import knex from '../db/knex';
import { validationResult } from 'express-validator';
import { DB } from '../interfaces/Db';
import { responseSuccess, responseErrorValidation, responseError } from '../helpers';
import { v4 } from 'uuid';

// Controller for adding to cart
export const addToCart = async (req: Request, res: Response, next: NextFunction): Promise<any> => {
    try {
        // Finds the validation errors in this request and wraps them in an object with handy functions
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return responseErrorValidation(res, 400, errors.array());
        }

        const productId: number = req.body.productId;
        const itemCount: number = req.body.itemCount;
        const amount: number = req.body.amount;
        const total: number = req.body.total;
        const buyerPubKey: string = String(req.body.buyerPubKey);
        const buyerUsername: string = String(req.socket.remoteAddress);
        const storeId: number = req.body.storeId;

        // If the product is already added update it and return
        const cartExist = await knex<DB.Cart>('Carts').where({ buyerUsername, productId }).orWhere({ buyerPubKey, productId });

        if (cartExist.length > 0) {
            await knex<DB.Cart>('Carts').update({ total: knex.raw(`total +  ${itemCount * amount}`), itemCount: cartExist[0].itemCount + itemCount }).where({ buyerUsername, productId }).orWhere({ buyerPubKey, productId });

            return responseSuccess(res, 200, 'Updated to cart successfully', {});
        } else {

            // Get or create cartId
            const cartIdCheck: DB.CartId[] = await knex<DB.CartId>('CartId').where({ buyerUsername, status: 'active' });

            if (cartIdCheck.length === 0) {
                const cartId = v4().substring(0, 10);
                await knex<DB.CartId>('CartId').insert({ buyerUsername, cartId });
            }

            if (buyerPubKey && buyerPubKey !== 'undefined') {
                const cartId = await knex<DB.CartId>('CartId').where({ buyerUsername: buyerPubKey, status: 'active' }).orderBy(
                    'id', 'desc'
                );

                await knex<DB.Cart>('Carts').insert({ buyerPubKey, cartId: cartId[0].cartId, productId, itemCount, amount, total, storeId });
            } else {
                const cartId = await knex<DB.CartId>('CartId').where({ buyerUsername, status: 'active' }).orderBy(
                    'id', 'desc'
                );

                await knex<DB.Cart>('Carts').insert({ buyerUsername, productId, cartId: cartId[0].cartId, itemCount, amount, total, storeId });
            }

            return responseSuccess(res, 200, 'Added to cart successfully', {});

        }

    } catch (err) {
        next(err);
    }
};

// Controller for updating cart product
export const updateCart = async (req: Request, res: Response, next: NextFunction): Promise<any> => {
    try {
        // Finds the validation errors in this request and wraps them in an object with handy functions
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return responseErrorValidation(res, 400, errors.array());
        }

        const productId: number = req.body.productId;
        const cartId: string = req.body.cartId;
        const itemCount: number = req.body.itemCount;
        const amount: number = req.body.amount;
        const total: number = req.body.total;
        const buyerPubKey: string = req.body.buyerPubKey;
        const buyerUsername: string = String(req.socket.remoteAddress);

        if (buyerPubKey) {
            await knex<DB.Cart>('Carts').update({ itemCount, amount, total }).where({ buyerPubKey, productId, cartId });
        } else {
            await knex<DB.Cart>('Carts').update({ itemCount, amount, total }).where({ buyerUsername, productId, cartId })
        }

        return responseSuccess(res, 200, 'Updated from cart successfully', {});

    } catch (err) {
        next(err);
    }
};

// Controller for updating cart product
export const listCart = async (req: Request, res: Response, next: NextFunction): Promise<any> => {
    try {
        // Finds the validation errors in this request and wraps them in an object with handy functions
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return responseErrorValidation(res, 400, errors.array());
        }

        let carts: DB.Cart[]
        const buyerPubKey: string = String(req.query.buyerPubKey);
        const buyerUsername: string = String(req.socket.remoteAddress);

        if (buyerPubKey !== 'undefined') {
            carts = await knex<DB.Cart>('Carts').where({ buyerPubKey });
        } else {
            carts = await knex<DB.Cart>('Carts').where({ buyerUsername });
        }

        // Add cart products to cart
        for(let cart of carts) {
            const cartProducts: DB.Product | undefined = await knex<DB.Product>('Products').where({ productId: cart.productId }).first();

            if (cartProducts) {
                cart.product = cartProducts;
            }
        }

        return responseSuccess(res, 200, 'Listed user cart successfully', carts);

    } catch (err) {
        next(err);
    }
};

// Controller for deleting from cart
export const deleteFromCart = async (req: Request, res: Response, next: NextFunction): Promise<any> => {
    try {
        // Finds the validation errors in this request and wraps them in an object with handy functions
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return responseErrorValidation(res, 400, errors.array());
        }

        const productId: number = req.body.productId;
        const buyerPubKey: string = req.body.buyerPubKey;
        const buyerUsername: string = String(req.socket.remoteAddress);
        const cartId: string = req.body.cartId;

        if (buyerPubKey) {
            await knex<DB.Cart>('Carts').delete().where({ buyerPubKey, productId, cartId });
        } else {
            await knex<DB.Cart>('Carts').delete().where({ buyerUsername, productId, cartId })
        }

        return responseSuccess(res, 200, 'Deleted from cart successfully', {});

    } catch (err) {
        next(err);
    }
};

// Controller for deleting  all items from cart
export const deleteAllFromCart = async (req: Request, res: Response, next: NextFunction): Promise<any> => {
    try {
        // Finds the validation errors in this request and wraps them in an object with handy functions
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return responseErrorValidation(res, 400, errors.array());
        }

        const buyerPubKey: string = req.body.buyerPubKey;
        const buyerUsername: string = String(req.socket.remoteAddress);
        const cartId: string = req.body.cartId;

        if (buyerPubKey) {
            await knex<DB.Cart>('Carts').delete().where({ buyerPubKey, cartId });
        } else {
            await knex<DB.Cart>('Carts').delete().where({ buyerUsername, cartId })
        }

        // update cartId to closed
        await knex<DB.CartId>('CartId').update({ status: 'closed' }).where({ cartId });

        return responseSuccess(res, 200, 'Deleted all from cart successfully', {});

    } catch (err) {
        next(err);
    }
};

// Controller for checking out cart
export const cartCheckout = async (req: Request, res: Response, next: NextFunction): Promise<any> => {
    try {
        // Finds the validation errors in this request and wraps them in an object with handy functions
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return responseErrorValidation(res, 400, errors.array());
        }

        const cartId: string = req.body.cartId;
        const email: string = req.body.email;
        const phoneNumber: string = req.body.phoneNumber;
        const address: string = req.body.address;
        const storeName: string = req.body.storeName;

        const orderId: string = v4().substring(0, 12).replace(/\-|\./g, '');

        // Get storeId
        const stores: DB.Store[] = await knex<DB.Store>('Stores').where({ name: storeName });

        if (stores.length === 0) return responseError(res, 404, 'Store not found');

        // Get storeId
        const storeId = stores[0].storeId;

        // Get all items with cartId
        const cartItems: DB.Cart[] = await knex<DB.Cart>('Carts').where({ cartId });

        // Map cart data to order
        const orderItems = cartItems.map(item => {
            return {
                user: phoneNumber,
                orderId,
                productId: item.productId,
                itemAmount: item.amount,
                itemCount: item.itemCount,
                itemTotal: item.total,
                storeId: item.storeId,
            }
        });

        // Get the order total
        const orderTotal = orderItems.reduce((partial, item) => partial + item.itemTotal, 0);

        // Insert orderId in the database
        await knex<DB.Order>('Order').insert({ orderId, user: phoneNumber, storeId, orderTotal })
            .returning('orderId')
            .then(async () => {


                const orderDetails = {
                    orderId,
                    email,
                    phoneNumber,
                    address
                };

                // Insert in order
                await knex<DB.OrderItems>('OrderItems').insert(orderItems);

                // Inser order details 
                await knex<DB.OrderDetails>('OrderDetails').insert(orderDetails);

                // update cartId to closed
                await knex<DB.CartId>('CartId').update({ status: 'closed' }).where({ cartId });

                // delete all from cart
                await knex<DB.Cart>('Carts').where({ cartId }).delete();

                const data = {
                    orderTotal,
                    orderDetails,
                    orderItems
                }

                return responseSuccess(res, 200, 'Placed order successfully', data);
            })
            .catch(e => {
                return responseError(res, 403, 'Error occured while creating order');
            })
    } catch (err) {
        next(err);
    }
};