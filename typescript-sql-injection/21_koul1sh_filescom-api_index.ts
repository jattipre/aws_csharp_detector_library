import express from 'express';
import { v4 as uuidv4 } from 'uuid';
import { connection } from './db/index'
import bodyParser from 'body-parser'
import cors from 'cors'
const app = express();
const port = 3001;

app.use(bodyParser.urlencoded({ extended: false }))
app.use(bodyParser.json())
app.use(cors())

app.post('/createFile', async (req, res) => {

    const body = req.body

    connection.query('INSERT INTO file ( filename , fileurl, userid , prefix) VALUES (?, ?,?,?)', [body.name, body.url, 1, req.query.prefix], (err, result) => {
        if (err) {
            res.send(`Error inserting file: ${err}`);
        } else {
            if (result.affectedRows > 0) {
                res.send("file created...");
            } else {
                res.send("Failed to create file");
            }
        }
    })

})

app.get('/getFiles', (req, res) => {
    const userId = req.query.userid
    const prefix = req.query.prefix
    var query = `SELECT * FROM file WHERE userid = ? AND prefix LIKE ?`;
    // if(prefix!='/'){
    //     query = `SELECT File.filename , File.fileurl FROM FILE JOIN folder ON file.foldername = folder.foldername where file.userid = ${userId} AND folder.prefix like`
    // }
    connection.query(query, [userId, `%${prefix}`], (err, result) => {
        if (err) {

            res.status(500).send(`Error getting files: ${err}`);
        } else {
            if (result.length > 0) { // Check if result contains any data

                res.send(result);
            } else {
                res.send([{ name: '', url: '' }]);
            }
        }
    });
})

app.post('/createFolder', (req, res) => {
    const userId = req.query.userid;
    const folderName = req.query.foldername;
    const prefix = req.query.prefix

    const query = `INSERT INTO FOLDER (foldername, userid , prefix) VALUES (?, ?, ?)`; // Using parameterized query
    connection.query(query, [folderName, userId, prefix], (err, result) => {
        if (err) {

            res.status(500).send(`Error creating folder: ${err}`);
        } else {
            if (result.affectedRows > 0) {
                res.send("Folder created...");
            } else {
                res.send("Failed to create folder");
            }
        }
    });

})

app.get('/listFolders', (req, res) => {
    const userId = req.query.userid;
    const prefix = req.query.prefix


    // Assuming your table name is 'folder'
    const query = `SELECT * FROM folder WHERE userid = ? AND prefix LIKE ?`;

    connection.query(query, [userId, `%${prefix}`], (err, result) => {
        if (err) {

            res.status(500).send(`Error listing folders: ${err}`);
        } else {
            // Check if result contains data
            if (result.length > 0) {
                res.send(result);
            } else {
                res.send([]);
            }
        }
    });
});

app.delete('/deleteFile', (req, res) => {
    const fileName = req.query.filename
    const query = `DELETE FROM file WHERE filename = '${fileName}'`;
    connection.query(query, (err, result) => {
        if (err) {
            res.status(500).send(`Error deleting file , ${err}`)
        } else {
            res.send(result);
        }
    })
})

app.listen(port, () => {
    console.log(`Server is running at 🚀 http://localhost:${port}`)
})