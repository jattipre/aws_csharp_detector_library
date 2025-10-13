import { Request, Response } from "express";
import gameModel from "./game.schema";

export const GET_GAMES = async (req: Request, res: Response) => {
  try {
    const start =
      typeof req.query.start === "string" ? parseInt(req.query.start, 10) : 0;
    const gamesOnPage =
      typeof req.query.gamesOnPage === "string"
        ? parseInt(req.query.gamesOnPage, 10)
        : 1000;
    const games = await gameModel
      .find({ title: new RegExp(req.query.title as string, "i") })
      .sort({ [req.query.sortField as string]: -1 })
      .skip(start)
      .limit(gamesOnPage);
    const count = await gameModel.countDocuments({
      title: new RegExp(req.query.title as string, "i"),
    });
    return res.status(200).json({ message: "games found", games, count });
  } catch (error) {
    return res.status(500).json({ message: "something went wrong", error });
  }
};

export const GET_GAME_BY_ID = async (req: Request, res: Response) => {
  try {
    const game = await gameModel.findOne({ id: req.params.id });
    if (!game) {
      return res.status(404).json({ message: "game not found" });
    }
    return res.status(200).json({ message: "game found", game });
  } catch (error) {
    return res.status(500).json({ message: "something went wrong", error });
  }
};

export const GED_GAMES_COUNT = async (req: Request, res: Response) => {
  try {
    const count = await gameModel.countDocuments({
      title: new RegExp(req.query.title as string, "i"),
    });
    return res.status(200).json({ message: "games count found", count });
  } catch (error) {
    return res.status(500).json({ message: "something went wrong", error });
  }
};
