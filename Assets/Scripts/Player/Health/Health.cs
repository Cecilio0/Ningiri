using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    [Header("Valores de vida y representacion grafica")]
    [SerializeField, Range(0f, 100f)] private float maxHealth;
    [SerializeField, Range(0f, 100f)] private float currentMaxHealth;
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image maxHealthBar;

    [Header("Frames de invulneravilidad")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int flashNumber;
    private SpriteRenderer sprite;


    private float currentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = currentMaxHealth;
        maxHealthBar.fillAmount = currentMaxHealth/maxHealth;
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestoreHealth (float healthRestored)
    {
        currentHealth = Mathf.Clamp(currentHealth + healthRestored, 0, currentMaxHealth);
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
        if (currentHealth == 0)
        {
            //Hurt
            //iframes
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, currentMaxHealth);
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
        if (currentHealth > 0)
        {
            //Hurt
            //iframes
            StartCoroutine(Invulneravility());
        }
        else 
        {
            //Dead
            //respawn
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator Invulneravility()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < flashNumber; i++)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            yield return new WaitForSeconds(iFrameDuration/(2*flashNumber));
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
            yield return new WaitForSeconds(iFrameDuration/(2*flashNumber));
        }
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

}