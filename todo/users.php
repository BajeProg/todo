<?
session_start();
if(isset($_POST["user"])){
    file_get_contents("https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=change_right&user=".$_POST["user"]."&right=".$_POST["right"]);
    header("Location: users.php");
}
?>
<!DOCTYPE html>
<html lang="ru">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Task Management</title>
  <link rel="stylesheet" href="styles/index.css">
</head>
<body>
    <header>
    <ul>
            <li><a href="index.php">Задачи</a></li>
            <?if($_SESSION["right"] > 1){?>
            <li><a href="lists.php">Списки</a></li>
            <li><a href="users.php">Пользователи</a></li>
            <li><a href="../">API</a></li>
            <?}?>
            <li><a href="logout.php">Выход</a></li>
        </ul>
    </header>
  <h1>Task Management</h1>

  <hr>

  <!-- Список задач -->
  <h2>Список пользователей</h2>
  <table>
    <thead border="0">
      <tr>
        <th>Пользователь</th>
        <th>Права доступа</th>
        <th>Изменить</th>
        <!--<th>Пользователи с доступом к списку:</th>
        <th>Действие</th>-->
      </tr>
    </thead>
    <tbody id="list">
      <!-- Здесь будут отображаться задачи -->
    </tbody>
  </table>

  <script>

    function showUsers() {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
            var users = JSON.parse(this.responseText).other;
            var list = document.getElementById("list");
            for (var i = 0; i < users.length; i++) {
                var user = users[i];
                var tr = document.createElement("tr");
                var login = document.createElement("td");
                var right = document.createElement("td");
                var buttons = document.createElement("td");

                login.innerText = user.login;
                right.innerText = user.right;
                buttons.innerHTML = "<form method='post'><select name='right'><option value='1'>user</option>><option value='2'>admin</option>><option value='3'>redactor</option><select><button type='submit' name='user' value='"+user.id+"'>Поменять</button><form>";


                tr.appendChild(login);
                tr.appendChild(right);
                tr.appendChild(buttons);
                list.appendChild(tr);
            }
        }
        };
        xhttp.open("GET", "https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=list_users", true);
        xhttp.send();
    }
    showUsers();
  </script>