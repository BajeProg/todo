<?php
    include_once("../db_connect.php");

    if(!isset($_GET["id"])) exit("ID don't set!");

    if(isset($_POST["name"])){

        $title = "";
        $desc = "";
        $arr = array();
        if($_POST["desc"] != "") array_push($arr, "`description`='".$_POST["desc"]."'");
        if($_POST["name"] != "") array_push($arr, "`title`='".$_POST["name"]."'");
        if(isset($_POST["completed"])) array_push($arr, "`completed`=true");
        else array_push($arr, "`completed`=false");
        array_push($arr, "`priority`=".$_POST["priority"]);

        $par = "";
        for($i = 0; $i < count($arr) - 1; $i++) $par = $par.$arr[$i].", ";
        $par = $par.$arr[count($arr) - 1];

        
        $query = "UPDATE `tasks` SET ".$par." WHERE `id`=".$_GET["id"];
        //exit($query);
        $res_query = mysqli_query($connection,$query);

        if(!$res_query) exit("Ошибка в запросе");

        header("Location: index.php");
        exit;
    }

    $json = file_get_contents("https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=list_tasks&id=".$_GET["id"]);
    $row = json_decode($json)->other[0];
?>

<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="styles/addTask.css">
    <title>TODO Edit</title>
</head>
<body>
    <h1>Редактировать задачу</h1>
    <form action="edit.php?id=<?echo($_GET["id"]);?>" method="post">
        <input type="text" name="name" id="name" placeholder="<?echo($row->title);?>">
        <textarea  type="text" name="desc" id="desc" placeholder="<?echo($row->description);?>"></textarea>
        <input type="checkbox" name="completed" id="c">
        <label for="c">Выполнена</label>
        <select id="priority" name="priority">
            <option value="0">Низкий</option>
            <option value="1">Средний</option>
            <option value="2">Высокий</option>
        </select>
        <button type="submit">Сохранить</button>
    </form>
</body>
</html>