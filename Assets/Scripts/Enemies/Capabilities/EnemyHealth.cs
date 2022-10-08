using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [Header("Generic health elements")]
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    [SerializeField] private float flashTime;
    private SpriteRenderer sprite;

    [Header("On death")]

    [SerializeField] private GameObject[] toDrop;
    [SerializeField] private float dropChance;
    

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
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
            
            //matar al enemigo
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Flash()
    {
        Color originalColor = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        sprite.color = originalColor;

    }
}
