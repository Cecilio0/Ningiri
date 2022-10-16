using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public float maxHealth;
    public float currentMaxHealth ;
    public float currentHealth;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> healthCoins;

    public GameData()
    {
        this.maxHealth = 100f;
        this.currentMaxHealth = 100f;
        this.currentHealth = 100f;
        this.playerPosition = Vector3.zero;
        this.healthCoins = new SerializableDictionary<string, bool>();
    }

    public int GetPercentageComplete()
    {
        //Ver cuantas monedas de vida se han recolectado
        int totalCollected = 0;
        foreach(bool collected in this.healthCoins.Values)
        {
            if(collected)
            {
                totalCollected++;
            }
        }

        //Asegurarse de que no se divida entre 0 al calcular el porcentaje
        int percentageCompleted = -1;
        if(healthCoins.Count != 0)
        {
            percentageCompleted = (totalCollected * 100 / healthCoins.Count);
        }
        return percentageCompleted;
    }
}
