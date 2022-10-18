using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour , IDataPersistence
{

    [Header("Valores de vida y representacion grafica")]
    [SerializeField, Range(0f, 100f)] private float maxHealth;
    [SerializeField, Range(0f, 100f)] private float currentMaxHealth;
    private float currentHealth;
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image maxHealthBar;

    [Header("Frames de invulneravilidad")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int flashNumber;
    [SerializeField] private int enemyLayer;
    [SerializeField] private int playerLayer;
    private SpriteRenderer sprite;

    [Header("Knockback")]
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private int knockbackDuration;
    private Move move;
    private Jump jump;
    private Rigidbody2D rb;

    [HideInInspector] public Vector2 respawnPoint;

    public void LoadData(GameData data)
    {
        this.maxHealth = data.maxHealth;
        this.currentMaxHealth = data.currentMaxHealth;
        this.currentHealth = data.currentHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.maxHealth = this.maxHealth;
        data.currentMaxHealth = this.currentMaxHealth;
        data.currentHealth = this.currentHealth;
    }

    void Awake()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        currentHealth = currentMaxHealth;
        maxHealthBar.fillAmount = currentMaxHealth/maxHealth;
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
        sprite = GetComponent<SpriteRenderer>();
        respawnPoint = transform.position;
        move = GetComponent<Move>();
        jump = GetComponent<Jump>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
    }


    //Current health updated
    public void RestoreHealth (float healthRestored)
    {
        currentHealth = Mathf.Clamp(currentHealth + healthRestored, 0, currentMaxHealth);
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
        if (currentHealth == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Current max health increased
    public void HealthUp(float healthIncreased)
    {
        currentMaxHealth += healthIncreased;
        maxHealthBar.fillAmount = currentMaxHealth/maxHealth;
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;

    }

    //Current max health decreased
    public void HealthDown(float healthIncreased)
    {
        currentMaxHealth += healthIncreased;
        maxHealthBar.fillAmount = currentMaxHealth/maxHealth;
        currentHealthBar.fillAmount = currentHealth/currentMaxHealth * maxHealthBar.fillAmount;
        
    }

    //Current health updated + Iframes or death
    public void TakeDamage(float damage, Vector2 enemyPos)
    {
        RestoreHealth(-damage);
        if (currentHealth > 0)
        {
            //Knockback
            StartCoroutine(Knockback(enemyPos));
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
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        for (int i = 0; i < flashNumber; i++)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            yield return new WaitForSeconds(iFrameDuration/(2*flashNumber));
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
            yield return new WaitForSeconds(iFrameDuration/(2*flashNumber));
        }
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }

    private IEnumerator Knockback(Vector2 enemyPos)
    {     
        move.isKnockedBack = true;
        jump.isKnockedBack = true;

        float direction = Mathf.Sign(transform.position.x - enemyPos.x);
        rb.velocity = new Vector2(knockbackSpeed*direction, 10f);

        for (int i = 0; i < knockbackDuration; i++)
        {
            yield return new WaitForFixedUpdate();
        }


        move.isKnockedBack = false;
        jump.isKnockedBack = false;

    }

}
