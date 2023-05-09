using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{

    public long lastUpdated;
    public int currentLevel;
    public float maxHealth;
    public float currentMaxHealth;
    public Vector2 respawn;
    public SerializableDictionary<string, bool> healthCoins;
    public SerializableDictionary<string, bool> collectibles;

    public GameData()
    {
        this.currentLevel = 3;
        this.maxHealth = 100f;
        this.currentMaxHealth = 30f;
        respawn = new Vector2(-95, -20);
        this.healthCoins = new SerializableDictionary<string, bool>();
        this.collectibles = new SerializableDictionary<string, bool>();
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

    public string GetLevel()
    {
        //Ver cuantas monedas de vida se han recolectado
        switch (currentLevel){
            case 2:
                return "BASURERO"; 
            case 5: 
                return "BOSQUE BAMBU";
        }
        return "DIALOGOS";
    }
}
