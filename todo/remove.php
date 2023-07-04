<?php
if(!isset($_GET["id"])) exit("ID don't set!");

include_once("../db_connect.php");

$query = "UPDATE `tasks` SET `deleted`=true WHERE `id`=".$_GET["id"];
$res_query = mysqli_query($connection,$query);
if(!$res_query) exit("<p>Непредвиденная ошибка, попробуйте позже</p>");

header("Location: index.php");
exit;