using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : EnemyHealth
{

    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image maxHealthBar;

    public new void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        currentHealthBar.fillAmount = currentHealth/maxHealth;
        if (currentHealth == 0)
        {
            //muerte del jefe
            Destroy(this.gameObject);
        }
    }
}
