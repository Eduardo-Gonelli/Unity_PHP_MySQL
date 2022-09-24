using UnityEngine;
using TMPro;

/// <summary>
/// @Author: https://github.com/Eduardo-Gonelli/
/// The code below uses Unity's TextMeshPro to receive, via inspector, 
/// a result field, email and password. It has three public methods 
/// associated with scene buttons that, after being called, test to 
/// see if the DataManager has finished loading data every 0.2 seconds.
/// When the DataManager finishes loading the data, the result is updated 
/// and the InvokeReapeting is canceled.
/// </summary>

public class UIManager : MonoBehaviour
{
    public TMP_InputField result;
    public TMP_InputField email;
    public TMP_InputField password;
    private bool dataReady = false;
    private string type;

    public void LoadAllData()
    {
        DataManager.instance.LoadAllData();
        type = "LoadAllData";
        InvokeRepeating("UpdateResult", 0f, 0.2f);
    }

    public void LoadPlayerData()
    {
        DataManager.instance.LoadPlayer(email.text);
        type = "LoadPlayerData";
        InvokeRepeating("UpdateResult", 0f, 0.2f);
    }

    public void AuthenticatePlayer()
    {
        DataManager.instance.AuthenticatePlayer(email.text, password.text);
        type = "Authenticate";
        InvokeRepeating("UpdateResult", 0f, 0.2f);
    }

    private void UpdateResult()
    {
        dataReady = DataManager.instance.dataReady;        

        if (dataReady)
        {
            CancelInvoke();
            if(type == "LoadAllData")
            {
                PlayerRootObject playerObj = JsonUtility.FromJson<PlayerRootObject>("{\"players\":" + DataManager.instance.json + "}");
                result.text = "";
                for (int i = 0; i < playerObj.players.Length; i++)
                {
                    result.text +=
                        "ID: " + playerObj.players[i].id
                        + ", Name: " + playerObj.players[i].name
                        + ", Email: " + playerObj.players[i].email
                        + ", Password: " + playerObj.players[i].password
                        + "\n";
                }
            }
            else if (type == "LoadPlayerData")
            {                
                result.text = "";
                if(DataManager.instance.json != "Player not found!")
                {
                    PlayerRootObject playerObj = JsonUtility.FromJson<PlayerRootObject>("{\"players\":" + DataManager.instance.json + "}");
                    for (int i = 0; i < playerObj.players.Length; i++)
                    {
                        result.text +=
                            "ID: " + playerObj.players[i].id
                            + ", Name: " + playerObj.players[i].name
                            + ", Email: " + playerObj.players[i].email
                            + ", Password: " + playerObj.players[i].password + "\n";
                    }
                }
                else
                {
                    result.text = DataManager.instance.json;
                }                
            }
            else if(type == "Authenticate")
            {
                result.text = DataManager.instance.json;
            }
        }
    }
}
