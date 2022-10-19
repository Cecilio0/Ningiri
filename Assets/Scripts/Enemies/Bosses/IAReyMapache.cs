using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAReyMapache : MonoBehaviour
{
    
    [Header("Elementos generales del ray mapache")]
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private float routineTimer;
    private Rigidbody2D rb;
    private Transform target;
    private float timer;//delimita cada cuanto ataca el jefe  
    private bool isOnSecondPhase; 
    private BossHealth health;
    
    [Header("Elementos de spawnear ayudante")]
    [SerializeField] private GameObject rata;
    [SerializeField] private GameObject mapache;
    [SerializeField] private Transform spawnTierra;
    [SerializeField] private GameObject paloma;
    [SerializeField] private Transform spawnAire;

    [Header("Elementos de lanzar proyectil")]
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float tiempoEntreDisparos;
    [SerializeField] private float airTime;
    private Transform projectileSpawn;

    [Header("Elementos de dash")]
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallLayer;
    bool isDashing;

    //elementos de pruebas
    //int attack = 0;
    
    


    // Start is called before the first frame update
    void Awake()
    {
        //elementos generales
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        timer = 3;
        isOnSecondPhase = false;
        health = GetComponent<BossHealth>();

        //elementos de lanzar proyectil
        projectileSpawn = transform;

        //elementos de dash
        isDashing = false;

        //elementos de pruebas
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnSecondPhase && health.currentHealth/health.maxHealth < 0.5f)
        {
            isOnSecondPhase = true;
        }
            

        if (isDashing && enemyCollider.IsTouchingLayers(wallLayer))
        {
            rb.velocity = Vector2.zero;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            transform.position = new Vector2(transform.position.x -Mathf.Sign(transform.localScale.x), transform.position.y);
            isDashing = false;
        }

        timer += Time.deltaTime;
        if (!isDashing && timer > routineTimer)
        {
            timer = 0;
            int attack;
            if(isOnSecondPhase)
                attack = Random.Range(0,2);
            else attack = Random.Range(0,3);

            //int attack = 1;//para hacer pruebas
            
            /*
            attack++;
            if (attack > 2)
                attack = 0;
            */
            
            Attack(attack);
            //en la segunda fase hara un comportamiento mas spawnear enemigo
            if (isOnSecondPhase)
            {
                Attack(2); 
            }
        }
    }
    
    private void Attack(int attack)
    {
        switch (attack)
        {
            case 0:
                StartCoroutine(Dash());
                break;
            case 1:
                int cantDisparos = (int)(routineTimer/tiempoEntreDisparos);
                StartCoroutine(Shoot(cantDisparos - (int)(1/tiempoEntreDisparos), tiempoEntreDisparos));
                break;
            
            case 2:
                int enemy = Random.Range(0, 3);
                SpawnLackey(enemy);
                break;

            case 3://pensar en comportamiento en el que spawnee pinchos en el suelo
                break;
        }
    }

    //hacer corrutina en la que salte dos veces para indicar que va a hacer el dash
    private IEnumerator Dash()
    {
        isDashing = true;

        rb.velocity = new Vector2(0, 2f);
        for (int i = 0; i < 25; i++)
            yield return new WaitForFixedUpdate();
        rb.velocity = new Vector2(0, 2f);
        for (int i = 0; i < 25; i++)
            yield return new WaitForFixedUpdate();

        rb.velocity = new Vector2(-speed*Mathf.Sign(transform.localScale.x), 0);
    }

    
    //spawnea un enemigo nuevo, el parametro indica cual se va a spawnear
    private void SpawnLackey(int enemy)
    {
        if (enemy == 0)//spawnea rata
        {
            Instantiate(rata, new Vector2(spawnTierra.position.x, spawnTierra.position.y), Quaternion.identity);
        }
        else if (enemy == 1)//spawnea mapache
        {
            Instantiate(mapache, spawnTierra.position, Quaternion.identity);
        }
        else //spawnea paloma
        {
            Instantiate(paloma, spawnAire.position, Quaternion.identity);
        }
    }

    //pensar en una corrutina para hacer que indique este ataque
    //El Jefe lanza una serie de proyectiles con trayectoria parabolica
    private IEnumerator Shoot(int cicles, float waitTime)
    {   
        Rigidbody2D targetRb = target.gameObject.GetComponent<Rigidbody2D>();
        float dx;
        float dy;
        for (int i = 0; i < cicles-1; i++)
        {
            //distancia hasta el jugador
            Vector2 velocity = targetRb.velocity;
            dy = target.position.y - transform.position.y;//se calcula al contrario por la formula
            dx = target.position.x - transform.position.x;

            if (velocity.x*dx > 0)
                ThrowProjectile(dx*1.3f, dy*1.3f);
            else if (velocity.x*dx < 0)
                ThrowProjectile(dx*0.7f, dy*0.7f);
            else ThrowProjectile(dx, dy);
            yield return new WaitForSeconds(tiempoEntreDisparos);
        }
    }

    //El Jefe lanza un proyectil con trayectoria parabolica
    private void ThrowProjectile(float dx, float dy)
    {
        
        int projectile = FindProjectile();
        projectiles[projectile].transform.position = transform.position;
        projectiles[projectile].GetComponent<EnemyProjectile>().Cast(-Mathf.Sign(transform.localScale.x), new Vector2(dx/airTime, 2f*dy/airTime + rb.gravityScale*airTime*10f));

    }

    private int FindProjectile()
    {
        int i = 0;
        while (i < projectiles.Length)
        {
            if (!projectiles[i].activeInHierarchy)
                return i;
            i++;
        }
        return 0;
    }
}
