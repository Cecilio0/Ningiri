using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [Header("Generic health elements")]
    [SerializeField] public float maxHealth;
    [HideInInspector] public float currentHealth;

    [SerializeField] private float flashTime;
    [SerializeField] protected SpriteRenderer[] sprites;

    [Header("On death")]

    [SerializeField] protected GameObject[] toDrop;//Elementos que puede dropear 
    [SerializeField, Range(0f, 100f)] protected float dropChance;//Probabilidad de dropear un elemento en porcentaje
    

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        dropChance = dropChance/100f;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (currentHealth > 0)
        {
            //knockback

            //parte grafica
            StartCoroutine(Flash());
        }
        else if(gameObject.tag != "Boss")
        {
            //dropear cosas
            Drop();
            //matar al enemigo
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Flash()
    {
        Color[] originalColors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            originalColors[i] = sprites[i].color;
            sprites[i].color = Color.red;
        }
        
        yield return new WaitForSeconds(flashTime);
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = originalColors[i];
        }
    }

    protected void Drop()
    {
        if (toDrop != null && Random.value < dropChance)
        {
            Instantiate(toDrop[Random.Range(0, toDrop.Length)], transform.position, Quaternion.identity);
        }
    }
}
