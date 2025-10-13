import { Router, Request, Response, NextFunction } from "express";
import { Types } from "mongoose";
import validationMiddleware from "../middlewares/validation";
import userModel from "./users.model";
import CreateUserDto from "./users.dto";
import Controller from "../interfaces/controller";
import User from "../interfaces/iuser";
import UserNotFoundException from "../exceptions/UserNotFound";
import IdNotValidException from "../exceptions/IdNotValid";
import HttpError from "../exceptions/Http";
import authMiddleware from "../middlewares/auth";
import getDataFromCookie from "../utils/dataFromCookie";
export default class UserController implements Controller {
    path = "/users";
    router = Router();
    private user = userModel;

    constructor() {
        this.initializeRoutes();
    }

    private initializeRoutes() {
        this.router.get(this.path, authMiddleware, this.getAllUsers);
        this.router.get(`${this.path}/:id`, authMiddleware, this.getUserById);
        this.router.get(`${this.path}/:offset/:limit/:order/:sort/:keyword?`, authMiddleware, this.getPaginatedUsers);
        this.router.post(this.path, this.createUser);
        this.router.patch(`${this.path}/:id`, [validationMiddleware(CreateUserDto, true), authMiddleware], this.modifyUser);
        this.router.delete(`${this.path}/:id`, authMiddleware, this.deleteUser);
    }

    private getAllUsers = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const loggedUserId = getDataFromCookie(req);
            const loggedUser = this.user.findById(loggedUserId);
            if ((await loggedUser).role_name != "admin") return next(new HttpError(401, "You dont't have permisson to get that!"));

            const users = await this.user.find();
            res.send(users);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };

    private getUserById = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const loggedUserId = getDataFromCookie(req);
            const loggedUser = this.user.findById(loggedUserId);
            if ((await loggedUser).role_name != "admin") return next(new HttpError(401, "You dont't have permisson to get that!"));

            const id = req.params.id;
            if (!Types.ObjectId.isValid(id)) return next(new IdNotValidException(id));

            const user = await this.user.findById(id);
            if (!user) return next(new UserNotFoundException(id));

            res.send(user);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };

    private getPaginatedUsers = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const loggedUserId = getDataFromCookie(req);
            const loggedUser = this.user.findById(loggedUserId);
            if ((await loggedUser).role_name != "admin") return next(new HttpError(401, "You dont't have permisson to get that!"));

            const offset = parseInt(req.params.offset);
            const limit = parseInt(req.params.limit);
            const order = req.params.order; // order?
            const sort = parseInt(req.params.sort); // desc: -1  asc: 1
            let users = [];
            let count = 0;
            if (req.params.keyword) {
                const regex = new RegExp(req.params.keyword, "i"); // i for case insensitive
                count = await this.user.find({ $or: [{ userName: { $regex: regex } }, { description: { $regex: regex } }] }).count();
                users = await this.user
                    .find({ $or: [{ userName: { $regex: regex } }, { description: { $regex: regex } }] })
                    .sort(`${sort == -1 ? "-" : ""}${order}`)
                    .skip(offset)
                    .limit(limit);
            } else {
                count = await this.user.countDocuments();
                users = await this.user
                    .find({})
                    .sort(`${sort == -1 ? "-" : ""}${order}`)
                    .skip(offset)
                    .limit(limit);
            }
            res.send({ count: count, users: users });
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
    private createUser = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const loggedUserId = getDataFromCookie(req);
            const loggedUser = this.user.findById(loggedUserId);
            if ((await loggedUser).role_name != "admin") return next(new HttpError(401, "You dont't have permisson to do that!"));

            const userData = req.body;
            const newuser = await this.user.create({ ...userData });
            if (!newuser) return next(new HttpError(400, "Failed to create user"));
            res.send(newuser);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
    private modifyUser = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const loggedUserId = getDataFromCookie(req);
            const loggedUser = this.user.findById(loggedUserId);
            if ((await loggedUser).role_name != "admin" && loggedUserId != req.params.id) return next(new HttpError(401, "You dont't have permisson to do that!"));

            const id = req.params.id;
            if (!Types.ObjectId.isValid(id)) return next(new IdNotValidException(id));

            const userData: User = req.body;
            const user = await this.user.findByIdAndUpdate(id, userData, { new: true });
            if (!user) return next(new UserNotFoundException(id));

            res.send(user);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };

    private deleteUser = async (req: Request, res: Response, next: NextFunction) => {
        try {
            const loggedUserId = getDataFromCookie(req);
            const loggedUser = this.user.findById(loggedUserId);
            if ((await loggedUser).role_name != "admin" && loggedUserId != req.params.id) return next(new HttpError(401, "You dont't have permisson to do that!"));

            const id = req.params.id;
            if (!Types.ObjectId.isValid(id)) return next(new IdNotValidException(id));

            const successResponse = await this.user.findByIdAndDelete(id);
            if (!successResponse) return next(new UserNotFoundException(id));

            res.sendStatus(200);
        } catch (error) {
            next(new HttpError(400, error.message));
        }
    };
}
