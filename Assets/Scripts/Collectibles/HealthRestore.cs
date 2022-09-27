using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRestore : MonoBehaviour , IDataPersistence
{

    [SerializeField] private float healthRestored;
    [SerializeField] private string healthCoinID;
    private bool isCollected = false;
    [ContextMenu("Generate guid for id")]

    private void GenerateGuid(){
        healthCoinID = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data){
        data.healthCoins.TryGetValue(healthCoinID, out isCollected);
        if(isCollected){
            gameObject.SetActive(false);
        }
    }
    public void SaveData(ref GameData data){
        if(data.healthCoins.ContainsKey(healthCoinID)){
            data.healthCoins.Remove(healthCoinID);
        }
        data.healthCoins.Add(healthCoinID, isCollected);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().RestoreHealth(healthRestored);
            gameObject.SetActive(false);
            isCollected = true;
        }
    }
    
}
