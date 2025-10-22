<?php
session_start();
include("connection.php");
include("functions.php");
date_default_timezone_set("Asia/Kolkata");
$r=date("hisdmy");

if($_SERVER['REQUEST_METHOD'] == "POST")
{
    //something was posted
    $name=$_POST["name"];
    $des=$_POST["designation"];
    $dept=$_POST["branch"];
    $doj=$_POST["DOJ"];
    $noe=$_POST["NOE"];
    $dob=$_POST["DOB"];
    $UID=$_POST["UID"];
    $email=$_POST["email"];
    $number=$_POST["phone"];
    $address=$_POST["address"];
    $password=  $_POST["password"];
    $fname=     $_POST["fname"];
    $mname=     $_POST["mname"];
    $gender=    $_POST["gender"];
    $marital=   $_POST["marital"];
    $Religion=  $_POST["religion"];
    $nation=    $_POST["nation"];
    $profilepic=$r.$_FILES['profilepic']['name'];
    move_uploaded_file($_FILES['profilepic']['tmp_name'],'profilepics/'.$profilepic);
    $signpic=   $r.$_FILES['signpic']['name'];
    
    move_uploaded_file($_FILES['signpic']['tmp_name'],'signatures/'.$signpic);
    if(!mysqli_num_rows(mysqli_query($con,"select * from bio_data where uniqueid='$UID'"))>0)
    {
        if(!mysqli_num_rows(mysqli_query($con,"select * from bio_data where email='$email'"))>0){
            if(!mysqli_num_rows(mysqli_query($con,"select * from bio_data where phone='$number'"))>0){
                //if(!mysqli_num_rows(mysqli_query($con,"select * from bio_data where department='$dept'"))>0){

                    
                    $query = "INSERT INTO `bio_data`(`uniqueid`,`ID`,`name`, `designation`, `department`, `doj`, `noe`, `dob`,  `email`, `phone`, `address`, `password`, `fname`, `mname`, `Gender`, `Marital`, `Religion`, `Nationality`,`profile_pic`,`signpic`) 
                                            VALUES ('$r','$UID','$name','$des','$dept','$doj','$noe','$dob','$email','$number','$address','$password','$fname','$mname','$gender','$marital','$Religion','$nation','$profilepic','$signpic')";
        
                    $res=mysqli_query($con, $query);
                    echo $con->error;
                    if($res){
        
                       echo "<script>alert('User Created Successfully')</script>";
                       echo "<script>window.location.href='INDEX.php'</script>";                    
                        
                    }else{                        
                        echo $con->error;
                    }
                

            }else
            {
                echo "<script>alert('Existing Mobile Number')</script>";
                echo "<script>window.location.href='HODsignup.php?des=CSE'</script>";
            }
        }else
        {
            echo "<script>alert('Existing Email ')</script>";
            echo "<script>window.location.href='HODsignup.php?des=CSE'</script>";
        }
        //save to database
    }else
    {
        echo "<script>alert('Existing Unique ID')</script>";
        echo "<script>window.location.href='HODsignup.php?des=CSE'</script>";
    }
    
}