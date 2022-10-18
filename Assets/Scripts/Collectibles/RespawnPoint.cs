using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    private SpriteRenderer[] sprites;
    private Color[] ogColors;
    void Awake()
    {
        sprites = new SpriteRenderer[transform.childCount];
        ogColors = new Color[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            sprites[i] = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            ogColors[i] = sprites[i].color;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            collision.GetComponent<Health>().respawnPoint = transform.position;
            foreach (SpriteRenderer item in sprites)
            {
                item.color = selectedColor;
            }
        }
    }
}
