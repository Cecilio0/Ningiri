using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMapache : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private float pursuitSpeed;
    [SerializeField] private float attackRange;
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
        if (enemyCollider.IsTouchingLayers(wallLayer))
            rb.velocity = -1*rb.velocity;
        //si el cronometro supera al timer se hace una rutina nueva
        Vector2 distance = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        if (distance.sqrMagnitude < patrolRange*patrolRange && Mathf.Abs(distance.y) < 2)
            AttackBehaviour(distance);
        else if (cronometro > timerRutinas)
            Behaviour();
        
    }

    private void AttackBehaviour(Vector2 distance)
    {
        if (distance.sqrMagnitude > attackRange*attackRange)
        {
            Move(pursuitSpeed, Mathf.Sign(distance.x));
        } 
        else if (!attacking)
        {
            StartCoroutine(Attack());
        }
        
    }

    private IEnumerator Attack()
    {
        //tiempo que se demora en aparecer el ataque
        attacking = true;
        rb.velocity = Vector2.zero;
        for (int i = 0; i < hitBoxSpeed; i++)
        {
            yield return new WaitForFixedUpdate();    
        }

        //animacion provisional de ataque
        float direccion = transform.localScale.x;
        Quaternion origen = pivoteBrazo.rotation;
        attackHitBox.enabled = true;
        float fraccion = 120/(float)attackLength;
        for (int i = 0; i < attackLength; i++)
        {
            pivoteBrazo.eulerAngles = new Vector3(0, 0, pivoteBrazo.eulerAngles.z + fraccion);
            yield return new WaitForFixedUpdate();
        }

        //final del ataque
        pivoteBrazo.rotation = origen;
        attackHitBox.enabled = false;
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
        rb.velocity = new Vector2(speed*direction, rb.velocity.y);
        transform.localScale = new Vector2(-transform.localScale.x*direction, transform.localScale.y);
    }
    
}
