using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Text;
using System;

/// <summary>
/// @Author: https://github.com/Eduardo-Gonelli/
/// The examples used below are for educational purposes.
/// Unless you're sure what you're doing, do not use it in production.
/// The codes below make a call to a PHP services server. 
/// PHP services access a MySQL database and return data in JSON format.
/// A player's id, name and email are used as an example.
/// </summary>

public class DataManager : MonoBehaviour
{    
    public static DataManager instance { get; private set; }
    public string json;
    public bool dataReady;
    private string token;
    // use your php service here
    string urlService = "http://localhost/senac/a8_unity_php/";
    
    void Awake()
    {
        //Singleton to have only one instance of the DataManager and be able to access it from any code.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // verifica se está logado com um token no playerprefs
        // se estiver, carrega a cena main
        
        if (!PlayerPrefs.HasKey("token"))
        {
            SceneManager.LoadScene("Login");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }                
    }



    // autentica o player e carrega os dados dele
    public void AuthenticatePlayer(string email, string password)
    {
        dataReady = false;
        WWWForm form = new WWWForm();        
        token = MD5Hash(email + Guid.NewGuid().ToString());
        PlayerPrefs.SetString("token", token);
        string url = urlService + "?action=api_login";        
        form.AddField("token", token);
        form.AddField("email", email);
        form.AddField("password", password);

        StartCoroutine(LoadData(form, url));
    }

    // verifica se o token está válido
    public void CheckToken()
    {
        dataReady = false;
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("jogador_id", PlayerPrefs.GetString("jogador_id"));
        string url = urlService + "?action=api_check_token";
        StartCoroutine(LoadData(form, url));
    }

    // carrega todas as pontuacoes
    public void LoadScores()
    {
        dataReady = false;
        WWWForm form = new WWWForm();
        form.AddField("jogador_id", PlayerPrefs.GetString("jogador_id"));
        form.AddField("token", PlayerPrefs.GetString("token"));
        string url = urlService + "?action=api_get_score";
        StartCoroutine(LoadData(form, url));
    }

    public void SaveScore(string score)
    {
        dataReady = false;
        WWWForm form = new WWWForm();
        form.AddField("jogador_id", PlayerPrefs.GetString("jogador_id"));
        form.AddField("score", score);
        form.AddField("token", PlayerPrefs.GetString("token"));
        string url = urlService + "?action=api_save_score";
        StartCoroutine(LoadData(form, url));
    }

    // gera um hash no padrão md5 e retorna a string
    private string MD5Hash(string v)
    {        
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(v);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }

    // função genérica para carregar dados
    IEnumerator LoadData(WWWForm form, string url)
    {           
        using(UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.certificateHandler = new ByPassHTTPSCertificate();
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                json = Encoding.UTF8.GetString(www.downloadHandler.data);                
                dataReady = true;                
            }
        }
    }
}
