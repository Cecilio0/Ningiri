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

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Se encontro mas de una instancia de DataPersistenceManager en la escena"); 
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Load any save data from a file using the data handler
        this.gameData = dataHandler.Load();
        //if no data can be loaded, initialize to a new game
        if(this.gameData == null)
        {
            Debug.Log("No se encontraron datos, inicializando un nuevo juego");
            this.NewGame();
        }

        //Meter los datos cargados a todos los otros scripts que lo necesiten
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(this.gameData);
        }
    }

    public void SaveGame()
    {
        //Pasar los datos a otros scripts para que los puedan actualizar
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref this.gameData);
        }

        //Guardar los datos en un archivo usando el data handler
        dataHandler.Save(this.gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersitenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersitenceObjects);
    }
}
