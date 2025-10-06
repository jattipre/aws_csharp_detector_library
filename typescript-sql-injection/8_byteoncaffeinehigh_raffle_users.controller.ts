import { fileNameGenerator } from './libs';
import { FastifyReply, FastifyRequest } from "fastify";

import * as fs from "fs";
import * as path from "path";

export async function getUsers(req: FastifyRequest, reply: FastifyReply) {
    const client = await req.pg.connect();
    const { rows: result } = await client.query("SELECT * FROM users;");
    return reply.code(200).send(result);
}

export async function getUserProfile(req: FastifyRequest, reply: FastifyReply) {
    const client = await req.pg.connect();

    const decodedToken = req.jwt.verify(req.cookies.access_token);
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-ignore
    const { rows: [result] } = await client.query(`SELECT * FROM users WHERE id = '${decodedToken.id}';`);
    return reply.code(200).send(result);
}


export async function editUserPhoto(req: FastifyRequest, reply: FastifyReply) {
    const client = await req.pg.connect();

    const decodedToken = req.jwt.verify(req.cookies.access_token);
   
    const data = await req.file();
    if (!data || !data.filename) {
        return reply.code(400).send({ error: "No image provided" });
    }
    const fileName = `${fileNameGenerator(new Date())}${path.extname(data.filename)}`;

    const uploadsPath = path.join(__dirname, '../../uploads', fileName);

    await data.file.pipe(fs.createWriteStream(uploadsPath));

    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-ignore
    await client.query(`UPDATE users SET photo = 'http://localhost:9200/uploads/${fileName}' WHERE id = '${decodedToken.id}';`);

    return reply.code(200).send({ path: `http://localhost:9200/uploads/${fileName}` });

}

// сохранять файлы с датой и крудом их убирать




