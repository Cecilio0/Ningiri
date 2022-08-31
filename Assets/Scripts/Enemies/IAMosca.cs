using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMosca : MonoBehaviour
{

    public float enemyAttackSpeed;
    public float enemyReturnSpeed;
    public float patrolRange;
    public Collider2D enemyCollider;
    public Rigidbody2D rb;
    public GameObject target;


    private Vector2 posInicial;

    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 distance = new Vector2(transform.position.x - target.transform.position.x, transform.position.y - target.transform.position.y);
        //si se esta dentro de cierto rango el enemigo debera buscar a el target.
        if (distance.magnitude < patrolRange)
        {
            GoTo(target.transform.position);
        } 
        else //si no vuelve al spawn
        {
            GoTo(posInicial);
        }
    }

    private void GoTo(Vector2 destination)
    {
        Vector2 origen = new Vector2(transform.position.x - destination.x, transform.position.y - destination.y);
        float vX = -origen.normalized.x*enemyReturnSpeed;
        float vY = -origen.normalized.y*enemyReturnSpeed;
        rb.velocity = new Vector2(vX, vY);
    }    
    
}
