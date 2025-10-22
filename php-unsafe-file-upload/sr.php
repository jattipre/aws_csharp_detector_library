<?php
include 'dbconfig/config.php';
?>
<!doctype html>
<html>
<head>
<link rel="stylesheet"  href="style.css">
<script type="text/javascript">
function previewImage(){
	var ofReader=new FileReader();
	ofReader.readAsDataURL(document.getElementById("imglnk").files[0]);
	ofReader.onload=function(ofREvent)
	{
		document.getElementById("uploadpreview").src=ofREvent.target.result;
	};
	};
	</script>
</head>

<body style="background-color:#7f8c8d">
<div id="main-wrapper">
<center><h2>Register Form</h2></center>
<form class="myform" action="register.php" method="post" enctype="multipart/form-data">
<center><img  id="uploadpreview" class="contact" src="contact.png"/></center><br>
<center><input type="file" name="imglnk"  id="imglnk" accept=".jpg,.jpeg,.png" onchange="previewImage();"/></center>
<label><b>select user type</b></label><br>
<select name="type">
<option value="-1">select user type</option>
<option value="admin">admin</option>
<option value="user">user</option>
</select>
<br><br>
<label><b>full name</b></label><br>
<input type="text" name="fullname" class="inputvalues" placeholder="type your full name"><br><br>

<input type="date"  name="DOB" >
<br><br>
<label><b>qualification<b></label>
<select class="qualification" name="qualification">
<option value="btech">B.tech</option>
<option value="be">B.E</option>
<option value="bsc">B.Sc</option>
<option value="msc">M.Sc</option>
<option value="mtech">M.tech</option>
</select><br><br>
<label><b>username</b></label><br>
<input name="username" type="text" class="inputvalues" placeholder="Type your user name"><br><br>
<label><b>Password</b></label><br>
<input name="password" type="password" class="inputvalues" placeholder="Type your password"><br><br>
<label><b>Password</b></label><br>
<input name="cpassword" type="password" class="inputvalues" placeholder="confirm your password"><br><br>
<input name="submit_btn" type="submit" id="signup_btn" value="sign up"/><br>
<a href="LR.php"><input type="button" id="back_btn" value="Back"/></a>

</form>
<?php
if(isset($_POST['submit_btn']))
{
	//echo '<script type="text/javascript">alert("signup button clicked")</script>';
	$type=$_POST['type'];
	$fullname=$_POST['fullname'];
	$gender=$_POST['gender'];
	$qualification=$_POST['qualification'];
	$username=$_POST['username'];
	$password=$_POST['password'];
	$cpassword=$_POST['cpassword'];
	$img_name=$_FILES['imglnk']['name'];
	$img_size=$_FILES['imglnk']['size'];
	$img_tmp=$_FILES['imglnk']['tmp_name'];
	$directory='uploads/';
		$target_file=$directory.$img_name;
	if($password==$cpassword)
	{
		$encrypted_password=md5($password);
		$query="select * from userinfotable where username='$username'";
		$query_run=mysqli_query($con,$query);
		if(mysqli_num_rows($query_run)>0)
		{
			echo '<script type="text/javascript">alert("try another username")</script>';
		}
		else if(file_exists($target_file))
		{
			echo '<script type="text/javascript">alert("try another image")</script>';
		}
		else if($img_size>2097152)
		{
			echo '<script type="text/javascript">alert("image file greater than 2mb")</script>';
		}
			
		else
		{
			move_uploaded_file($img_tmp,$target_file);
			$query=" insert into userinfotable values('','$username','$fullname','$gender','$qualification','$encrypted_password','$target_file','$type')";
			$query_run=mysqli_query($con,$query);
			if($query_run)
			{
				echo '<script type="text/javascript">alert("user registerd ")</script>';
			}
			else{
				echo '<script type="text/javascript">alert("error ")</script>';
			}
			
		}
	}
	else{
		echo '<script type="text/javascript">alert("password and confirm password not match ")</script>';
		
	}
}
?>
</div>
</body>