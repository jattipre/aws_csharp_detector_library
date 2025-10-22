<?php
ini_set('display_errors', 1);
function back()
{
    header('Location: ' . $_SERVER['HTTP_REFERER']);
}
class Database
{
    private $host = "localhost";
    private $username = "root";
    private $password = "";
    private $database = "anggaran_2022";
    public $connection = "";

    function __construct()
    {
    }

    function connectDatabase()
    {
        $this->connection = mysqli_connect($this->host, $this->username, $this->password, $this->database);
        if (!$this->connection) {
            die("could not connect: " . mysql_error());
        }
    }

    function closeDatabase()
    {
        mysqli_close($this->connection);
    }
}
class Show extends Database
{
    function getName($id_user)
    {
        $this->connectDatabase();
        $query = "SELECT * FROM user where id_user='$id_user'";
        $result = mysqli_query($this->connection, $query);
        $data = mysqli_fetch_all($result, MYSQLI_ASSOC);
        mysqli_free_result($result);
        $this->closeDatabase();
        return $data;
    }
    function getTable($querys)
    {
        $this->connectDatabase();
        $query = $querys;
        $result = mysqli_query($this->connection, $query);
        $data = mysqli_fetch_all($result, MYSQLI_ASSOC);
        mysqli_free_result($result);
        $this->closeDatabase();
        return $data;
    }
    function saveData($query)
    {
        $this->connectDatabase();
        $result = mysqli_query($this->connection, $query);

        if (!$result) {
            die('Could not get data: ' . mysqli_connect_error());
        } else {
            echo "<script>alert('data berhasil ditambahkan')</script>";
        }

        $this->closeDatabase();
    }
    function saveManyData($query)
    {
        $this->connectDatabase();
        $result = mysqli_query($this->connection, $query);

        if (!$result) {
            die('Could not get data: ' . mysqli_connect_error());
        }

        $this->closeDatabase();
    }
    function updateData($query)
    {
        $this->connectDatabase();
        $result = mysqli_query($this->connection, $query);

        if (!$result) {
            die('Could not get data: ' . mysqli_connect_error());
        } else {
            echo "<script>alert('data berhasil diubah')</script>";
        }

        $this->closeDatabase();
    }

    function deleteData($query)
    {
        $this->connectDatabase();
        $result = mysqli_query($this->connection, $query);

        if (!$result) {
            die('Could not delete data: ' . mysqli_connect_error());
        }
    }
    function upload()
    {
        $namaFile = $_FILES['spj-file']['name'];
        $ukuranFile = $_FILES['spj-file']['size'];
        $error = $_FILES['spj-file']['error'];
        $tmpName = $_FILES['spj-file']['tmp_name'];

        //ceka pakah ada file yg diupload
        if ($error === 4) {
            echo "<script>
      alert('Pilih File terlebih Dahulu');
      </script>";
            return false;
        }

        // cek ukuran file
        if ($ukuranFile > 1000000000) {
            echo "<script>
      alert('Ukuran file terlalu besar');
      </script>";
            return false;
        }

        // upload file
        //generate nama baru
        $namaFile = uniqid() . '-' . $namaFile;

        move_uploaded_file($tmpName, 'file/' . $namaFile);
        return $namaFile;
    }
}

class Admin extends Database
{
    function login($email, $password)
    {
        $this->connectDatabase();
        $sql = "SELECT * FROM admin WHERE email_admin='$email' AND password_admin='$password'";
        $query = mysqli_query($this->connection, $sql);
        $data = mysqli_fetch_assoc($query);
        $this->closeDatabase();

        if (mysqli_num_rows($query) > 0) {
            $_SESSION['email_admin'] = $data['email_admin'];
            $_SESSION['id_admin'] = $data['id_admin'];

            setcookie("message", "", time() - 60);
            header("location: halAdmin.php");
            die($data);
            return 1;
        } else {
            return 0;
            //$db2=new User();
            //$db2->userLogin($email,$password);
        }
    }
}

class User extends Database
{
    function userLogin($email, $password)
    {
        $this->connectDatabase();
        $sql = "SELECT * FROM user WHERE email='$email' AND password='$password'";
        $query = mysqli_query($this->connection, $sql);
        $data = mysqli_fetch_assoc($query);
        $this->closeDatabase();
        if (mysqli_num_rows($query) < 1) {
            setcookie("message", "maaf, email atau password salah", time() + 10);
            header("location: login-form.php");
        } else {
            echo $data['email'] . $data['password'];
            $_SESSION['username'] = $data['username'];
            $_SESSION['email'] = $data['email'];
            $_SESSION['id_user'] = $data['id_user'];

            setcookie("message", "", time() - 60);
            header("location: halUser.php");
        }
    }

    function getUserInfo()
    {
        $this->connectDatabase();
        $query = "SELECT * from user";
        $result = mysqli_query($this->connection, $query);
        $data = mysqli_fetch_all($result, MYSQLI_ASSOC);
        mysqli_free_result($result);
        $this->closeDatabase();
        return $data;
    }
}
