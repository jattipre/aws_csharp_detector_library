import express from "express";
import bodyParser from "body-parser";
import pg from "pg";
import session from "express-session";
import multer from "multer";
import fs from "fs";
import path from "path";
import { dirname } from "path";
import { fileURLToPath } from "url";

const __dirname = dirname(fileURLToPath(import.meta.url));

const app = express();

const port = 3000;

const db = new pg.Client({
  user: "postgres",
  host: "localhost",
  database: "RockFetch",
  password: "AshlinJoelSizzin",
  port: 5432,
});
db.connect();

app.use(express.static("public"));
app.use("/uploads", express.static("uploads"));

app.use(bodyParser.urlencoded({ extended: true }));

app.use(
  session({
    secret: "your_secret_key",
    resave: false,
    saveUninitialized: true,
    cookie: { secure: false },
  })
);

const publicUploadsDir = path.join(__dirname, "public", "uploads");
if (!fs.existsSync(publicUploadsDir)) {
  fs.mkdirSync(publicUploadsDir, { recursive: true });
}

const storage = multer.diskStorage({
  destination: function (req, file, cb) {
    cb(null, "uploads/");
  },
  filename: function (req, file, cb) {
    cb(null, Date.now() + "-" + file.originalname);
  },
});

const upload = multer({ storage: storage });

app.get("/", function (req, res) {
  res.sendFile(__dirname + "/public/index.html");
});

app.get("/credentials", function (req, res) {
  res.render("credentials.ejs");
});

app.post("/login", async function (req, res) {
  const { username, password } = req.body;
  try {
    const cartLengthResult = await db.query(
      "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
      [username]
    );
    const cart_length = cartLengthResult.rows[0].count;

    const query =
      "SELECT * FROM accountdb WHERE username = $1 AND password = $2";
    const result = await db.query(query, [username, password]);

    if (result.rows.length > 0) {
      req.session.username = username;
      const notificationCount = await db.query(
        "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
        [username]
      );
      const notification = notificationCount.rows[0].count;
      const notification_display = notification > 0;

      const notifications = await db.query(
        "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
        [username]
      );
      const notification_content = notifications.rows;

      res.render("welcome.ejs", {
        username,
        cart_length,
        notification_display,
        notification_content,
      });
    } else {
      res.render("invalid_credentials.ejs");
    }
  } catch (err) {
    console.error("Error executing query", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/homepage", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  res.render("welcome.ejs", {
    username: username,
    cart_length,
    notification_display,
    notification_content,
  });
});

app.get("/store", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM itemsdb ORDER BY created_at DESC";
    const result = await db.query(query);
    const items = result.rows;
    res.render("store.ejs", {
      items: items,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/sell_item", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM itemsdb ORDER BY created_at DESC";
    const result = await db.query(query);
    const items = result.rows;
    res.render("store_sell.ejs", {
      items: items,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/search_items", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const item = req.query.item;
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;

  try {
    const query = "SELECT * FROM itemsdb WHERE UPPER(item_name) = UPPER($1)";
    const result = await db.query(query, [item]);
    const items = result.rows;
    res.render("store.ejs", {
      items: items,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.log("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.post("/items_post", upload.single("image"), async function (req, res) {
  if (!req.file) {
    return res.status(400).send("No file uploaded.");
  }

  const name = req.session.username;
  const item_name = req.body.item_name;
  const price = req.body.price;
  const period = req.body.period;

  const uploadedFilePath = path.join(__dirname, "uploads", req.file.filename);
  const targetFilePath = path.join(
    __dirname,
    "public",
    "uploads",
    req.file.filename
  );

  const targetDir = path.dirname(targetFilePath);
  if (!fs.existsSync(targetDir)) {
    fs.mkdirSync(targetDir, { recursive: true });
  }

  fs.rename(uploadedFilePath, targetFilePath, (err) => {
    if (err) {
      console.error("Error moving file", err.stack);
      return res.status(500).send("Internal Server Error");
    }

    const imagePath = req.file.filename;
    const query = `
      INSERT INTO itemsdb (name, item_name, price, period, image_path)
      VALUES ($1, $2, $3, $4, $5)
    `;

    db.query(query, [name, item_name, price, period, imagePath])
      .then(() => {
        res.redirect("/store");
      })
      .catch((err) => {
        console.error("Error executing query", err.stack);
        res.status(500).send("Internal Server Error");
      });
  });
});

app.post("/add/item", async function (req, res) {
  const added_by = req.session.username;
  const name = req.body.name;
  const item_name = req.body.item_name;
  const period = req.body.period;
  const price = req.body.price;
  const image_path = req.body.image_path;

  try {
    const insertQuery = `
      INSERT INTO cart (name, item_name, period, price, image_path, added_by)
      VALUES ($1, $2, $3, $4, $5, $6)
    `;
    await db.query(insertQuery, [
      name,
      item_name,
      period,
      price,
      image_path,
      added_by,
    ]);

    const username = req.session.username;
    const selectQuery = "SELECT * FROM itemsdb";
    const result = await db.query(selectQuery);
    const items = result.rows;
    const cartLengthResult = await db.query(
      "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
      [username]
    );
    const cart_length = cartLengthResult.rows[0].count;
    const notificationCount = await db.query(
      "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
      [username]
    );
    const notification = notificationCount.rows[0].count;
    let notification_display = false;
    if (notification > 0) {
      notification_display = true;
    }
    const notifications = await db.query(
      "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
      [username]
    );
    const notification_content = notifications.rows;

    res.render("store.ejs", {
      clicked: true,
      selectedItem: item_name,
      items: items,
      username: added_by,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error executing query", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/car_pool", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM carpooldb ORDER BY created_at DESC";
    const result = await db.query(query);
    const travels = result.rows;
    res.render("car_pool.ejs", {
      travels: travels,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/travel_add", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM carpooldb ORDER BY created_at DESC";
    const result = await db.query(query);
    const travels = result.rows;
    res.render("car_pool_plan.ejs", {
      travels: travels,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/search_destination", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const place = req.query.place;
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;

  try {
    const query =
      "SELECT * FROM carpooldb WHERE UPPER(destination) = UPPER($1)";
    const result = await db.query(query, [place]);
    const travels = result.rows;
    res.render("car_pool.ejs", {
      travels: travels,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.log("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.post(
  "/carpool_plan_post",
  upload.single("image"),
  async function (req, res) {
    const name = req.session.username;
    const source = req.body.source;
    const destination = req.body.destination;
    const car_name = req.body.car_name;
    const car_type = req.body.car_type;
    const date = req.body.date;
    const time = req.body.time;
    const duration = req.body.duration;
    const price = req.body.price;
    const vacancies = req.body.vacancies;

    const query = `
      INSERT INTO carpooldb (name, source, destination, car_name, car_type, date, time, duration, price, vacancies)
      VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)
    `;

    db.query(query, [
      name,
      source,
      destination,
      car_name,
      car_type,
      date,
      duration,
      time,
      price,
      vacancies,
    ])
      .then(() => {
        res.redirect("/car_pool");
      })
      .catch((err) => {
        console.error("Error executing query", err.stack);
        res.status(500).send("Internal Server Error");
      });
  }
);

app.get("/networking", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM networkdb";
    const result = await db.query(query);
    const ideas = result.rows;
    res.render("networking.ejs", {
      ideas: ideas,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/networking_add", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM networkdb";
    const result = await db.query(query);
    const ideas = result.rows;
    res.render("networking_ideasub.ejs", {
      ideas: ideas,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/search_idea", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const domain = req.query.domain;
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;

  try {
    const query =
      "SELECT * FROM networkdb WHERE UPPER(project_domain) = UPPER($1)";
    const result = await db.query(query, [domain]);
    const ideas = result.rows;
    res.render("networking.ejs", {
      ideas: ideas,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.log("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.post(
  "/networking_add_post",
  upload.single("image"),
  async function (req, res) {
    const name = req.session.username;
    const project_name = req.body.project_name;
    const project_domain = req.body.project_domain;
    const outcome = req.body.outcome;
    const description = req.body.description;
    const requirement = req.body.requirement;
    const skills = req.body.skills;

    const query = `
      INSERT INTO networkdb (name, project_name, project_domain, outcome, description, requirement, skills)
      VALUES ($1, $2, $3, $4, $5, $6, $7)
    `;

    db.query(query, [
      name,
      project_name,
      project_domain,
      outcome,
      description,
      requirement,
      skills,
    ])
      .then(() => {
        res.redirect("/networking");
      })
      .catch((err) => {
        console.error("Error executing query", err.stack);
        res.status(500).send("Internal Server Error");
      });
  }
);

app.get("/my_items", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM itemsdb WHERE name=($1)";
    const result = await db.query(query, [username]);
    const items = result.rows;
    res.render("my_items.ejs", {
      items: items,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/my_journeys", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM carpooldb WHERE name=($1)";
    const result = await db.query(query, [username]);
    const travels = result.rows;
    res.render("my_journeys.ejs", {
      travels: travels,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/cart", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const totalAmount = await db.query(
    "SELECT SUM(price) AS total FROM cart WHERE added_by=($1)",
    [username]
  );
  const total = totalAmount.rows[0].total;
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;

  try {
    const query = "SELECT * FROM cart WHERE added_by = ($1)";
    const result = await db.query(query, [username]);
    const cart_result = result.rows;
    res.render("cart.ejs", {
      cart_length,
      cart_result,
      total: total,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.post("/delete/cart", async function (req, res) {
  const id = req.body.id;

  try {
    const query = "DELETE FROM cart where id=($1)";
    await db.query(query, [id]);
    res.redirect("/cart");
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.post("/buy/item", async function (req, res) {
  const username = req.session.username;
  const id = req.body.id;
  const name = req.body.name;
  const item_name = req.body.item_name;
  const period = req.body.period;
  const price = req.body.price;
  const added_by = req.body.added_by;
  const status = req.body.status;

  try {
    const insertQuery =
      "INSERT INTO placedorders(name, item_name, period, price, added_by, status) VALUES ($1, $2, $3, $4, $5, $6)";
    await db.query(insertQuery, [
      name,
      item_name,
      period,
      price,
      added_by,
      status,
    ]);
    const deleteQuery = "DELETE FROM cart where id=($1)";
    await db.query(deleteQuery, [id]);
    res.render("item_placed.ejs", { username: username, name: name });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.get("/my_ideas", async function (req, res) {
  const username = req.session.username;
  const cartLengthResult = await db.query(
    "SELECT COUNT(*) FROM cart WHERE added_by=($1)",
    [username]
  );
  const cart_length = cartLengthResult.rows[0].count;
  const notificationCount = await db.query(
    "SELECT COUNT(*) FROM placedorders where LOWER(name)=LOWER($1) and status='pending'",
    [username]
  );
  const notification = notificationCount.rows[0].count;
  let notification_display = false;
  if (notification > 0) {
    notification_display = true;
  }
  const notifications = await db.query(
    "SELECT * FROM placedorders WHERE name = ($1) and status='pending'",
    [username]
  );
  const notification_content = notifications.rows;
  try {
    const query = "SELECT * FROM networkdb WHERE LOWER(name)=LOWER($1)";
    const result = await db.query(query, [username]);
    const ideas = result.rows;
    res.render("my_ideas.ejs", {
      ideas: ideas,
      username: username,
      cart_length,
      notification_display,
      notification_content,
    });
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.post("/notification/action", async function (req, res) {
  const id = req.body.id;
  const status = req.body.status;

  const editQuery = "UPDATE placedorders SET status = ($1) WHERE id = ($2)";
  await db.query(editQuery, [status, id]);
  res.redirect("/homepage");
});

app.post("/delete_item", async function (req, res) {
  const username = req.session.username;
  const id = req.body.item_id;

  try {
    const query = "DELETE FROM itemsdb WHERE id=($1)";
    await db.query(query, [id]);
    res.redirect("/my_items");
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.post("/delete_journey", async function (req, res) {
  const username = req.session.username;
  const id = req.body.item_id;

  try {
    const query = "DELETE FROM carpooldb WHERE id=($1)";
    await db.query(query, [id]);
    res.redirect("/my_journeys");
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.post("/delete_idea", async function (req, res) {
  const username = req.session.username;
  const id = req.body.item_id;

  try {
    const query = "DELETE FROM networkdb WHERE id=($1)";
    await db.query(query, [id]);
    res.redirect("/my_ideas");
  } catch (err) {
    console.error("Error fetching items", err.stack);
    res.status(500).send("Internal Server Error");
  }
});

app.listen(port, function () {
  console.log(`Listening on port: ${port} `);
});
