using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField result;
    public TMP_InputField email;
    public TMP_InputField password;
    public Button loginButton;
    public Button jogarButon;
    private bool dataReady = false;

    private void Start()
    {
        jogarButon.interactable = false;
    }

    public void AuthenticatePlayer()
    {
        loginButton.interactable = false;
        // verifica se os campos estão preenchidos
        if(email.text == "" || password.text == "")
        {
            result.text = "Preencha o email e senha para continuar!";
            return;
        }
        DataManager.instance.AuthenticatePlayer(email.text, password.text);        
        InvokeRepeating("UpdateResult", 0f, 0.2f);
    }

    // verifica, a cada 0.2 segundos, se os dados estão prontos
    private void UpdateResult()
    {
        dataReady = DataManager.instance.dataReady;

        if (dataReady)
        {
            CancelInvoke();
            // verifica se o json retornado é um erro
            string json = DataManager.instance.json;
            ErrorResponse errorResponses = JsonUtility.FromJson<ErrorResponse>(json);
            if (!string.IsNullOrEmpty(errorResponses.error))
            {
                result.text = errorResponses.error;
                loginButton.interactable = true;
            }
            // se não for, associa os dados do player a um objeto
            else
            {
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                // se não for, mostra os dados do player
                //PlayerRootObject playerObj = JsonUtility.FromJson<PlayerRootObject>("{\"players\":[" + DataManager.instance.json + "]}");

                result.text = "";                
                //for (int i = 0; i < playerObj.players.Count; i++)
                //{
                    result.text +=
                        "ID: " + playerData.id
                        + ", Name: " + playerData.name
                        + ", Email: " + playerData.email                        
                        + "\n";
                //}
                PlayerPrefs.SetString("jogador_id", playerData.id);
                PlayerPrefs.SetString("nome", playerData.name);
                PlayerPrefs.SetString("email", playerData.email);
                jogarButon.interactable = true;
            }   
        }
    }

    public void Jogar()
    {
        SceneManager.LoadScene("Main");
    }
}
