using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : EnemyHealth
{

    [SerializeField] private GameObject[] toDisable;
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image maxHealthBar;
    [SerializeField] private SceneChange sceneChange;
    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        currentHealthBar.fillAmount = 1;
    }

    public new void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        currentHealthBar.fillAmount = currentHealth/maxHealth;
        if (currentHealth == 0)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (sceneChange != null && playerHealth.currentHealth == playerHealth.currentMaxHealth)
            {
                sceneChange.escena = 1;
            }
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
