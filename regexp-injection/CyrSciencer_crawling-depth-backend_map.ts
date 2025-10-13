import express from "express";
import { baseMap } from "../models/BaseMap";

console.log("🗺️ Map routes module loaded");

const router = express.Router();

router.post("/baseMap", async (req, res) => {
  console.log("📥 POST /api/baseMap - Creating new base map");
  console.log("📋 Request body:", JSON.stringify(req.body, null, 2));

  const newBaseMap = new baseMap(req.body);
  console.log("🏗️ New base map instance created:", newBaseMap);

  try {
    console.log("💾 Saving base map to database...");
    await newBaseMap.save();
    console.log("✅ Base map saved successfully:", newBaseMap);
    res.status(201).json(newBaseMap);
    console.log("📤 Response sent with status 201");
  } catch (error) {
    console.error("❌ Error saving base map:", error);
    console.error("🔍 Error details:", {
      name: (error as Error).name,
      message: (error as Error).message,
    });
    res.status(400).json({ message: (error as Error).message });
    console.log("📤 Error response sent with status 400");
  }
});

router.get("/baseMap", async (req, res) => {
  console.log("📥 GET /api/baseMap - Retrieving base map");
  console.log("🔍 Search criteria:", req.body);

  try {
    console.log("🔎 Searching for maps with exit form:", req.body.exitForm);
    const foundBaseMaps = await baseMap.find({
      name: { $regex: new RegExp(req.body.exitForm, "i") },
    });
    console.log(`📊 Found ${foundBaseMaps.length} matching maps`);

    if (foundBaseMaps.length === 0) {
      console.log("⚠️ No maps found matching criteria");
      return res.status(404).json({ message: "No maps found" });
    }

    const randomIndex = Math.floor(Math.random() * foundBaseMaps.length);
    const selectedMap = foundBaseMaps[randomIndex];
    console.log(
      `🎲 Randomly selected map at index ${randomIndex}:`,
      selectedMap
    );

    res.status(200).json(selectedMap);
    console.log("📤 Response sent with status 200");
  } catch (error) {
    console.error("❌ Error retrieving base map:", error);
    console.error("🔍 Error details:", {
      name: (error as Error).name,
      message: (error as Error).message,
    });
    res.status(400).json({ message: (error as Error).message });
    console.log("📤 Error response sent with status 400");
  }
});

export default router;
