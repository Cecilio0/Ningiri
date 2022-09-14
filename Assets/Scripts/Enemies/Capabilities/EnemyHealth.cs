using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    private float currentHealth;

    [SerializeField] private float flashTime;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        else
        {
            //dropear cosas

            //matar al enemigo
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        sprite.color = Color.white;

    }
}
