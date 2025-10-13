import { Router, Request, Response, NextFunction } from "express";
import { Types } from "mongoose";
import validationMiddleware from "../middlewares/validation";
import productModel from "./products.model";
import CreateProductDto from "./products.dto";
import Controller from "../interfaces/controller";
import Product from "../interfaces/iproduct";
import IdNotValidException from "../exceptions/IdNotValid";
import HttpError from "../exceptions/Http";
import authMiddleware from "../middlewares/auth";
import getDataFromCookie from "../utils/dataFromCookie";
import userModel from "../users/users.model";
export default class ProductController implements Controller {
    public path = "/products";
    public router = Router();
    private product = productModel;
    private user = userModel;
    constructor() {
        this.initializeRoutes();
    }

    private initializeRoutes() {
        this.router.get(this.path, this.getAllProducts);
        this.router.get(`${this.path}/:id`, this.getProductById);
        this.router.get(`${this.path}/:offset/:limit/:order/:sort/:keyword?`, this.getPaginatedProducts);
        this.router.post(this.path, authMiddleware, this.createProduct);
        this.router.patch(`${this.path}/:id`, [validationMiddleware(CreateProductDto, true), authMiddleware], this.modifyProduct);
        this.router.delete(`${this.path}/:id`, authMiddleware, this.deleteProductById);
    }

    private getAllProducts = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const products = await this.product.find();
            res.send(products);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
    private getPaginatedProducts = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const offset = parseInt(req.params.offset);
            const limit = parseInt(req.params.limit);
            const order = req.params.order; // order?
            const sort = parseInt(req.params.sort); // desc: -1  asc: 1
            let products = [];
            let count = 0;
            if (req.params.keyword) {
                const regex = new RegExp(req.params.keyword, "i"); // i for case insensitive
                count = await this.product.find({ $or: [{ productName: { $regex: regex } }, { description: { $regex: regex } }] }).count();
                products = await this.product
                    .find({ $or: [{ productName: { $regex: regex } }, { description: { $regex: regex } }] })
                    .sort(`${sort == -1 ? "-" : ""}${order}`)
                    .skip(offset)
                    .limit(limit);
            } else {
                count = await this.product.countDocuments();
                products = await this.product
                    .find({})
                    .sort(`${sort == -1 ? "-" : ""}${order}`)
                    .skip(offset)
                    .limit(limit);
            }
            res.send({ count: count, products: products });
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };

    private getProductById = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const id: string = req.params.id;
            // if (!(await isIdValid(this.product, [productId], next))) return;

            const product = await this.product.findById(id);
            if (!product) return next(new HttpError(404, `Failed to get product by id ${id}`));

            res.send(product);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };

    private createProduct = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const userId = getDataFromCookie(req);
            const user = await this.user.findById(userId);
            if (user.role_name != "admin") return next(new HttpError(401, "You dont have permisson to do that!"));

            const productData = req.body;
            const newproduct = await this.product.create({ ...productData });
            if (!newproduct) return next(new HttpError(400, "Failed to create product"));
            res.send(newproduct);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
    private modifyProduct = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const userId = getDataFromCookie(req);
            const user = await this.user.findById(userId);
            if (user.role_name != "admin") return next(new HttpError(401, "You dont have permisson to do that!"));

            const id = req.params.id;
            if (!Types.ObjectId.isValid(id)) return next(new IdNotValidException(id));

            const productData: Product = req.body;
            const product = await this.product.findByIdAndUpdate(id, productData, { new: true });
            //if (!order) return next(new UserNotFoundException(id));

            res.send(product);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
    private deleteProductById = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const userId = getDataFromCookie(req);
            const user = await this.user.findById(userId);
            if (user.role_name != "admin") return next(new HttpError(401, "You dont have permisson to do that!"));

            const productId: string = req.params.id;
            // if (!(await isIdValid(this.product, [productId], next))) return;

            const response = await this.product.findByIdAndDelete(productId);
            if (!response) return next(new HttpError(404, `Failed to delete product by id ${productId}`));

            res.sendStatus(200);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
}
