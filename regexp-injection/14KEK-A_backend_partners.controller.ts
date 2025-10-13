import { Router, Request, Response, NextFunction } from "express";
import { Types } from "mongoose";
import validationMiddleware from "../middlewares/validation";
import partnerModel from "./partners.model";
import CreatePartnerDto from "./partners.dto";
import Controller from "../interfaces/controller";
import Partner from "../interfaces/ipartner";
//import UserNotFoundException from "../exceptions/UserNotFound";
import IdNotValidException from "../exceptions/IdNotValid";
import HttpError from "../exceptions/Http";
import authMiddleware from "../middlewares/auth";
import userModel from "../users/users.model";
import getDataFromCookie from "../utils/dataFromCookie";

export default class ProductController implements Controller {
    path = "/partners";
    router = Router();
    private partner = partnerModel;
    private user = userModel;

    constructor() {
        this.initializeRoutes();
    }

    private initializeRoutes() {
        this.router.get(this.path, authMiddleware, this.getAllPartners);
        this.router.get(`${this.path}/:id`, authMiddleware, this.getPartnerById);
        this.router.get(`${this.path}/:offset/:limit/:order/:sort/:keyword?`, authMiddleware, this.getPaginatedPartners);
        this.router.patch(`${this.path}/:id`, [validationMiddleware(CreatePartnerDto, true), authMiddleware], this.modifyPartner);
        this.router.delete(`${this.path}/:id`, authMiddleware, this.deletePartner);
    }

    private getAllPartners = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const userId = getDataFromCookie(req);
            const user = await this.user.findById(userId);
            if (user.role_name == "user") return next(new HttpError(401, "You don't have permission to get that!"));

            const partners = await this.partner.find();
            res.send(partners);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
    private getPaginatedPartners = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const userId = getDataFromCookie(req);
            const user = await this.user.findById(userId);
            if (user.role_name == "user") return next(new HttpError(401, "You don't have permission to get that!"));

            const offset = parseInt(req.params.offset);
            const limit = parseInt(req.params.limit);
            const order = req.params.order; // order?
            const sort = parseInt(req.params.sort); // desc: -1  asc: 1
            let partners = [];
            let count = 0;
            if (req.params.keyword) {
                const regex = new RegExp(req.params.keyword, "i"); // i for case insensitive
                count = await this.partner.find({ $or: [{ partnerName: { $regex: regex } }, { description: { $regex: regex } }] }).count();
                partners = await this.partner
                    .find({ $or: [{ partnerName: { $regex: regex } }, { description: { $regex: regex } }] })
                    .sort(`${sort == -1 ? "-" : ""}${order}`)
                    .skip(offset)
                    .limit(limit);
            } else {
                count = await this.partner.countDocuments();
                partners = await this.partner
                    .find({})
                    .sort(`${sort == -1 ? "-" : ""}${order}`)
                    .skip(offset)
                    .limit(limit);
            }
            res.send({ count: count, partners: partners });
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };

    private getPartnerById = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const userId = getDataFromCookie(req);
            const user = await this.user.findById(userId);
            if (user.role_name == "user") return next(new HttpError(401, "You don't have permission to get that!"));

            const id = req.params.id;
            if (!Types.ObjectId.isValid(id)) return next(new IdNotValidException(id));

            const partner = await this.partner.findById(id);
            //if (!user) return next(new UserNotFoundException(id));

            res.send(partner);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };

    private modifyPartner = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const userId = getDataFromCookie(req);
            const user = await this.user.findById(userId);
            if (user.role_name == "user") return next(new HttpError(401, "You don't have permission to get that!"));

            const id = req.params.id;
            if (!Types.ObjectId.isValid(id)) return next(new IdNotValidException(id));

            const partnerData: Partner = req.body;
            const partner = await this.partner.findByIdAndUpdate(id, partnerData, { new: true });
            //if (!partner) return next(new UserNotFoundException(id));

            res.send(partner);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };

    private deletePartner = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const userId = getDataFromCookie(req);
            const user = await this.user.findById(userId);
            if (user.role_name == "user") return next(new HttpError(401, "You don't have permission to get that!"));

            const id = req.params.id;
            if (!Types.ObjectId.isValid(id)) return next(new IdNotValidException(id));

            //const successResponse = await this.partner.findByIdAndDelete(id);
            //if (!successResponse) return next(new UserNotFoundException(id));

            res.sendStatus(200);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
}
