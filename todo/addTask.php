<?php
    if(isset($_POST["name"]) && isset($_POST["desc"]) && $_POST["name"] != ""){
        include_once("../db_connect.php");

        $desc = "null";
        if($_POST["desc"] != "") $desc = "'".$_POST["desc"]."'";

        $query = "INSERT INTO `tasks`(`title`, `description`,`userid`) VALUES ('".$_POST["name"]."', ".$desc.", (SELECT `id` FROM `users` WHERE `login`='".$_COOKIE["user"]."'))";
        $res_query = mysqli_query($connection,$query);

        if(!$res_query) exit("Ошибка в запросе");

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
    <link rel="stylesheet" href="styles/addTask.css">
    <title>New Task</title>
</head>
<body>
    <h1>Добавить задачу</h1>
    <form action="addTask.php" method="post">
        <input type="text" name="name" id="name" placeholder="Название">
        <textarea  type="text" name="desc" id="desc" placeholder="Описание"></textarea>
        <button type="submit">Сохранить</button>
    </form>
</body>
</html>