using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPaloma : MonoBehaviour
{

    [Header ("Attack")]
    [SerializeField] private float enemyPursuitSpeed;
    [SerializeField] private float enemyReturnSpeed;
    [SerializeField] private float attackLength;//duracion de todo el ataque
    [SerializeField] private float attackRange;
    [SerializeField] private float attackStartup;//tiempo que se demora en inciar el ataque

    private bool attacking;

    [Header ("Patrol")]
    [SerializeField] private float patrolRange;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Rigidbody2D rb;
    private Transform target;


    private Vector2 posInicial;

    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attacking = false;
        posInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!attacking)
        {
            Vector2 distance = new Vector2(transform.position.x - target.position.x, transform.position.y - target.position.y);
            //si se esta dentro de cierto rango el enemigo debera buscar a el target.
            if (Physics2D.Raycast(transform.position, target.position) && distance.sqrMagnitude < patrolRange*patrolRange)
            {
                if (distance.sqrMagnitude < attackRange*attackRange)
                {
                    StartCoroutine(Attack(distance));
                } 
                else
                {
                    float vX = -distance.normalized.x*enemyPursuitSpeed;
                    float vY = -distance.normalized.y*enemyPursuitSpeed;
                    Flip(distance);
                    rb.velocity = new Vector2(vX, vY);
                }
                
            } 
            else //si no vuelve al spawn
            {
                Vector2 origen = new Vector2(transform.position.x - posInicial.x, transform.position.y - posInicial.y);
                if (origen.sqrMagnitude < attackRange*attackRange)
                {
                    rb.velocity = Vector2.zero;
                } 
                else
                {  
                    origen = origen.normalized;
                    float vX = -origen.x*enemyReturnSpeed;
                    float vY = -origen.y*enemyReturnSpeed;
                    Flip(origen);
                    rb.velocity = new Vector2(vX, vY);
                }
            }
        }
    }

    private void Flip(Vector2 direccion)
    {
        if (direccion.x < 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        } 
        else if (direccion.x > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        } 
    }

    private IEnumerator Attack(Vector2 distance)
    {
        attacking = true;
        rb.velocity = Vector2.zero;
        float velocity = attackRange*attackRange/(2f*(attackLength));
        distance = distance.normalized*velocity;
        yield return new WaitForSeconds(attackStartup);
        rb.velocity = new Vector2(-distance.x, -distance.y);
        yield return new WaitForSeconds((attackLength)/2);
        rb.velocity = new Vector2(distance.x, distance.y);
        yield return new WaitForSeconds((attackLength)/2);
        rb.velocity = Vector2.zero;
        attacking = false;
    }
    
}
