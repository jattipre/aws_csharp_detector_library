<?php

	include './config/koneksi.php';
	$conn = mysqli_connect("localhost","root","","mebook");
	$title = $_POST['title'];
		$author = $_POST['author'];
	$rating = $_POST['rating'];
		$pages = $_POST['pages'];
		$genre = $_POST['genre'];
		$category = $_POST['category'];
		$description = $_POST['description'];
		$status = $_POST['status'];
		    $image = upload();
	

	
		$sql = "INSERT INTO books_data (title, author, rating,pages,genre,category,description,image,status)
						VALUES ('$title', '$author', '$rating', '$pages', '$genre', '$category', '$description', '$image', '$status')";
						$result = mysqli_query($conn, $sql);
			if ($result) {
					echo "<script>alert('Selamat, input buku berhasil!')</script>";
					header("Location: ./admin.php");
				
				} else {
					echo "<script>alert('Woops! Terjadi kesalahan.')</script>";
				}
		

	header("location:./admin.php");


function upload() {

	$namaFile = $_FILES['image']['name'];
	$ukuranFile = $_FILES['image']['size'];
	$error = $_FILES['image']['error'];
	$tmpName = $_FILES['image']['tmp_name'];

	// cek apakah tidak ada gambar yang diupload
	if( $error === 4 ) {
		echo "<script>
				alert('pilih gambar terlebih dahulu!');
			  </script>";
		return false;
	}

	// cek apakah yang diupload adalah gambar
	$ekstensiGambarValid = ['jpg', 'jpeg', 'png'];
	$ekstensiGambar = explode('.', $namaFile);
	$ekstensiGambar = strtolower(end($ekstensiGambar));
	if( !in_array($ekstensiGambar, $ekstensiGambarValid) ) {
		echo "<script>
				alert('yang anda upload bukan gambar!');
			  </script>";
		return false;
	}

	// cek jika ukurannya terlalu besar
	if( $ukuranFile > 1000000 ) {
		echo "<script>
				alert('ukuran gambar terlalu besar!');
			  </script>";
		return false;
	}

	// lolos pengecekan, gambar siap diupload
	// generate nama gambar baru
	$namaFileBaru = uniqid();
	$namaFileBaru .= '.';
	$namaFileBaru .= $ekstensiGambar;
	//buatkanlah fungsi untuk memindahkan image yang sudah di upload ke folder img 
	move_uploaded_file($tmpName, '/img' . $namaFileBaru);


// 	if (move_uploaded_file($tmpName, 'img/' . $namaFileBaru)) {
//     echo 'File berhasil diupload!';
// } else {
//     echo 'Gagal mengupload file. Error: ' . $_FILES['image']['error'];
// }

	return $namaFileBaru;
}

?>


