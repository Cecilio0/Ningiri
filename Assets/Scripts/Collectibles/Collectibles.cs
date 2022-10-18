using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string collectibleID;
    private bool isCollected = false;
    [ContextMenu("Generate guid for id")]

    private void GenerateGuid(){
        collectibleID = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data){
        data.collectibles.TryGetValue(collectibleID, out isCollected);
        if(isCollected){
            gameObject.SetActive(false);
        }
    }
    public void SaveData(ref GameData data){
        if(data.collectibles.ContainsKey(collectibleID)){
            data.collectibles.Remove(collectibleID);
        }
        data.collectibles.Add(collectibleID, isCollected);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            isCollected = true;
        }
    }
    
}
