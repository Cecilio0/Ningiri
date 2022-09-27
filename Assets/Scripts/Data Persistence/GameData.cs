using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float maxHealth;
    public float currentMaxHealth ;
    public float currentHealth;
    public Vector2 playerPosition;
    public Dictionary<string,bool> healthCoins;

    public GameData(){
        maxHealth = 100f;
        currentMaxHealth = 100f;
        currentHealth = 100f;
        playerPosition = Vector2.zero;
        healthCoins = new Dictionary<string, bool>();
    }
}
