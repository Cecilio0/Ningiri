using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour{

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake() {
        if(instance != null){
        Debug.LogError("More than one DataPersistenceManager in scene!");
        } 
        instance = this;
    }
    /////////////////////////////////////////////////////////////////////
    public void Begin(){
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame(){
        this.gameData = new GameData();
    }
    public void LoadGame(){
        //TODO: Load any save data from a file using the data handler

        // if no data can be loadad, initialize a new game
        if (this.gameData == null){
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        //TODO: push the loaded data to all the scripts that need it
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects){
            dataPersistenceObject.LoadData(gameData);
        }
        Debug.Log("Loaded health: " + gameData.maxHealth);
        Debug.Log("Loaded current max health" + gameData.currentMaxHealth);
        Debug.Log("Loaded current health: " + gameData.currentHealth);
    }
    public void SaveGame(){
        //TODO: pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects){
            dataPersistenceObject.SaveData(ref gameData);
        }
        //TODO: save the data to a file using the data handler

        Debug.Log("Saved health: " + gameData.maxHealth);
        Debug.Log("Saved current max health" + gameData.currentMaxHealth);
        Debug.Log("Saved current health: " + gameData.currentHealth);
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
