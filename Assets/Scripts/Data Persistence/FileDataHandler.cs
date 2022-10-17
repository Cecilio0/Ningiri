using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    //Ruta del directorio donde se guardan los datos
    private string dataDirPath = "";
    //Nombre del archivo donde se guardan los datos
    private string dataFileName = "";
    //Manejo de encriptado
    private bool useEncryption = false;
    private readonly string encryptionGuide = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileId)
    {
        //Caso base: El profileId es nulo, regresar null
        if(profileId == null)
        {
            return null;
        }
        //Ruta completa del archivo que incluye su nombre y la ruta del directorio
        string fullPath = Path.Combine(this.dataDirPath, profileId, this.dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                //Cargar los datos serializados del archivo
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //Opcion de encriptado
                if(this.useEncryption)
                {
                    dataToLoad = Encriptacion(dataToLoad);
                }
                //Deserializar los datos del JSON a un objeto 
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error al tratar de cargar los datos del archivo: " + fullPath + "\n" + e.Message);
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        //Caso base: El profileId es nulo, regresar null
        if(profileId == null)
        {
            return;
        }
        //Ruta completa del archivo que incluye su nombre y la ruta del directorio
        string fullPath = Path.Combine(this.dataDirPath, profileId, this.dataFileName);

        try
        {
            //Creacion de directorio en caso de que no exista
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serializacion de los game data object a un JSON
            string dataToStore = JsonUtility.ToJson(data);

            //Opcion de encriptado
            if(this.useEncryption)
            {
                dataToStore = Encriptacion(dataToStore);
            }

            //Escritura de los datos serializados en el archivo
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Ocurrio un error mientras se trataba de guardar los datos en el archivo:" + fullPath + "\n" + e.Message);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        //Loop sobre todos los nombres de directorios en el directorio de datos
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(this.dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            //Programacion preventiva: chequear si el data file existe
            //si no existe, entonces el folder no es un profile y debe ser saltado
            string fullPath = Path.Combine(this.dataDirPath, profileId, this.dataFileName);
            if(!File.Exists(fullPath))
            {
                Debug.LogWarning("Saltando directorio cuando se estan cargando los profiles porque no contiene data: " + profileId);
                continue;
            }

            //Cargar los datos del juego para este profile y agregarlos al diccionario
            GameData profileData = Load(profileId);

            //Programacion preventiva: asegurarse que los datos del profile no son nulos.
            //Porque si lo son entonces algo va a fallar y deberiamos saberlo
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Se trato de guardar el profile pero algo salio mal. Profile ID: " + profileId);
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = null;
        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach(KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            //Saltar esta entrada si el game data es nulo
            if(gameData == null)
            {
                continue;
            }

            //Si esta es la primer data con la que nos encontramos que existe, entonces es la mas reciente por el momento
            if(mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }
            //Si no, comparar para ver cual fecha es la mas reciente
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                //Si la nueva fecha es mas reciente que la mas reciente hasta ahora, entonces esta es la mas reciente hasta ahora
                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }
    private string Encriptacion (string data)
    {
        string encriptedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            encriptedData += (char)(data[i] ^ encryptionGuide[i % encryptionGuide.Length]);
        }
        return encriptedData;
    }
}
