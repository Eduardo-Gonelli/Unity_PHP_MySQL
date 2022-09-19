<?php
include 'db.php';

// o tipo de requisição que está sendo feita.
$type = "";
if(isset($_POST['type']))
{
    $type = $_POST['type'];
    if($type == "LoadAllData")
    {
        while($row = $consulta_players->fetch_assoc())
        {
            $myArray[] = $row;
        }
        echo json_encode($myArray);    
    }
    elseif($type == "LoadPlayer")
    {
        $email = $_POST['email'];
        $sql = "SELECT id, name, email, password FROM PLAYERS WHERE email = '$email'";        
        $result = mysqli_query($conexao, $sql);
        if(mysqli_num_rows($result) > 0){
            while($row = mysqli_fetch_assoc($result))
            {
                $myArray[] = $row;
            }
            echo json_encode($myArray); 
        }
        else{
            echo "Player not found!";
        }
        

    }
    elseif($type == "Authenticate")
    {
        $email = $_POST['email'];
        $password = $_POST['password'];
        $sql = "SELECT id, name FROM PLAYERS WHERE email = '$email' and password = '$password'";
        $result = mysqli_query($conexao, $sql);
        if(mysqli_num_rows($result) > 0){
            while($row = mysqli_fetch_assoc($result))
            {
                $myArray[] = $row;
            }
            echo json_encode($myArray);
            echo "User authentication successful!";
        }
        else{
            echo "Username or password is invalid!";
        }
        
    }
    else
    {
        echo "Invalid request!";
    }
}



?>