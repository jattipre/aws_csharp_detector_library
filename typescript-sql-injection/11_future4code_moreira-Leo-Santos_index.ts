import app from './app';
import connection from './connection';

// Exercício 3
/* a)
 */
app.get('/actor/:id', async (req, res) => {
    try {
        const {id} = req.params;
        const result = await connection('actors').where('id', id);

        res.status(200).send(result);
    } 
    catch(err: any){
        res.status(400).send({message: err.message})
    };
});

/* b)
 */
app.get('/actor/by/gender', async (req, res) => {
    try{
        const {gender} = req.query;
        const result = await connection('actors').where('gender', `${gender}`).count(`* as ${gender}`);
        res.status(200).send(result);
    }
    catch(err: any){
        res.status(400).send({message: err.message})
    };    
});

// Exercício 1
/* a)
 * A resposta que chega do metodo raw é um Promise contendo
 * informações da tabela de dados junto com o resultado esperado em si no caso este ocupa a posição result[0]
 */

/* b) 
 */
app.get('/actor/where/name', async (req, res) => {
    try{
        // usando knex.raw():
        const {name} = req.query;

        // const result = await connection.raw(
        //     `SELECT * FROM actors WHERE name LIKE "%${name}%";` 
        // );
        
        // usando QueryBuilder
        const result = await connection('actors').whereLike('name', `%${name}%`);
        res.status(200).send(result);     
    }
    catch(err){
        console.log(err);
    };
});

/* c) 
*/
app.get('/actor/gender/:gender', async (req, res) => {
    try {
        const {gender} = req.params;

        // usando knex.raw()
        const result = await connection.raw(`
        SELECT COUNT(*) AS "${gender}" FROM actors WHERE gender = "${gender}" ORDER BY gender;
        `);
        res.status(200).send(result[0]);
        
    } 
    catch(err){
        console.log(err);
    };
});

// Exercício 2
/* c) - coloquei aqui por ser um GET
*/
app.get('/actor/salary/average', async (req, res) => {
    try {
        const {gender} = req.query;

        const result = await connection('actors').avg("salary as media salarial").where('gender', gender as string);

        res.status(200).send(result);
    } 
    catch(err){
        console.log(err);
    }
});

/* a)
*/
app.put('/actor', async (req, res) => {
    try{
        const {id, salary} = req.body;

        // usando queryBuilder

        await connection('actors').where({id: id}).update({salary: Number(salary)});
        
        res.status(200).send('Salary changed');
    }
    catch(err){
        console.log(err);
    };    
});

/* b)
*/
app.delete('/actor/delete/:id', async (req, res) => {
    try {
        const {id} = req.params;

        await connection('actors').where('id', id).del();
        res.status(200).send('actor deleted');
    } 
    catch(err){
        console.log(err);
    }
});
