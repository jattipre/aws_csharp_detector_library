<?php 
session_start();
$_SESSION['scholarID'] = 1815003;
include 'connection.php';
$ScholarID = $_SESSION['scholarID'];
if (isset($_POST['upload'])) {
	# code...
	$x =  "file";
	$file_name = $_FILES[$x]['name'];
        $file_type = $_FILES[$x]['type'];
        $file_size = $_FILES[$x]['size'];
        $file_tem_loc = $_FILES[$x]['tmp_name'];
        $ext= explode('.',$file_name);
        $_SESSION['ext']= $fileext=strtolower(end($ext));
        $file_store = "InternCertificates/intern".$ScholarID.".pdf";
        echo "<script>alert('Update Picture');</script>";
        move_uploaded_file($file_tem_loc, $file_store);
	$CompanyName = $_POST['CompanyName'];
	$Duration = $_POST['Duration'];
	$sql = "INSERT INTO internshipdetails (ScholarID,CompanyName,Duration,Certificate,Status) VALUES ('$ScholarID','$CompanyName','$Duration','$file_store','0')";
	mysqli_query($con,$sql);
	echo '<script>alert("Sent for verification");</script>';
	echo '<script>window.location.href="student.php"</script>';
}
 ?>
 <!DOCTYPE html>
 <html >
 <head>
 	<title>
 		Internship Details	
 	</title>
 </head>
 <body>
 <div style="text-align: center;">
		<h1>
			Internship Details
		</h1>
 <form action="?" method="POST" enctype="multipart/form-data">
 	<p>
			Scholar Id : <?php echo $ScholarID; ?>
		</p>
		<p>
			Company Name :
			<input type="text" name="CompanyName">
		</p>
		<p>
			Duration : 
			<input type="number" name="Duration">
		</p>
		<p>
			
 	<input type="file" name="file">
		</p>

 	<input type="submit" name="upload" value="Nigga">

 </form>  
 <p>
		<script type="text/javascript">
			function goback() {
				window.history.back();
			}
		</script>
		<button onclick="goback()">Back</button>
	</p>
 </div><                                                                                                                                                                                                
 </body>
 </html>