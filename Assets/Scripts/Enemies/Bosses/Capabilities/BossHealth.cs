using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : EnemyHealth
{

    [SerializeField] private GameObject[] toDisable;
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image maxHealthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        currentHealthBar.fillAmount = 1;
    }

    public new void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        currentHealthBar.fillAmount = currentHealth/maxHealth;
        if (currentHealth == 0)
        {
            foreach (GameObject thing in toDisable)
            {
                thing.SetActive(false);
            }
            //muerte del jefe
            base.Drop();
            Destroy(this.gameObject);
        }
    }


}
