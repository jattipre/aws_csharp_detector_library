import connection from "../connection";
import express, { Request, Response } from "express";
const router = express.Router();

 router.get("/", (req: Request, res: Response) => {
  connection.query("SELECT * FROM posts", (err, results) => {
    res.json(results);
  });
});

router.post("/", async (req: Request, res: Response) => {  
  const post = req.body
  connection.query(`INSERT INTO posts VALUES ('${post.title}', '${post.postText}', '${post.userName}')`)
  res.json(post)
})

export default router;
