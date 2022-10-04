using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;


    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    public bool isNewGame;//variable para saber si la partida es un juego nuevo o no

    private void Awake() {
        if(instance != null)
        {
            Debug.Log("More than one DataPersistenceManager in scene!. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        } 
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        
    }
    
    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        //Debug.Log("OnSceneLoaded called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
        
    }
    public void OnSceneUnloaded(Scene scene){
        //Debug.Log("OnSceneUnloaded called");
        SaveGame();
    }
    
    public void NewGame(){
        this.gameData = new GameData();
    }


    public void LoadGame(){
        //Load any save data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loadad, initialize a new game
        if (this.gameData == null || isNewGame){
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
            return;
        }

        //push the loaded data to all the scripts that need it
        foreach(IDataPersistence dataPersistenceObj in this.dataPersistenceObjects){
            dataPersistenceObj.LoadData(gameData);
        }
    }
    public void SaveGame(){
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved");
            return;
        }
        //pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObj in this.dataPersistenceObjects){
            dataPersistenceObj.SaveData(ref gameData);
        }
        //save the data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
