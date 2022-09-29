using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMapache : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private Transform target;
    [SerializeField] private float pursuitSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackLength;
    [SerializeField] private float hitBoxSpeed;
    [SerializeField] private Collider2D attackHitBox;
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
        attacking = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(hitBoxSpeed);
        attackHitBox.enabled = true;
        yield return new WaitForSeconds(attackLength-hitBoxSpeed);
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
