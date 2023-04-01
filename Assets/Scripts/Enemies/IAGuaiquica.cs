using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAGuaiquica : MonoBehaviour
{
    private const float raiz2 = 1.414f;

    [Header("Ataque")]
    [SerializeField] private float attackRange;
    [SerializeField] private int attackStartupLengthFrames;
    [SerializeField] private int attackLengthFrames;
    [SerializeField] private int attackEndLengthFrames;
    private bool isAttacking;

    [Header("Patrulla")]
    public float patrolSpeed;
    [SerializeField] private float patrolRange;
    public Transform platformCheck;
    public GameObject wallCheck;
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(!isAttacking) {

            Vector2 distance = new Vector2(target.position.x-transform.position.x, target.position.y-transform.position.y);
            if(distance.sqrMagnitude < patrolRange*patrolRange && Mathf.Abs(distance.y) < 2){//si esta en rango de buscar al jugador
                if(Mathf.Abs(distance.x) < attackRange && Mathf.Abs(distance.y) < attackRange){//si esta en rango de atacarlo
                    StartCoroutine(Attack(distance.x));
                } else if(Mathf.Sign(distance.x) != Mathf.Sign(transform.localScale.x)){//si esta andando en direccion al jugador
                    Flip();
                }
                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);//continua andando en direccion al jugador

            } else if (mustPatrol){
                mustFlip = !Physics2D.OverlapCircle(platformCheck.position, 0.1f, groundLayer);
                Patrol();
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
        mustPatrol = true;
        rb.velocity = new Vector2(patrolSpeed, rb.velocity.y); 
    }

    private IEnumerator Attack(float distance){

        if(Mathf.Sign(distance) != Mathf.Sign(transform.localScale.x)){//si esta andando en direccion al jugador
            Flip();
        }

        isAttacking = true;
        //esperar antes de iniciar el salto
        for (int i = 0; i < attackStartupLengthFrames; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        //iniciar el salto
        float vx = distance/(raiz2*(float)attackLengthFrames/50f);
        float vy = 5f*((float)attackLengthFrames/50f);
        rb.velocity = new Vector2(vx, vy);

        //esperar a que se complete el salto
        for (int i = 0; i < attackLengthFrames; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        rb.velocity = Vector2.zero;

        //esperar despues de terminar el salto
        for (int i = 0; i < attackLengthFrames; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        isAttacking = false;
    }
}
