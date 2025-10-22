<?php

if((isset($_FILES['image'])) && (!empty($_FILES['image']))) {
if(is_uploaded_file($_FILES['image']['tmp_name'])) {
mysql_pconnect("localhost", "root", "");
mysql_select_db ("lib1");
$UploadName1 = $_FILES['image']['name'];
$UploadTmp1 = $_FILES['image']['tmp_name'];
$UploadType1 = $_FILES['image']['type'];
$FileSize1 = $_FILES['image']['size'];
$target_path1 = "book/".$UploadName1;
$bname = $_POST['bookname'];
$auth = $_POST['auth'];
$cat = $_POST['category'];
$lan = $_POST['lan'];
$dsc = $_POST['desc'];
$UploadName = $_FILES['pdf']['name'];
$UploadTmp = $_FILES['pdf']['tmp_name'];
$UploadType = $_FILES['pdf']['type'];
$FileSize = $_FILES['pdf']['size'];
$target_path = "Upload/".$UploadName;
if(!$UploadTmp1){
		die("No File Selected, Please Upload Again");
}
else
{
			move_uploaded_file($UploadTmp1, "../book/".$UploadName1);
}

if(!$UploadTmp){
		die("No File Selected, Please Upload Again");
}
else
{
			move_uploaded_file($UploadTmp, "../Upload/".$UploadName);
}
$sql = "INSERT INTO books(bname,authname,category,language,image,pdf,descript)
VALUES('$bname', '$auth', '$cat', '$lan', '$target_path1', '$target_path', '$dsc')";
$current_id = mysql_query($sql) or die("<b>Error:</b> Problem on Image Insert<br/>" . mysql_error());
if(isset($current_id)) {
header("Location: addbooks2.php?book=1");

}}}

?>
	