<?php
require 'config/config.php';
require('model/functions.fn.php');

if (isset($_POST["what"]) && isset($_POST["who"])) {
	if (!empty($_POST["what"]) && !empty($_POST["who"])) {
		echo "pass1";
		//echo $_POST["what"];
		//prepare what image
		$img = $_POST['what'];
		$img = str_replace('data:image/png;base64,', '', $img);
		$img = str_replace(' ', '+', $img);
		$fileData = base64_decode($img);

		//echo $_POST["who"];
		//prepare who image
		$img2 = $_POST['who'];
		$img2 = str_replace('data:image/png;base64,', '', $img2);
		$img2 = str_replace(' ', '+', $img2);
		$fileData2 = base64_decode($img2);
		//saving
		$fileName1 = 'image/'.uniqid(true).'.png';
		file_put_contents($fileName1, $fileData2);
		$fileName2 = 'image/'.uniqid(true).'.png';
		file_put_contents($fileName2, $fileData);
		$photoObject = substr($fileName1, 6);
		$photoPerson = substr($fileName2, 6);
		$NameObject = $_POST['NameObject'];
		$NamePerson = $_POST['NamePerson'];
		insertSharedObject($db, $NameObject, $NamePerson, $photoObject, $photoPerson);
	}
	elseif (isset($_FILES['cameraObject'])&&$_FILES['cameraObject']['error']==0 && isset($_FILES['cameraPerson'])&&$_FILES['cameraPerson']['error']==0)
	{
		echo "pass2";
		if($_FILES['cameraObject']['size']<=2000000 && $_FILES['cameraPerson']['size']<=2000000)
		{
			$ext = strtolower(substr(strrchr($_FILES['cameraObject']['name'], '.'),1));
			$ext2 = strtolower(substr(strrchr($_FILES['cameraPerson']['name'], '.'),1));
			if($ext=="jpg" || $ext=="png" || $ext=="gif" || $ext2=="jpg" || $ext2=="png" || $ext2=="gif")
			{
				$tailleimage = getimagesize($_FILES['cameraObject']['tmp_name']);
				$tailleimage2 = getimagesize($_FILES['cameraPerson']['tmp_name']);
				if($tailleimage[0] <= 1000 && $tailleimage[1] <= 1000 && $tailleimage2[0] <= 1000 && $tailleimage2[1] <= 1000)
				{
					$image = "";
					$image2 = "";
					$path = 'image/'.uniqid(true).".".$ext;
					$path2 = 'image/'.uniqid(true).".".$ext;
					move_uploaded_file($_FILES['cameraObject']['tmp_name'],$path);
					move_uploaded_file($_FILES['cameraPerson']['tmp_name'],$path2);
					$image = substr($path, 6);
					$image2 = substr($path2, 6);
					header('Location: dashboard.php');
				}
			}
		}
	}
}

header('Location: dashboard.php');
//echo "<img src=".$_POST["object"].">";
?>