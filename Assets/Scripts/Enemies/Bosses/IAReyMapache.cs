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
    
    [Header("Elementos de spawnear ayudante")]
    [SerializeField] private GameObject rata;
    [SerializeField] private GameObject mapache;
    [SerializeField] private Transform spawnTierra;
    [SerializeField] private GameObject paloma;
    [SerializeField] private Transform spawnAire;

    [Header("Elementos de lanzar proyectil")]
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float tiempoEntreDisparos;
    private Transform projectileSpawn;

    [Header("Elementos de dash")]
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallLayer;
    bool isDashing;


    
    


    // Start is called before the first frame update
    void Awake()
    {
        //elementos generales
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        timer = 0;

        //elementos de lanzar proyectil
        projectileSpawn = transform;

        //elementos de dash
        isDashing = false;

        //apenas inicia el nivel el rey mapache debe estar desactivado para que no use sus patrones de ataque
        //gameObject.SetActive(false);
        //se activara de nuevo cuando el juegador entre a la pelea de jefe
        
    }

    // Update is called once per frame
    void Update()
    {
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
            int attack = Random.Range(0,3);
            //int attack = 0;//para hacer pruebas
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
            }
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
            Instantiate(rata, new Vector2(spawnTierra.position.x, spawnTierra.position.y-0.7f), Quaternion.identity);
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
        float distance;
        for (int i = 0; i < cicles-1; i++)
        {
            //distancia hasta el jugador
            distance = target.position.x - transform.position.x;
            ThrowProjectile(distance);
            yield return new WaitForSecondsRealtime(tiempoEntreDisparos);
        }
    }

    //El Jefe lanza un proyectil con trayectoria parabolica
    private void ThrowProjectile(float distance)
    {
        
        int projectile = FindProjectile();
        projectiles[projectile].transform.position = transform.position;
        projectiles[projectile].GetComponent<EnemyProjectile>().Cast(-Mathf.Sign(transform.localScale.x), new Vector2(distance/2, 18f));

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
