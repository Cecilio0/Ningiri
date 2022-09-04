using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IARata : MonoBehaviour
{

    public float patrolSpeed;
    public Transform platformCheck;
    public GameObject wallCheck;
    public Collider2D EnemyCollider;
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public LayerMask obstaculo;

    private bool mustPatrol;
    private bool mustFlip;
    // Start is called before the first frame update
    void Start()
    {
        mustPatrol = true;
        mustFlip = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustFlip = !Physics2D.OverlapCircle(platformCheck.position, 0.1f, groundLayer);
            Patrol();
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
    }
}
