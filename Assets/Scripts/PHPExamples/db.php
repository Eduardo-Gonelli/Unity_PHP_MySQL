<?php 

$server = "localhost";
$user = "root";
$pkey = "";
$db = "your database name";

$conexao = mysqli_connect($server, $user, $pkey, $db);


$query = "SELECT * FROM PLAYERS";
$consulta_players = mysqli_query($conexao, $query);
