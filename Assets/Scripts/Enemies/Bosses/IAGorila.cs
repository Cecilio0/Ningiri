using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAGorila : MonoBehaviour
{
    private const float G2 = 4.9f;//mitad de la gravedad
    private Rigidbody2D rb;
    private Transform target;
    private bool isAttacking;

    [Header("Rutinas")]
    [SerializeField] private float timeBetweenAttacksSeconds;
    private float timeSinceLastAttackSeconds;

    [Header("Salto al jugador")]
    [SerializeField] private float jumpAttackTimeSeconds;
    
    [Header("Ataque en las lianas")]
    [SerializeField] private Transform[] lianas;
    [SerializeField] private float jumpLianaTimeSeconds;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private GameObject banana;
    [SerializeField] private float deltaBananas;
    [SerializeField] private int shotsPerRoutine;
    [SerializeField] private int bananaShotsPerRoutine;
    [SerializeField] private int timeBetweenShotsFrames;
    [SerializeField] private float projectileAirTimeSeconds;



    void Awake()
    {   
        rb = GetComponent<Rigidbody2D>();
        isAttacking = false;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        timeSinceLastAttackSeconds = 0;
    }

    void Update(){
        if(!isAttacking){
            timeSinceLastAttackSeconds += Time.deltaTime;
            if(timeSinceLastAttackSeconds > timeBetweenAttacksSeconds){
                isAttacking = true;
                int ataque = Random.Range(0,2);
                switch(ataque){
                    case 0://saltar al jugador
                        StartCoroutine(AtaqueSalto());
                        break;
                    case 1://lianas
                        StartCoroutine(AtaqueLianas(Random.Range(0,2), Random.Range(1,3)));
                        break;
                }
                timeSinceLastAttackSeconds = 0;
            } else {
                //cositas que quiera hacer entre rutinas mitad


            }
        }   
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
    }

    private IEnumerator AtaqueSalto(){
        
        //subirse a la liana
        Vector2 delta = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);//vector desde el gorila hasta el jugador
        rb.velocity = new Vector2(delta.x/jumpAttackTimeSeconds, (delta.y + 1.5f)/jumpAttackTimeSeconds + G2*jumpAttackTimeSeconds);//vector de velocidad necesaria para llegar al jugador
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x)*Mathf.Sign(rb.velocity.x), transform.localScale.y);//mira al jugador
        yield return new WaitForSeconds(jumpAttackTimeSeconds);
        rb.velocity = Vector2.zero;
        isAttacking = false;
    }

    private IEnumerator AtaqueLianas(int liana, int tipoAtaque){
        isAttacking = true;
        //subirse a la liana
        Vector2 delta = new Vector2(lianas[liana].position.x - transform.position.x, lianas[liana].position.y - transform.position.y);//vector desde el gorila hasta la liana
        rb.velocity = new Vector2(delta.x/jumpLianaTimeSeconds, delta.y/jumpLianaTimeSeconds + G2*jumpLianaTimeSeconds);//vector de velocidad necesaria para llegar a la liana
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x)*Mathf.Sign(rb.velocity.x), transform.localScale.y);//mira la liana

        yield return new WaitForSeconds(jumpLianaTimeSeconds);
        float ogGrav = rb.gravityScale;
        rb.gravityScale = 0;//se le quita la gravedad para que se quede quieto en la liana
        rb.velocity = Vector2.zero;//se le quita el momentum 

        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x)*Mathf.Sign(target.position.x - transform.position.x), transform.localScale.y);//mira al jugador

        switch(tipoAtaque){
            case 1://disparar al jugador
                for (int i = 0; i < shotsPerRoutine; i++){
                    for (int j = 0; j < timeBetweenShotsFrames; j++)
                        yield return new WaitForFixedUpdate();

                    delta = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);//vector desde el gorila hasta el jugador
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x)*Mathf.Sign(delta.x), transform.localScale.y);//mira al jugador
                    Shoot(delta);
                }
                break;

            case 2://lanzar bananas
                int direction;
                if(liana == 0)
                    direction = 1;
                else direction = -1;
                for (int i = 1; i <= bananaShotsPerRoutine; i++){
                    for (int j = 0; j < timeBetweenShotsFrames; j++)
                        yield return new WaitForFixedUpdate();

                    delta = new Vector2(i*direction*6 + Random.value*deltaBananas, target.position.y - transform.position.y);//vector desde el gorila hasta el jugador
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x)*Mathf.Sign(delta.x), transform.localScale.y);//mira al jugador
                    ShootB(delta);
                }
                break;
        }

        //se baja
        rb.velocity = new Vector2(2*Mathf.Sign(target.position.x - transform.position.x), 2);//se baja con un salto chiquito
        rb.gravityScale = ogGrav;//se reinicia la gravedad del enemigo
        yield return new WaitForSeconds(1.5f);
        rb.velocity = Vector2.zero;
        isAttacking = false;
    }

    private void Shoot(Vector2 delta){
        Vector2 speed = new Vector2(delta.x/projectileAirTimeSeconds, delta.y/projectileAirTimeSeconds + 2*G2*projectileAirTimeSeconds);//vector de velocidad necesaria para llegar al jugador
        
        int projectile = FindProjectile();
        projectiles[projectile].transform.position = transform.position;
        projectiles[projectile].GetComponent<EnemyProjectile>().Cast(Mathf.Sign(delta.x), speed);
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

    private void ShootB(Vector2 delta){
        Vector2 speed = new Vector2(delta.x/projectileAirTimeSeconds, delta.y/projectileAirTimeSeconds + 2*G2*projectileAirTimeSeconds);//vector de velocidad necesaria para llegar al jugador
        
        GameObject projectile = GameObject.Instantiate(banana, transform.position, Quaternion.identity);
        projectile.GetComponent<EnemyLingeringProjectile>().Cast(Mathf.Sign(delta.x), speed);
    }
}