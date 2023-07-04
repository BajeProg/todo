<?php
include_once("../db_connect.php");
include_once("../functions.php");

if(isset($_POST["login"]) && isset($_POST["pass"])){
$query = "SELECT * FROM `users` WHERE `login`='".$_POST['login']."' AND `password`='".$_POST['pass']."' ";
$res_query = mysqli_query($connection,$query);
if(!$res_query){
    exit("<p style='color: red;'>Ошибка в запросе!</p><meta http-equiv='refresh' content='2;URL=signin.php'/>");
}
$rows = mysqli_num_rows($res_query);
if($rows == 0) exit("<p style='color: red;'>Неправильный логин или пароль</p><meta http-equiv='refresh' content='2;URL=signin.php'/>");
$res = mysqli_fetch_assoc($res_query);

session_start();
session_regenerate_id();
$query = "INSERT INTO `todo_sessions`(`session_id`,`userid`) VALUES ('".session_id()."', ".$res["id"].")";
$res_query = mysqli_query($connection,$query);
if(!$res_query){
    exit("<p style='color: red;'>Ошибка в запросе!<br>".$query."</p><meta http-equiv='refresh' content='20;URL=signin.php'/>");
}

$_SESSION["user"] = $res["id"];
$_SESSION["right"] = $res["access_rights"];

header("Location: index.php");
exit;
}
?>
<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="./styles/signin.css">
    <title>TODO Sign In</title>
</head>
<body>
    <div class="container">
        <h1>Вход</h1>
        <form action="login.php" method="post">
            <input type="text" name="login" placeholder="Логин">
            <br><br>
            <input type="password" name="pass" placeholder="Пароль">
            <br><br>
            <button type="submit">Войти</button>
        </form>
        <br><br>
        <a href="register.php">Нет аккаунта? Зарегистрируйтесь!</a>
    </div>
</body>
</html>