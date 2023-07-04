<?php
if(isset($_POST['login']) && isset($_POST['pass'])){
    include_once("../db_connect.php");

    $query = "CALL `RegisterUser`('".$_POST['login']."', '".$_POST['pass']."');";
    $res_query = mysqli_query($connection,$query);

    if(!$res_query) exit("<p>Непредвиденная ошибка, попробуйте позже</p>");

    setcookie("user", $_POST["login"]);
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
    <link rel="stylesheet" href="styles/signin.css">
    <title>TODO Register</title>
</head>
<body>
    <div class="container">
        <h1>Регистрация</h1>
        <form action="register.php" method="post">
            <input type="text" name="login" placeholder="Логин">
            <br><br>
            <input type="password" name="pass" placeholder="Пароль">
            <br><br>
            <button type="submit">Зарегистрироваться</button>
        </form>
        <br><br>
        <a href="signin.php">Уже есть аккаунт? Войдите!</a>
    </div>
</body>
</html>