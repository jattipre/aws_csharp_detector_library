import express from "express";
import { knex } from "database/knex";

const ApiError = require("../../api-error");
const router = express.Router();


export const LayChucNangTheoDichVuSo = () => {
    return router.get(
        "/",
        async (
            req: express.Request,
            res: express.Response,
            next: express.NextFunction
        ) => {
            try {
                const { MaDV } = req.query;
                const results = await knex.raw(`
                    SELECT *
                    FROM CHUCNANG
                    JOIN DICHVUSO ON DICHVUSO.MADV = CHUCNANG.MADV
                    JOIN PHANHECHUCNANG ON PHANHECHUCNANG.MAPHCN = CHUCNANG.MAPHCN
                    WHERE DICHVUSO.MADV = ${MaDV}
                `);
                res.send(results)
            } catch (error) {
                console.log(error);
                return next(new ApiError(500, "Không có dữ liệu"));
            }
        }
    );
};
