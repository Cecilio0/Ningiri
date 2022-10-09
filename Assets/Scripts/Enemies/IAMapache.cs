using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMapache : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private float pursuitSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private int attackEndLag;
    [SerializeField] private int attackLength;//duracion completa del ataque
    [SerializeField] private int hitBoxSpeed;//frames que se demora en salir la hitbox
    [SerializeField] private Collider2D attackHitBox;
    [SerializeField] Transform pivoteBrazo;
    private Transform target;
    private bool attacking;

    [Header("Patrulla")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float patrolRange;
    [SerializeField] private float timerRutinas;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Transform platformCheck;
    [SerializeField] private LayerMask wallLayer;
    
    private float cronometro;
    private Rigidbody2D rb;



    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attackHitBox.enabled = false;
        attacking = false;
        cronometro = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        cronometro += Time.deltaTime;
        if (enemyCollider.IsTouchingLayers(wallLayer) || !Physics2D.OverlapCircle(platformCheck.position, 0.1f, wallLayer))
        {
            Move(rb.velocity.x, -1);
        }
            
        //si el cronometro supera al timer se hace una rutina nueva
        Vector2 distance = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        RaycastHit2D ray = Physics2D.Linecast(transform.position, target.position, wallLayer);
        if (ray.collider == null && distance.sqrMagnitude < patrolRange*patrolRange && Mathf.Abs(distance.y) < 1.5f)
            AttackBehaviour(distance);
        else if (cronometro > timerRutinas)
            Behaviour();
        
    }

    private void AttackBehaviour(Vector2 distance)
    {
        if (distance.sqrMagnitude > attackRange*attackRange)
        {
            if (distance.x < 0)
            {
                transform.localScale = new Vector2( Mathf.Abs(transform.localScale.x), transform.localScale.y);
                Move(pursuitSpeed, 1);
            }
            else if (distance.x > 0)
            {
                transform.localScale = new Vector2( -Mathf.Abs(transform.localScale.x), transform.localScale.y);
                Move(pursuitSpeed, 1);
            }

        } 
        else 
        {
            rb.velocity = Vector2.zero;
            if (!attacking)
                StartCoroutine(Attack(distance));
        }
        
    }

    private IEnumerator Attack(Vector2 distance)
    {
        //tiempo que se demora en aparecer el ataque
        attacking = true;
        
        for (int i = 0; i < hitBoxSpeed; i++)
        {
            yield return new WaitForFixedUpdate();    
        }

        //animacion provisional de ataque
        Vector3 inicio = pivoteBrazo.localEulerAngles;
        attackHitBox.enabled = true;
        int fraccion = 120/attackLength;
        Vector3 rotacion = new Vector3(0f, 0f, fraccion);
        for (int i = 0; i < attackLength; i++)
        {
            pivoteBrazo.Rotate(rotacion);
            yield return new WaitForFixedUpdate();
        }

        //final del ataque
        pivoteBrazo.localEulerAngles = inicio;
        attackHitBox.enabled = false;
        for (int i = 0; i < attackEndLag; i++)
            yield return new WaitForFixedUpdate();
        attacking = false;
    }

    //esto unicamente se hace si el personaje no esta dentro del rango de vision
    private void Behaviour()
    {
        
        cronometro = 0;
        int action = Random.Range(0, 2);
        switch(action)
        {
            case 0://se queda quieto   
                break;
            case 1://se mueve
                int direction = Random.Range(0, 2);
                switch (direction)
                {
                    case 0:
                        Move(patrolSpeed, -1);
                        break;
                    case 1:
                        Move(patrolSpeed, 1);
                        break;
                }
                break;
        }
    }

    private void Move(float speed, float direction)
    {
        transform.localScale = new Vector2(transform.localScale.x*direction, transform.localScale.y);
        rb.velocity = new Vector2(-speed*Mathf.Sign(transform.localScale.x), rb.velocity.y);
    }
    
}
