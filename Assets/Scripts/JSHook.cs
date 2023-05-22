using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class JSHook : MonoBehaviour
{
    public void test()
    {
        StartCoroutine(UpdateSavesPost(this._id, guardados));
    }

    [SerializeField]private string _id = "646a9965e9b9f14e81f7dfaa";
    public string[] guardados;
    [HideInInspector]public string redirectUrl;
    public static JSHook instance;

    //se inicializa el objeto
    void Start()
    {
        
        if (instance != null)//revisar si ya existe una instancia y si ya existe eliminar la que se acaba de crear
        {
            Debug.Log("Se encontro mas de una instancia de JsHook en la escena. Destruyendo el nuevo"); 
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);//para que el gameObject persista entre escenas

        
        test();
        
    }

    //set el url al cual dirigira el boton de salida
    public void SetUrl(string _id)
    {
        this._id = _id;
    }

    //accion al undir el boton de salida
    public void QuitButton()
    {
        UpdateSaves();
        /*
        OpenLinks.OpenLinkSelf(redirectUrl);
        OpenLinks.LogOut();
        */
    }

    void OnApplicationQuit()
    {
        UpdateSaves();
        //OpenLinks.LogOut();
    }


    //////////////////////////////////////////////////////////////////
    //Informacion de usuario y conversion de JSON a objeto
    [System.Serializable]
    public class ReactUnity//formato del objeto a recibir
    {   
        /*
        {
            "user":"juan.lopez@dnau.io",
            "token":"api token"
        }
        */
        public string _id;//recordar que esto es un correo electronico

    }

    //formato del objeto user dentro de InfoUsuario recibir
    [System.Serializable]
    public class User
    {  
        public string _id;
        public string guardado1;
        public string guardado2;
        public string guardado3;
    } 


    //metodo llamado por react para enviar el user y el api token
    /*
    {
        "user":"name"
        "token":"api token"
    }
    */
    public void UserCredentialsString(string jsonReact)
    {
        
        //hacer que se retorne el json del metodo de post
        ReactUnity jsonInicial = JsonUtility.FromJson<ReactUnity>(jsonReact);
        this._id = jsonInicial._id;
        GetSaves();

    }

    public void GetSaves()
    {
        StartCoroutine(GetSaves(this._id));//metodo post
    }

    public void UpdateSaves()
    {
        StartCoroutine(UpdateSavesPost(this._id, this.guardados));//metodo post
    }

    

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //de aqui en adelante es el consumo de la api

    /*
    Url: https://dnau-api.herokuapp.com/refresh-token/
    body: {
        "user":"tests",
        "password":"PRUEBA"
    } recuerda que tiene headers access-token
    */
    //se hace una unityWebRequest y se asignan los valores de la variable userInfo y apiToken
    private IEnumerator GetSaves(string _id)
    {
        string url = $"http://localhost:3002/user/saves/{_id}";//url donde se consume la api

        //se crea un string para guardar la respuesta
        string responseBody = "";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            //request.SetRequestHeader("access-token", token);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                //se guarda la respuesta en el responseBody
                byte[] results = request.downloadHandler.data;
                foreach(byte car in results)
                {
                    responseBody += (char)car;
                }
            }
        }
        if(responseBody != "")//se revisa si el responseBody es nulo 
        {
            User info = JsonUtility.FromJson<User>(responseBody);//se convierte el responseBody a un objeto
            guardados[0] = info.guardado1;//se guarda la informacion del usuario en un objeto de la clase
            guardados[1] = info.guardado2;
            guardados[2] = info.guardado3;
        }
        else
        {
            Debug.Log("error de verificacion del servidor");
            #if !UNITY_EDITOR && UNITY_WEBGL
                OpenLinks.OpenLinkSelf(redirectUrl);
            #endif
            
        }
    }

    private IEnumerator UpdateSavesPost(string _id, string[] guardados)
    {
        string url = $"http://localhost:3002/user/saves/{_id}";//url donde se consume la api
        WWWForm form = new WWWForm();
        //se añaden los elementos del body
        
        form.AddField("guardado1", guardados[0]);
        form.AddField("guardado2", guardados[1]);
        form.AddField("guardado3", guardados[2]);
        
        

        string responseBody = "";
        
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("error de verificacion del servidor\nvolver a intentar");
            }
            else
            {
                Debug.Log("Succex");
                byte[] results = request.downloadHandler.data;
                foreach(byte car in results)
                {
                    responseBody += (char)car;
                }
                Debug.Log(responseBody);
            }   
        }
    }
}