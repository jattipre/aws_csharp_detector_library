<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Excel To Mail</title>
<style type="text/css">
body {
	background-color: #1E7145;
}
body,td,th {
	text-decoration: blink;
	color: #FFFFFF;
}
.downleft {	background-image: url(img/4.png);
	background-repeat: no-repeat;
}
.downright {	background-image: url(img/3.png);
	background-repeat: no-repeat;
}
.upleft {	background-image: url(img/1.png);
	background-repeat: no-repeat;
	font-size: xx-small;
	text-decoration: none;
}
.upright {	background-image: url(img/2.png);
	background-repeat: no-repeat;
	font-size: xx-small;
	text-decoration: none;
}
#down {	background-image: url(img/6.png);
	background-repeat: repeat-x;
}
#left {	background-image: url(img/8.png);
	background-repeat: repeat-y;
}
#right {	background-image: url(img/7.png);
	background-repeat: repeat-y;
}
#up {	background-image: url(img/5.png);
	background-repeat: repeat-x;
	font-size: xx-small;
	text-decoration: none;
}
.Body2 {
	font-family: Tahoma, Geneva, sans-serif;
}
.Main_Div {
	font-family: Tahoma, Geneva, sans-serif;
	-webkit-transition: all 0s ease 0s;
	-moz-transition: all 0s ease 0s;
	-ms-transition: all 0s ease 0s;
	-o-transition: all 0s ease 0s;
	word-break: break-all;
}
.Table_C {
	font-family: Tahoma, Geneva, sans-serif;
	border: thin solid #FFF;
	-webkit-transition: all 0s ease 0s;
	-moz-transition: all 0s ease 0s;
	-ms-transition: all 0s ease 0s;
	-o-transition: all 0s ease 0s;
	transition: all 0s ease 0s;
	cursor: default;
	text-decoration: blink;
}
th {
	-webkit-transition: all 0s ease 0s;
	-moz-transition: all 0s ease 0s;
	-ms-transition: all 0s ease 0s;
	-o-transition: all 0s ease 0s;
	transition: all 0s ease 0s;
	text-decoration: blink;
}
th:hover {
	background-color: #FFF;
	color: #1E7145;
	cursor: default;
	text-decoration: blink;
}
.Link {
	font-family: Tahoma, Geneva, sans-serif;
	text-decoration: blink;
	-webkit-transition: all;
	-moz-transition: all;
	-ms-transition: all;
	-o-transition: all;
	transition: all;
}
.Link:hover {
	text-decoration: blink;
}
a:link {
	color: #FFFFFF;
	text-decoration: none;
}
a:visited {
	text-decoration: none;
	color: #FFFFFF;
}
a:hover {
	text-decoration: none;
	color: #1E7145;
}
a:active {
	text-decoration: none;
	color: #FFFFFF;
}
</style>
<script>
function set(x){
	document.getElementById('tt').value = x;
}
function seth(x){
	document.getElementById('mh').value = x;
}
</script>
</head>

<body>
<div align="center"><img src="img/image.png" width="876" height="256"/></div>
<br/>
<div align="center">
  
  <table border="0" cellspacing="0" cellpadding="0" align="center">
    <tr>
      <td width="15" height="15" class="upleft"></td>
      <td height="15" id="up"></td>
      <td width="15" height="15" class="upright"></td>
    </tr>
    <tr>
      <td width="15" id="left"></td>
      <td align="center" class="Body2">
      <div align="center">
      <img src="img/Lv2.png" width="766" height="122" /> </div></td>
      <td width="15" id="right"></td>
    </tr>
  </table>
  
  <table width="1024" border="0" align="center" cellpadding="0" cellspacing="0">
    <tr>
      <td width="15" height="15" class="upleft"></td>
      <td height="15" id="up"></td>
      <td width="15" height="15" class="upright"></td>
    </tr>
    <tr>
      <td width="15" id="left"></td>
      <td align="center" class="Body2"><div class="Main_Div"><font color="#FFFFFF">
      ستون ایمیل ها را انتخاب کنید
  </font>    
      
      
      <br /><?php
	  
	  
require_once 'reader.php';
$data = new Spreadsheet_Excel_Reader();
$data->setOutputEncoding('UTF8');
//$data->read($_FILES['fileaddress']['tmp_name']);
//error_reporting(E_ALL ^ E_NOTICE);

if ($_FILES["file"]["error"] > 0)
  {
  echo "Error: " . $_FILES["file"]["error"] . "<br>";
  }
else
  {
  if (file_exists("upload/" . $_FILES["file"]["name"]))
      {
      echo $_FILES["file"]["name"] . " already exists. ";
      unlink("upload/" . $_FILES["file"]["name"]);
	  }
    else
      {
      move_uploaded_file($_FILES["file"]["tmp_name"],
      "upload/" . $_FILES["file"]["name"]);
      //echo "Stored in: " . "upload/" . $_FILES["file"]["name"];
      }
    }
  
  $data->read("upload/" . $_FILES["file"]["name"]);
  
//echo $data->sheets[0]['cells'][3][3];

foreach ($data->sheets[0]['cells'] as $v1) {
    foreach ($v1 as $v2) {
        $y++;
    }
	$x++;
}
$y=$y/$x;
echo '<table border="1" cellpadding="1" cellspacing="1" class="Table_C"><tr>';
for ($j=1;$j<=$y;$j++){
		echo '<th scope="col" onclick="set('.$j.')"><a onclick="set('.$j.')">'.$j.'</a></th>';	
}
echo '</tr>';
for ($i=1;$i<=$x;$i++){
	echo '<tr>';
	for ($j=1;$j<=$y;$j++){
		$tempp = $data->sheets[0]['cells'][$i][$j];
		echo '<td align="center">'.$tempp.'</td>';
	}
	echo '</tr>';
}
echo '</table>';
echo '<script> seth("'."upload/" . $_FILES["file"]["name"].'");</script>';
//unlink("upload/" . $_FILES["file"]["name"]);
      ?>
      <p align="center">
	  
      <form action="3.php" method="post" name="Frm">
	  <input name="Et" type="text" readonly="readonly" id="tt" value="ستون ایمیل ها را انتخاب کنید"/>
	  <input type="hidden" value="fff" name="M" id="mh">
	  <?php
	  echo '<script> seth("'."upload/" . $_FILES["file"]["name"].'");</script>';
	  ?>
	<br />
	<input name="Sub" type="submit" value="Finish" />
	</form></p>
      </div>
      
      <br /></td>
      <td width="15" id="right"></td>
    </tr>
    <tr>
      <td width="15" height="15" class="downleft"></td>
      <td height="15" id="down"></td>
      <td width="15" height="15" class="downright"></td>
    </tr>
  </table>
</div>
</body>
</html>
