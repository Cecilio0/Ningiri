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
}
