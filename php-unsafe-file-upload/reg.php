<?php
// Establish a connection to the database
$dbHost = 'localhost';
$dbName = 'employee';
$dbUser = 'USER';
$dbPass = 'PASSWORD';

$db = new PDO("mysql:host=$dbHost;dbname=$dbName;charset=utf8", $dbUser, $dbPass);
$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
if (!$db) {
    die("Connection failed: " . mysqli_connect_error());
}

// Get form data from $_POST array
$gender = $_POST['gender'];
$firstName = $_POST['first_name'];
$lastName = $_POST['last_name'];
$firstEmail = $_POST['first_email'];
$secondEmail = $_POST['second_email'];
$city = $_POST['city'];
$state = $_POST['state'];
$zipCode = $_POST['zip_code'];
$photo = $_FILES['photo']['name'];

// SQL statement to insert the data into the database
$stmt = $db->prepare("INSERT INTO employee (gender, first_name, last_name, first_email, second_email, city, state, zip_code, photo) VALUES (:gender, :first_name, :last_name, :first_email, :second_email, :city, :state, :zip_code, :photo)");

// Bind values to the parameters in the SQL statement
$stmt->bindParam(':gender', $gender);
$stmt->bindParam(':first_name', $firstName);
$stmt->bindParam(':last_name', $lastName);
$stmt->bindParam(':first_email', $firstEmail);
$stmt->bindParam(':second_email', $secondEmail);
$stmt->bindParam(':city', $city);
$stmt->bindParam(':state', $state);
$stmt->bindParam(':zip_code', $zipCode);
$stmt->bindParam(':photo', $photo);

// Upload photo to the server
$photoFilePath = 'uploads/' . $photo;
if (!file_exists($photoFilePath)) {
    mkdir($photoFilePath, 0777, true);
}
$photo = $photoFilePath . basename($_FILES["photo"]["name"]);
// move_uploaded_file($_FILES['photo']['tmp_name'], $photoFilePath);
if (move_uploaded_file($_FILES["photo"]["tmp_name"], $photo)) {
    echo "The file ". htmlspecialchars( basename( $_FILES["photo"]["name"])). " has been uploaded.";
} else {
    echo "Sorry, there was an error uploading your file.";
}

// Execute the SQL statement
$stmt->execute();

// Redirect the user back to the form page
header('Location: index2.html');
?>
