<?php
session_start();
include("connection.php");

$username = $_SESSION["username"];
$sql = "SELECT * FROM regi WHERE username='$username'";

$result = mysqli_query($conn, $sql);
$row = mysqli_fetch_assoc($result);

$username = $row['username'];
$phfirst = $row['phfirst'];
$phsecond = $row['phsecond'];
$phthird = $row['phthird'];
$phfour = $row['phfour'];

?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Upload Photos</title>
    <style>
        body {
            background-image: url('media/hindi-banner.jpg');
            background-repeat: no-repeat;
            background-attachment: fixed;
            background-position: center;
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            color: #333;
        }

        .container {
            width: 80%;
            margin: 0 auto;
            background-color: rgba(255, 255, 255, 0.8);
            padding: 30px;
            border-radius: 15px; /* Added rounded corners */
            box-shadow: 0px 0px 15px rgba(0, 0, 0, 0.2);
            text-align: center;
        }

        h2 {
            font-size: 24px;
            color: #444;
        }

        table {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }

        td {
            padding: 12px;
            text-align: center;
        }

        input[type="file"] {
            font-size: 16px;
            padding: 5px;
            background-color: #f4f4f4;
            border: 1px solid #ccc;
            border-radius: 5px; /* Added rounded corners */
        }

        input[type="submit"] {
            font-size: 18px;
            background-color: #4CAF50;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 15px; /* Added rounded corners */
            cursor: pointer;
            width: 100%;
        }

        input[type="submit"]:hover {
            background-color: #45a049;
        }

        img {
            border-radius: 5px; /* Added rounded corners to images */
        }

        .form-group {
            margin-bottom: 20px;
        }

        @media (max-width: 768px) {
            .container {
                width: 90%;
                padding: 20px;
            }

            h2 {
                font-size: 20px;
            }

            input[type="submit"] {
                width: 100%;
                font-size: 16px;
            }
        }
    </style>
</head>
<body>

<div class="container">
    <h2>You Can Add Your Photos Here</h2>
    <form action="" method="POST" enctype="multipart/form-data">
        <table align="center">
            <tr>
                <td><input type="text" name="n1" placeholder="Username" value="<?php echo $username; ?>" style="border-style:none;font-size:20px;" readonly></td>
            </tr>
            <tr class="form-group">
                <td>
                    <img src="photo/userphoto/<?php echo $phfirst; ?>" height="50" width="50"><br>
                    <input type="file" name="ph1">
                </td>
            </tr>
            <tr class="form-group">
                <td>
                    <img src="photo/userphoto/<?php echo $phsecond; ?>" height="50" width="50"><br>
                    <input type="file" name="ph2">
                </td>
            </tr>
            <tr class="form-group">
                <td>
                    <img src="photo/userphoto/<?php echo $phthird; ?>" height="50" width="50"><br>
                    <input type="file" name="ph3">
                </td>
            </tr>
            <tr class="form-group">
                <td>
                    <img src="photo/userphoto/<?php echo $phfour; ?>" height="50" width="50"><br>
                    <input type="file" name="ph4">
                </td>
            </tr>
            <tr>
                <td><input type="submit" name="submit" value="Add Photos"></td>
            </tr>
        </table>
    </form>
</div>

<?php
if (isset($_POST['submit'])) {
    $username = $_POST['n1'];

    $image1 = $_FILES['ph1']['name'];
    $ph1 = $_FILES['ph1']['tmp_name'];
    move_uploaded_file($ph1, "photo/userphoto/$image1");

    $image2 = $_FILES['ph2']['name'];
    $ph2 = $_FILES['ph2']['tmp_name'];
    move_uploaded_file($ph2, "photo/userphoto/$image2");

    $image3 = $_FILES['ph3']['name'];
    $ph3 = $_FILES['ph3']['tmp_name'];
    move_uploaded_file($ph3, "photo/userphoto/$image3");

    $image4 = $_FILES['ph4']['name'];
    $ph4 = $_FILES['ph4']['tmp_name'];
    move_uploaded_file($ph4, "photo/userphoto/$image4");

    $sql = "UPDATE regi SET phfirst='$image1', phsecond='$image2', phthird='$image3', phfour='$image4' WHERE username='$username'";
    if (mysqli_query($conn, $sql)) {
        echo "<script>alert('Photos Added Successfully');</script>";
    } else {
        echo "<script>alert('Error: " . mysqli_error($conn) . "');</script>";
    }
}
?>

</body>
</html>
