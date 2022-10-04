using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    //Path del directorio donde queremos guardar los datos en el computador
    private string dataDirPath = "";
    //Nombre del archivo donde se guardaran los datos
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    //Constructor
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption){
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    //Metodo de carga
    public GameData Load(){
         //Creacion del Path combinado
        string fullPath  = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath)){
            try{
                //Cargar los datos serializados del archivo
                string dataToLoad = "";
                using(FileStream fileStream = new FileStream(fullPath, FileMode.Open)){
                    using(StreamReader reader = new StreamReader(fileStream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //Desencriptar los datos (opcional)
                if(useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                //Deserializar los datos del JSON a un objeto de tipo GameData
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                
            }
            catch(Exception e){
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    //Metodo de guardado
    public void Save(GameData data){
        //Creacion del Path combinado
        string fullPath  = Path.Combine(dataDirPath, dataFileName);
        //Try-Catch para evitar errores
        try{
            //Se crea el directorio donde se va a guardar el archivo si no existe
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serializacion del gameData Object a un JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            //Encriptacion de los datos (opcional)
            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //Escribir la serializacion de datos en el archivo
            using(FileStream stream = new FileStream(fullPath, FileMode.Create)){
                using(StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e){
            Debug.LogError("Error ocurred when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data){
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++){
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}