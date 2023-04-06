using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMapRojo : MonoBehaviour
{
    private const float raiz2 = 1.414f;

    [Header("Ataque")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private int attackCooldownFrames; //para no pegar al jugador a una pared
    private int framesSinceLastAttack;
    private bool isAttacking;

    [Header("Patrulla")]
    public float patrolSpeed;
    [SerializeField] private float patrolRange;
    public Transform platformCheck;
    public Collider2D EnemyCollider;
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    private Transform target;
    private bool mustPatrol;
    private bool mustFlip;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        isAttacking = false;
        mustPatrol = true;
        mustFlip = false;
        framesSinceLastAttack = attackCooldownFrames;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        framesSinceLastAttack++;
        if(!isAttacking ) {

            Vector2 distance = new Vector2(target.position.x-transform.position.x, target.position.y-transform.position.y);
            if(framesSinceLastAttack > attackCooldownFrames //si se puede hacer el ataque de nuevo
            && distance.sqrMagnitude < patrolRange*patrolRange //si esta en rango de buscar al jugador,
            && Mathf.Abs(distance.y) < 2 //si esta a una altura adecuada para buscar al jugador
            && Mathf.Abs(distance.x) > 1.6f){ //verificacion para no tener problemas con colliders dentro de paredes
                StartCoroutine(Attack(distance.x));

            } else if (mustPatrol){
                mustFlip = !Physics2D.OverlapCircle(platformCheck.position, 0.1f, groundLayer);
                Patrol();
            }
        } else{//si el mapache esta atacando
            mustFlip = !Physics2D.OverlapCircle(platformCheck.position, 0.1f, groundLayer);
            if (mustFlip || EnemyCollider.IsTouchingLayers(groundLayer)){
                isAttacking = false;
                Flip();
            }
            
        } 
    }

    void Patrol()
    {
        if (mustFlip || EnemyCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }
        rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        patrolSpeed *= -1;
        attackSpeed *= -1;
        mustPatrol = true;
        
    }


    //Corrutina de Sprint
    private IEnumerator Attack(float distance)
    {

        if(Mathf.Sign(distance) != Mathf.Sign(transform.localScale.x)){//si esta andando en direccion al jugador
            Flip();
        }

        mustPatrol = false;
        isAttacking = true;
        rb.velocity = new Vector2(attackSpeed, rb.velocity.y);

        while (isAttacking){
            yield return new WaitForEndOfFrame();
        }
        framesSinceLastAttack = 0;
        mustPatrol = true;
    }
}
