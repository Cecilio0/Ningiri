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

    public void NewGame(){

    }
    public void LoadGame(){

    }
    public void SaveGame(){

    }
}
