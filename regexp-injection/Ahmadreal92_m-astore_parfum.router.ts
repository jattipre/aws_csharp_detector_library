import {Router} from 'express';
import { sample_parfums, sample_tags } from '../data';
import asyncHandler from 'express-async-handler';
import { ParfumModel } from '../models/parfum.model';




const router = Router();

router.get("/seed", asyncHandler(
 async (req, res) => {
    const parfumsCount = await ParfumModel.countDocuments();
    if(parfumsCount> 0){
      res.send("Seed is already done!");
      return;
    }

    await ParfumModel.create(sample_parfums);
    res.send("Seed Is Done!");
}
))


router.get("/",asyncHandler(
  async (req, res) => {
    const parfums = await ParfumModel.find();
      res.send(parfums);
  }
))

router.get("/search/:searchTerm", asyncHandler(
  async (req, res) => {
    const searchRegex = new RegExp(req.params.searchTerm, 'i');
    const parfums = await ParfumModel.find({name: {$regex:searchRegex}})
    res.send(parfums);
  }
))

router.get("/tags", asyncHandler(
  async (req, res) => {
    const tags = await ParfumModel.aggregate([
      {
        $unwind:'$tags'
      },
      {
        $group:{
          _id: '$tags',
          count: {$sum: 1}
        }
      },
      {
        $project:{
          _id: 0,
          name:'$_id',
          count: '$count'
        }
      }
    ]).sort({count: -1});

    const all = {
      name : 'All',
      count: await ParfumModel.countDocuments()
    }

    tags.unshift(all);
    res.send(tags);
  }
))

router.get("/tag/:tagName",asyncHandler(
  async (req, res) => {
    const parfums = await ParfumModel.find({tags: req.params.tagName})
    res.send(parfums);
  }
))

router.get("/:parfumId", asyncHandler(
  async (req, res) => {
    const parfum = await ParfumModel.findById(req.params.parfumId);
    res.send(parfum);
  }
))


export default router;