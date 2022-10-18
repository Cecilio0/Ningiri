using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour{
    [Header("Debbuging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "";
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Se encontro mas de una instancia de DataPersistenceManager en la escena. Destruyendo el nuevo"); 
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if(disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence esta desactivado actualmente");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if(overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Se sobreescribio el profile id seleccionado con el valor de testSelectedProfileId" + testSelectedProfileId);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded Called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded Called");
        SaveGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        //Actualizar el profile usado para guardar y cargar
        this.selectedProfileId = newProfileId;
        //Cargar el juego, que usara ese profile, actualizando la game Data adecuadamente
        LoadGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Retornar si el data persistence esta desactivado
        if(disableDataPersistence)
        {
            return;
        }
        //Load any save data from a file using the data handler
        this.gameData = dataHandler.Load(selectedProfileId);

        //Empezar un nuevo juego si los datos son nulos y estamos configurados para inicializar los datos para propositos de debbugeo
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        //if no data can be loaded, dont continue
        if(this.gameData == null)
        {
            Debug.Log("No se encontraron datos. Un Nuevo Juego debe ser creado antes que los datos puedan ser cargados");
            return;
        }

        //Meter los datos cargados a todos los otros scripts que lo necesiten
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(this.gameData);
        }
    }

    public void SaveGame()
    {
        //Retornar si el data persistence esta desactivado
        if(disableDataPersistence)
        {
            return;
        }
        //Si no se tiene ningun dato para guardar, lanzar una advertencia aqui
        if(this.gameData == null)
        {
            Debug.LogWarning("No se encontraron datos. Un Nuevo Juego debe ser creado antes que los datos puedan ser guardados");
            return;
        }
        //Pasar los datos a otros scripts para que los puedan actualizar
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref this.gameData);
        }

        //Tomar el tiempo para saber cuando fue la ultima vez que se guardo
        this.gameData.lastUpdated = System.DateTime.Now.ToBinary();
        //Guardar los datos en un archivo usando el data handler
        dataHandler.Save(this.gameData, selectedProfileId);
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

    public bool HasGameData()
    {
        return this.gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
