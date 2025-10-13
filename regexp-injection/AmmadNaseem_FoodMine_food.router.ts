import { FoodModel } from './../models/food.model';
import {Router} from 'express';
import { sample_foods, sample_tags } from '../data';
import expressAsyncHandler from 'express-async-handler';

const router = Router();


router.get("/seed",expressAsyncHandler(
    async (req,res)=>{
        const foodsCount=await FoodModel.countDocuments();
        if (foodsCount>0) {
            res.send("Seed is already done!");
            return;
        }

        await FoodModel.create(sample_foods);
        res.send("Seed is done, goog job.");
    }
 )
);


router.get("/",expressAsyncHandler(
    async(req,res)=>{
        const foods=await FoodModel.find();
        res.send(foods);
    }
));

router.get("/search/:searchTerm",expressAsyncHandler(
    async (req,res)=>{
        // FIRST MAKE REGULAR EXPRESSION CASE INSENSITIVE
        const searchRegex=new RegExp(req.params.searchTerm,'i');
    
        const foods=await FoodModel.find({name:{$regex:searchRegex}});
    
        // const searchTerm=req.params.searchTerm;
        // const foods=sample_foods
        // .filter(food=>food.name.toLowerCase()
        // .includes(searchTerm.toLowerCase()));
    
        res.send(foods);
    }
));

// router.get("/tags",(req,res)=>{
//     res.send(sample_tags);
// });
router.get("/tags",expressAsyncHandler(
    async (req,res)=>{
        const tags=await FoodModel.aggregate([
            {
                $unwind:'$tags'//when we wants to put field then it start with dollar.
                // unwind do:we have 2 foods and each food has 3 tags, after unwind with tags going to 6 foods each food tags property only one item. UNWIND CONVERTS AN ARRAY JUST A NORMAL FIELD WITH ONE VALUE. 
                // IN THIS WAY WE CAN GROUP AND FIND SIMILAR  AND COUNT THEM.
                //  unwind tags=>6 foods tags 1
            },
            {
                $group:{
                    _id:'$tags',
                    count:{$sum:1} //SUM OPERATOR WITH THE VALUE OF ONE.
                }
            },
            {
                $project:{//FOR SHOWING THE FRONTEND
                    _id:0,
                    name:'$_id', //IT COMES FROM GROUP
                    count:'$count'
                }

            }
        ]).sort({count:-1}); //-1 means descending order and 1 means ascending order.

        const all={
            name:'All',
            count:await FoodModel.countDocuments()
        }

        // NOW I AM ADDING all{} AT BEGINNING TO OF THE TAG 
        tags.unshift(all);

        res.send(tags);
    }
));

router.get("/tag/:tagName",expressAsyncHandler(
    async (req,res)=>{
        const foods=await FoodModel.find({tags:req.params.tagName});
        // const tagName=req.params.tagName;
        // const foods=sample_foods
        // .filter(food=>food.tags?.includes(tagName));
    
        res.send(foods);
    }
));

router.get("/:foodId",expressAsyncHandler(
    async (req,res)=>{
        const food=await FoodModel.findById(req.params.foodId);
        // const foodId=req.params.foodId;
        // const food=sample_foods.find(food=>food.id==foodId);
    
        res.send(food);
    }
));

export default router;