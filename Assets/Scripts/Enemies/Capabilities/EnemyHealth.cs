using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    private float currentHealth;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
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
        }
        else
        {
            //dropear cosas

            //matar al enemigo
            gameObject.SetActive(false);
        }
    }
}
