using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Text;


/// <summary>
/// @Author: https://github.com/Eduardo-Gonelli/
/// The examples used below are for educational purposes. Do not use in production.
/// The codes below make a call to a PHP services server. 
/// PHP services access a MySQL database and return data in JSON format.
/// A player's id, name and email are used as an example.
/// </summary>

public class DataManager : MonoBehaviour
{    
    public static DataManager instance { get; private set; }
    public string json;
    public bool dataReady;
    // use your php service here
    string urlService = "http://localhost/senac_aulas/senac_aulas_exemplos/grad_gsd/ex_mysql_php/model/unity_service.php";
    
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
        //Tests
        //LoadAllData();
        //LoadPlayer("edd@gmail.com");
        //AuthenticatePlayer("edu@gmail.com", "123456");
        SceneManager.LoadScene("Main");
    }

    public void LoadAllData()
    {
        dataReady = false;
        WWWForm form = new WWWForm();
        form.AddField("type", "LoadAllData");
        StartCoroutine(LoadData(form));
    }

    public void LoadPlayer(string email)
    {
        dataReady = false;
        WWWForm form = new WWWForm();
        form.AddField("type", "LoadPlayer");
        form.AddField("email", email);
        StartCoroutine(LoadData(form));
    }

    public void AuthenticatePlayer(string email, string password)
    {
        dataReady = false;
        WWWForm form = new WWWForm();
        form.AddField("type", "Authenticate");
        form.AddField("email", email);

        // transform the password string in hash sha256
        // see GetHash function on line 107
        using (SHA256 sha256Hash = SHA256.Create())
        {
            string hash = GetHash(sha256Hash, password);
            password = hash;
        }

        form.AddField("password", password);        
        StartCoroutine(LoadData(form));
    }

    IEnumerator LoadData(WWWForm form)
    {        
        
        using(UnityWebRequest www = UnityWebRequest.Post(urlService, form))
        {
            www.certificateHandler = new ByPassHTTPSCertificate();
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                json = www.downloadHandler.text;
                dataReady = true;
                //print(json);                
            }
        }
        
    }

    // create a string hash from a string
    // from https://learn.microsoft.com/pt-br/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=net-6.0
    private static string GetHash(HashAlgorithm hashAlgorithm, string input)
    {

        // Convert the input string to a byte array and compute the hash.
        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}
