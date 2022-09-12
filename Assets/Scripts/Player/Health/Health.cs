using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    [SerializeField, Range(0f, 100f)] private float maxHealth;
    [SerializeField, Range(0f, 100f)] private float currentMaxHealth;
    public Image currentHealthBar;
    public Image maxHealthBar;

    public GameObject respawn;
    private float currentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = currentMaxHealth;
        maxHealthBar.fillAmount = currentMaxHealth/maxHealth;
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, currentMaxHealth);
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
        if (currentHealth > 0)
        {
            //Hurt
            //iframes
        }
        else 
        {
            //Dead
            //respawn
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
