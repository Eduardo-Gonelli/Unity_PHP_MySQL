using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine.UI;

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
    public TMP_InputField score;
    public Button loadScoresButton;
    public Button saveScoreButton;
    private bool dataReady = false;
    private string type;

    public void LoadScores()
    {
        DisableButtons();
        type = "LoadScores1";
        DataManager.instance.CheckToken();        
        InvokeRepeating("UpdateResult", 0f, 0.2f);
        Debug.Log("LoadScores");        
    }

    public void LoadScores2()
    {
        dataReady = false;
        type = "LoadScores2";
        DataManager.instance.LoadScores();
        InvokeRepeating("UpdateResult", 0f, 0.2f);
    }

    public void SaveScore()
    {
        DisableButtons();
        dataReady = false;
        type = "SaveScore";
        DataManager.instance.SaveScore(score.text);
        InvokeRepeating("UpdateResult", 0f, 0.2f);
    }

    private void UpdateResult()
    {
        dataReady = DataManager.instance.dataReady;        

        if (dataReady)
        {
            CancelInvoke();
            if(type == "LoadScores1")
            {
                string json = DataManager.instance.json;
                ErrorResponse errorResponses = JsonUtility.FromJson<ErrorResponse>(json);
                if (!string.IsNullOrEmpty(errorResponses.error))
                {
                    result.text = errorResponses.error + "\nRetornando para a tela de login!";   
                    Invoke(nameof(LoadLoginScene), 2f);
                    EnableButtons();
                }
                else
                {
                    LoadScores2();
                }
            }
            else if(type == "LoadScores2")
            {
                ScoreRoot scores = JsonUtility.FromJson<ScoreRoot>("{\"players\":" + DataManager.instance.json + "}");
                result.text = "";
                foreach (ScoreData score in scores.players)
                {
                    result.text += score.name + ": " + score.score + "\n";
                }
                EnableButtons();
            }
            else if (type == "SaveScore")
            {
                string json = DataManager.instance.json;
                ErrorResponse errorResponses = JsonUtility.FromJson<ErrorResponse>(json);
                if (!string.IsNullOrEmpty(errorResponses.error))
                {
                    result.text = errorResponses.error + "\nRetornando para a tela de login!";
                    Invoke(nameof(LoadLoginScene), 2f);
                    EnableButtons();
                }
                else
                {
                    LoadScores2();
                }                               
            }
        }
    }

    private void LoadLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

    public void DisableButtons()
    {
        saveScoreButton.interactable = false;
        loadScoresButton.interactable = false;
    }

    public void EnableButtons()
    {
        saveScoreButton.interactable = true;
        loadScoresButton.interactable = true;
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.DeleteKey("jogador_id");
        SceneManager.LoadScene("Login");
    }
}
