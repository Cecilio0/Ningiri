using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour{
    private GameData gameData;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake() {
        if(instance != null){
        Debug.LogError("More than one DataPersistenceManager in scene!");
        }
        instance = this;
    }

    public void Begin(){
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

    }
    public void SaveGame(){
        //TODO: pass the data to other scripts so they can update it

        //TODO: save the data to a file using the data handler
    }

    private void OnApplicationQuit() {
        SaveGame();
    }
}
