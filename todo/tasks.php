<?php
    include_once("../db_connect.php");

    $order="";
    $from="";
    $to="";
    if(isset($_POST["order"])) $order=$_POST["order"];
    if(isset($_POST["from"]) && $_POST["from"] != "") $from=" AND `date` >= '".$_POST["from"]."' ";
    if(isset($_POST["to"]) && $_POST["to"] != "") $to=" AND `date` <= '".$_POST["to"]."' ";

    $query = "SELECT * FROM `tasks` WHERE `deleted`=false AND `completed`=false AND `userid`=(SELECT `id` FROM `users` WHERE `login`='".$_COOKIE["user"]."')".$from.$to.$order;
    $res_query = mysqli_query($connection,$query);

    if(!$res_query) exit("Ошибка в запросе");

    $rows = mysqli_num_rows($res_query);

    echo("<div class='active'><h1>Активные задачи</h1>");
    for ($i=0; $i < $rows; $i++){
        $row = mysqli_fetch_assoc($res_query);
        echo('<div class="task">'.
    '<form action="index.php" method="post" id="task">'.
    '<button type="submit" name="completedtaskid" id="task" value="'.$row["id"].'">Выполнено</button>'.
    '</form>'.
    '<p>'.$row["title"].'</p>'.
    '<p id="task"><a href="remove.php?id='.$row["id"].'">Удалить</a></p>'.
    '<p id="task"><a href="edit.php?id='.$row["id"].'">Редактировать</a></p>'.
    '<p class="description">'.$row["description"].'<br><br>'.$row["date"].'</p>'.
    '</div>');
    }
    echo("</div>");


    $query = "SELECT * FROM `tasks` WHERE `completed`=true";
    $res_query = mysqli_query($connection,$query);

    if(!$res_query) exit("Ошибка в запросе");

    $rows = mysqli_num_rows($res_query);

    echo("<div class='not-active'><h1>Выполненные задачи</h1>");
    for ($i=0; $i < $rows; $i++){
        $row = mysqli_fetch_assoc($res_query);
    echo('<div class="completed-task">
    <form action="index.php" method="post" id="task">
        <button type="submit" name="restoretaskid" id="task" value="'.$row["id"].'">Восстановить</button>
    </form>
    <p>'.$row["title"].'</p>
    <p class="description">'.$row["description"].'<br><br>'.$row["date"].'</p>
    </div>');
    }
    echo("</div>");
?>