<?
session_start();
if(!isset($_SESSION["user"])){
  header("Location: login.php");
  exit;
}

if(isset($_POST["title"])){
file_get_contents("https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=add_task&title=".str_replace(" ", "+", $_POST["title"])."&desc=".str_replace(" ", "+", $_POST["description"])."&dl=".str_replace(" ", "+", $_POST["deadline"])."&user=".$_SESSION["user"]."&pr=".$_POST["priority"]);
header("Location: index.php");
}

$order = "";
$from = "";
$to = "";
if(isset($_POST["order"]) && $_POST["order"]!="") $order = "&order=".str_replace(" ", "+", $_POST["order"]);
if(isset($_POST["from"]) && $_POST["from"]!="") $from = "&from=".str_replace(" ", "+", $_POST["from"]);
if(isset($_POST["to"]) && $_POST["to"]!="") $to = "&to=".str_replace(" ", "+", $_POST["to"]);
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
            <li><a href="">Задачи</a></li>
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
  <h2 onclick="showNewTaskForm(this);" id="clicable">Добавить задачу</h2>
  <form id="new-task-form" style="display: none;" class="filters" method="post">
    <label for="title">Заголовок:</label>
    <input type="text" id="title" name="title" required>

    <label for="description">Описание:</label>
    <textarea id="description" name="description" required></textarea>

    <label for="deadline">Срок выполнения:</label>
    <input type="date" id="deadline" name="deadline" required>

    <!--<label for="status">Статус:</label>
    <select id="status" name="status">
      <option value="incomplete">Не выполнено</option>
      <option value="complete">Выполнено</option>
    </select>-->

    <label for="priority">Приоритет:</label>
    <select id="priority" name="priority">
      <option value="0">Низкий</option>
      <option value="1">Средний</option>
      <option value="2">Высокий</option>
    </select>

    <!--<label for="list">Прикрепить к списку:</label>
    <select id="list" name="list">
        <option value="">-</option>
    </select>-->

    <button type="submit">Добавить задачу</button>
  </form>

  <hr>

  <div class="fil">
        <form action="index.php" method="post" id="fil">
            <select name="order">
                <option value="" style="display: none;">Сортировка</option>
                <option value="ORDER BY `date` DESC">Сначала новые</option>
                <option value="ORDER BY `date` ASC">Сначала старые</option>
                <option value="ORDER BY `priority` ASC">Сначала с низким приоритетом</option>
                <option value="ORDER BY `priority` DESC">Сначала с высоким приоритетом</option>
            </select>
            <div class="date">
                <label for="from">От:</label>
                <input type="date" name="from">
            </div>
            <div class="date">
                <label for="to">До:</label>
                <input type="date" name="to">
            </div>
            <br>
            <button type="submit">Показать</button>
            <button type="reset">Сбросить</button>
        </form>
    </div>

    <hr>

  <!-- Список задач -->
  <h2>Список задач</h2>
  <table>
    <thead border="0">
      <tr>
        <th>Заголовок</th>
        <th>Описание</th>
        <th>Срок выполнения</th>
        <th>Статус</th>
        <th>Приоритет</th>
        <?if($_SESSION["right"] > 1){?>
        <th>Действия</th><?}?>
      </tr>
    </thead>
    <tbody id="tasks">
      <!-- Здесь будут отображаться задачи -->
    </tbody>
  </table>

  <script>
    function showNewTaskForm(header){
        var form = document.getElementById('new-task-form');
        form.style.display = form.style.display == "none" ? "grid" : "none";
        header.innerText = header.innerText == "Добавить задачу" ? "Отмена" : "Добавить задачу";
    }
    function showTasks() {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
            var tasks = JSON.parse(this.responseText).other;
            var table = document.getElementById("tasks");
            for (var i = 0; i < tasks.length; i++) {
                var task = tasks[i];
                var tr = document.createElement("tr");
                if(i % 2 == 1) tr.setAttribute('style', 'background-color: #dadada;');
                var title = document.createElement("td");
                title.innerText = task.title;
                var desc = document.createElement("td");
                desc.innerText = task.description;
                var deadline = document.createElement("td");
                deadline.innerText = task.deadline;
                var status = document.createElement("td");
                switch(task.completed){
                    case "0": task.completed = "Не выполнена"; break;
                    case "1": task.completed = "Выполнена"; break;
                }
                status.innerText = task.completed;
                var priority = document.createElement("td");

                switch(task.priority){
                    case "0": task.priority = "Низкий"; break;
                    case "1": task.priority = "Средний"; break;
                    case "2": task.priority = "Высокий"; break;
                }
                priority.innerText = task.priority;
                <?if($_SESSION["right"] > 1){?>
                var action = document.createElement("td");
                action.innerHTML = "<a href='remove.php?id="+task.id+"'>Удалить</a><br><a href='edit.php?id="+task.id+"'>Редактировать</a>";<?}?>

                tr.appendChild(title);
                tr.appendChild(desc);
                tr.appendChild(deadline);
                tr.appendChild(status);
                tr.appendChild(priority);
                <?if($_SESSION["right"] > 1){?>tr.appendChild(action);<?}?>
                table.appendChild(tr);
            }
            }
        };
        xhttp.open("GET", "https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=list_tasks<?echo $order.$from.$to;?>", true);
        xhttp.send();
    }
    showTasks();

    function showLists() {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
            var tasks = JSON.parse(this.responseText).other;
            var table = document.getElementById("list");
            for (var i = 0; i < tasks.length; i++) {
                var task = tasks[i];
                var option = document.createElement("option");
                option.value = task.name;
                option.innerText = task.name;
                
                table.appendChild(option);
            }
        }
        };
        xhttp.open("GET", "https://dev.rea.hrel.ru/BVV/?token=SecretToken&type=list_list_tasks", true);
        xhttp.send();
    }
    showLists();
  </script>
</body>
</html>
