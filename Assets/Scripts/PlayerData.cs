// to work with JSON, classes and variables need to be public or serializable.

// this is the player data represented as a common class.
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string id;
    public string name;
    public string email;    
}

// as the json format used in this example has several players,
// a class is created to store all the players. See example from
// Bunny83: https://answers.unity.com/questions/1503047/json-must-represent-an-object-type.html
[System.Serializable]
public class PlayerRootObject
{
    public List<PlayerData> players;
}

// json format used as example
// [{"id":"1","name":"Ed","email":"edu@gmail.com","password":"hashSha256EncryptedString"},{"id":"1","name":"Ed","email":"edu@gmail.com","password":"hashSha256EncryptedString"}]
