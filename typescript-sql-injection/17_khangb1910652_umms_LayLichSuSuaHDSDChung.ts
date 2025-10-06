import express from "express";
import { knex } from "database/knex";

const ApiError = require("../../api-error");
const router = express.Router();

export const LayLichSuSuaHDSDChung = () => {
    return router.get(
        "/",
        async (
            req: express.Request,
            res: express.Response,
            next: express.NextFunction
        ) => {
            try {
                const { MaDV, MaPhienBan } = req.query;
                const results = await knex.raw(`
                    SELECT *
                    FROM PHIENBANHDSD
                    JOIN CHITIETPHIENBANHDSD ON CHITIETPHIENBANHDSD.MAPHIENBAN = PHIENBANHDSD.MAPHIENBAN
                    JOIN HUONGDANSUDUNG ON CHITIETPHIENBANHDSD.MAHDSD = HUONGDANSUDUNG.MAHDSD
                    JOIN DICHVUSO ON DICHVUSO.MADV = HUONGDANSUDUNG.MADV
                    JOIN NGUOIDUNG ON NGUOIDUNG.MANGUOIDUNG = CHITIETPHIENBANHDSD.MANGUOIDUNG
                    WHERE DICHVUSO.MADV = ${MaDV} AND PHIENBANHDSD.MAPHIENBAN = ${MaPhienBan}
                    ORDER BY CHITIETPHIENBANHDSD.THOIGIANTAO ASC
                `);
                const array = [] as any;
                let firstResult = results[0]; // Get the first result

                results.forEach((row: any) => {
                    const formattedDate = new Date(row.THOIGIANTAO).toLocaleString("vi-VN", {
                        day: "2-digit",
                        month: "2-digit",
                        year: "numeric",
                        hour: "2-digit",
                        minute: "2-digit",
                        second: "2-digit",
                    });

                    const object = {
                        id: row.MAPHIENBAN,
                        title: row.TENTIEUDEPB,
                        timeOrigin: row.THOIGIANTAO,
                        time: formattedDate,
                        describe: row.MOTA,
                        url: row.DUONGDANFILE,
                        user: row.TENDANGNHAP,
                        edit: "", // Initialize edit field
                        revert: row.REVERT,
                    };
                    
                    if (object.id === firstResult.MAPHIENBAN) {
                        if (object.describe !== firstResult.MOTA) {
                            object.edit += 'Mô tả';
                        }
                        if (object.title !== firstResult.TENTIEUDEPB) {
                            object.edit += ', Tên tiêu đề';
                        }
                        if (object.url !== firstResult.DUONGDANFILE) {
                            object.edit += ', File';
                        }
                        
                        firstResult = row;
                    }
                    
                    array.push(object);
                });
                
                res.send(array.reverse());
            } catch (error) {
                console.log(error);
                return next(new ApiError(500, "Không có dữ liệu"));
            }
        }
    );
};
