const express = require("express");
const router = express.Router();
const Story = require("../models/Story"); 
const Comment = require("../models/Comments"); 
const User = require("../models/User");
const { body, validationResult } = require("express-validator"); 
const fetchuser = require("../middleware/fetchuser"); 

router.get("/stories/:category", fetchuser, async (req, res) => {
  try {
    const stories = await Story.find({ category: req.params.category });
    res.json(stories);
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

router.get("/stories", fetchuser, async (req, res) => {
  try {
    const stories = await Story.find();
    res.json(stories);
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

router.post("/story/:id/comments", fetchuser, [
  body('content', 'Content is required').notEmpty()
], async (req, res) => {
  const errors = validationResult(req);
  if (!errors.isEmpty()) {
    return res.status(400).json({ errors: errors.array() });
  }

  const { content } = req.body;
  const newComment = new Comment({ content, story: req.params.id, user: req.user.id });
  try {
    const savedComment = await newComment.save();

    const user = await User.findById(req.user.id);
    user.comments.push(savedComment.id);
    await user.save();

    const story = await Story.findById(req.params.id);
    story.comments.push(savedComment.id);
    await story.save();

    res.json(savedComment);
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

router.get("/story/:id/comments", fetchuser, async (req, res) => {
  try {
    const comments = await Comment.find({ story: req.params.id }).populate('user', 'username');
    res.json(comments);
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});


router.get("/user/:id", fetchuser, async (req, res) => {
  try {
    const user = await User.findById(req.params.id).select('-password').populate('submissions').populate('comments');
    res.json(user);
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

router.get("/search/:keyword", fetchuser, async (req, res) => {
  try {
    const regex = new RegExp(req.params.keyword, 'i');
    const stories = await Story.find({ content: regex });
    const comments = await Comment.find({ content: regex });
    res.json({ stories, comments });
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

router.post("/bookmark/:id", fetchuser, async (req, res) => {
  try {
    const user = await User.findById(req.user.id).select('-password');
    const story = await Story.findById(req.params.id);
    if (user.bookmarks.includes(req.params.id)) {
      return res.status(400).json({ error: "Bookmark already exists" });
    }
    user.bookmarks.push(req.params.id);
    story.bookmarks.push(req.user.id);
    await user.save();
    await story.save();
    res.json(user);
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

router.post("/story", fetchuser, [
  body('title', 'Title is required').notEmpty(),
  body('content', 'Content is required').notEmpty(),
  body('imageUrl', 'Image URL is required').notEmpty(),
  body('category', 'Category is required').notEmpty()
], async (req, res) => {
  const errors = validationResult(req);
  if (!errors.isEmpty()) {
    return res.status(400).json({ errors: errors.array() });
  }

  const { title, content, imageUrl, category } = req.body;
  const newStory = new Story({ title, content, imageUrl, category, user: req.user.id });
  try {
    const savedStory = await newStory.save();
    const user = await User.findById(req.user.id);
    user.submissions.push(savedStory.id);
    await user.save();
    res.json(savedStory);
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

router.delete("/story/:id", fetchuser, async (req, res) => {
  try {
    const story = await Story.findById(req.params.id);
    if (!story) {
      return res.status(404).json({ error: "Story not found" });
    }
    if (story.user.toString() !== req.user.id) {
      return res.status(401).json({ error: "Not authorized" });
    }
    await story.deleteOne();
    res.json({ message: "Story removed" });
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

router.put("/story/:id", fetchuser, async (req, res) => {
  const { title, content, imageUrl, category } = req.body;
  const updatedStory = {};
  if (title) updatedStory.title = title;
  if (content) updatedStory.content = content;
  if (imageUrl) updatedStory.imageUrl = imageUrl;
  if (category) updatedStory.category = category;

  try {
    let story = await Story.findById(req.params.id);
    if (!story) {
      return res.status(404).json({ error: "Story not found" });
    }
    if (story.user.toString() !== req.user.id) {
      return res.status(401).json({ error: "Not authorized" });
    }
    story = await Story.findByIdAndUpdate(req.params.id, { $set: updatedStory }, { new: true });
    res.json(story);
  } catch (error) {
    console.log(error.message);
    res.status(500).send("Some Error Occurred");
  }
});

module.exports = router; 