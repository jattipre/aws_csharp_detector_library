<?php
var_dump($_FILES);
echo '<br>';

var_dump($_POST);
echo '<br>';
$fechaHoraActual = date('Y-m-d_H-i-s-ms');
echo trim($fechaHoraActual);
echo '<br>';
$archivo = $_FILES['foto']['name']; // Cambia esto por tu archivo

// Obtener información sobre el archivo
$informacion = pathinfo($archivo);

// Obtener la extensión
$extension = $informacion['extension'];
echo '<br>';
var_dump($informacion);
echo '<br>';
echo "La extensión del archivo es: " . $extension; 
$ruta = "img/".$informacion['filename'].$fechaHoraActual.'.'.$informacion['extension'];
move_uploaded_file($_FILES['foto']['tmp_name'], $ruta);
echo '<br> RUTAAA: ';
echo "$ruta";
echo '<br>';

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <img src="<?= $ruta?>" alt="">  
</body>
</html>


