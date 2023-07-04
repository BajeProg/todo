<?php
session_start();
if(isset($_POST["title"])){
    $json = file_get_contents("https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=list_users");
    $users = json_decode($json)->other;
    
    file_get_contents("https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=add_list&title=".str_replace(" ", "+", $_POST["title"]));
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

  <!-- Форма для создания новой задачи -->
  <h2 onclick="showNewTaskForm(this);" id="clicable">Добавить список</h2>
  <form id="new-task-form" style="display: none;" class="filters" method="post">
    <label for="title">Заголовок:</label>
    <input type="text" id="title" name="title" required>

    <p>Пользователи с доступом к списку:</p>
    <span id="users">

    </span>

    <button type="submit">Добавить список</button>
  </form>

  <hr>

  <!-- Список задач -->
  <h2>Список задач</h2>
  <table>
    <thead border="0">
      <tr>
        <th>Список</th>
        <!--<th>Пользователи с доступом к списку:</th>
        <th>Действие</th>-->
      </tr>
    </thead>
    <tbody id="list">
      <!-- Здесь будут отображаться задачи -->
    </tbody>
  </table>

  <script>
    function showNewTaskForm(header){
        var form = document.getElementById('new-task-form');
        form.style.display = form.style.display == "none" ? "grid" : "none";
        header.innerText = header.innerText == "Добавить задачу" ? "Отмена" : "Добавить задачу";
    }

    function showLists() {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
            var tasks = JSON.parse(this.responseText).other;
            var table = document.getElementById("list");
            for (var i = 0; i < tasks.length; i++) {
                var task = tasks[i];
                var tr = document.createElement("tr");
                var td = document.createElement("td");
                td.innerText = task.name;
                var users = document.createElement("td");
                users.id = "users";
                var button = document.createElement("td");
                var a = document.createElement("a");



                tr.appendChild(td);
                table.appendChild(tr);
            }
        }
        };
        xhttp.open("GET", "https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=list_list_tasks", true);
        xhttp.send();
    }
    showLists();


    function showUsers() {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
            var users = JSON.parse(this.responseText).other;
            var list = document.getElementById("users");
            for (var i = 0; i < users.length; i++) {
                var user = users[i];
                var input = document.createElement("input");
                input.type = "checkbox";
                input.id = user.login;
                var label = document.createElement("label");
                label.for = input.id;
                label.innerText = user.login;


                list.appendChild(input);
                list.appendChild(label);
                list.innerHTML += "<br>";
            }
        }
        };
        xhttp.open("GET", "https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=list_users", true);
        xhttp.send();
    }
    showUsers();
  </script>
</body>
</html>
