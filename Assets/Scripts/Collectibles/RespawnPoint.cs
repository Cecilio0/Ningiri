using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private Sprite onPickup;
    private SpriteRenderer spriteR;
    private Color[] ogColors;
    void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
        ogColors = new Color[transform.childCount];

        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            collision.GetComponent<Health>().respawnPoint = transform.position;
            spriteR.sprite = onPickup;
        }
    }
}
