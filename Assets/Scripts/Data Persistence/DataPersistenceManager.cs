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

    private void Awake() {
        if(instance != null){
        Debug.LogError("More than one DataPersistenceManager in scene!");
        } 
        instance = this;
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
        Debug.Log("OnSceneLoaded called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();

    }
    public void OnSceneUnloaded(Scene scene){
        Debug.Log("OnSceneUnloaded called");
        SaveGame();
    }
    
    public void NewGame(){
        this.gameData = new GameData();
    }
    public void LoadGame(){
        //Load any save data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loadad, initialize a new game
        if (this.gameData == null){
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        //TODO: push the loaded data to all the scripts that need it
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects){
            dataPersistenceObject.LoadData(gameData);
        }
        //Debug.Log("Loaded health: " + gameData.maxHealth);
        //Debug.Log("Loaded current max health" + gameData.currentMaxHealth);
        // Debug.Log("Loaded current health: " + gameData.currentHealth);
    }
    public void SaveGame(){
        //TODO: pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects){
            dataPersistenceObject.SaveData(ref gameData);
        }
        //Debug.Log("Saved health: " + gameData.maxHealth);
        //Debug.Log("Saved current max health" + gameData.currentMaxHealth);
        //Debug.Log("Saved current health: " + gameData.currentHealth);

        //TODO: save the data to a file using the data handler
        dataHandler.Save(gameData);
    }
    /////////////////////////////////////////////////////////////////////
    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
