import { Request, Response } from "express";
import User, { IUser, IUserData, UserRole } from "../../models/User";
import bcrypt from "bcryptjs";
import jwt from "jsonwebtoken";
import { isEmpty, pick } from "lodash";

type UserRegisterType = Pick<
  IUser,
  "email" | "password" | "role" | "userFullName" | "username"
> & {
  nik?: string;
};

const register = async (req: Request, res: Response) => {
  try {
    const { username, email, userFullName, password, role }: IUser = req.body;

    if (
      isEmpty(username) ||
      isEmpty(email) ||
      isEmpty(userFullName) ||
      isEmpty(password)
    ) {
      return res.status(400).json({ message: "Mandatory fields required" });
    }

    if (role === UserRole.PublicUser && isEmpty(req.body.userNIK)) {
      return res.status(400).json({ message: "Mandatory fields required" });
    }

    const isFoundEmail = await User.findOne({
      email: { $regex: new RegExp(req.body.email, "i") },
    });
    if (isFoundEmail) {
      return res.status(400).json({ message: "Email already used" });
    }

    const isFoundUsername = await User.findOne({
      username: { $regex: new RegExp(req.body.username, "i") },
    });

    if (isFoundUsername) {
      return res.status(400).json({ message: "Username already exists" });
    }

    if (role === UserRole.PublicUser) {
      const isFoundNik = await User.findOne({
        nik: req.body.userNIK,
      });

      if (isFoundNik) {
        return res.status(400).json({ message: "NIK already exists" });
      }
    }

    const newUserData: UserRegisterType = {
      username,
      email,
      userFullName,
      password: bcrypt.hashSync(password, 12),
      role: isEmpty(role) ? UserRole.PublicUser : role,
      nik: role === UserRole.PublicUser ? req.body.userNIK : null,
    };

    const newUserRecord = await User.create(newUserData);

    const returnValue: IUserData = pick(newUserRecord, [
      "_id",
      "username",
      "email",
      "userFullName",
      "role",
    ]);

    const token = jwt.sign(
      { userData: returnValue },
      process.env.JWT_SECRET as string,
      {
        expiresIn: "1h",
      }
    );

    res.status(201).json({ data: { user: returnValue, token: token } });
  } catch (err) {
    res.status(500).json({ message: err });
  }
};

export default register;
