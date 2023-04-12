using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IALemur : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField] private int timeBetweenShotsFrames;
    [SerializeField] private int attackCooldownFrames;
    [SerializeField] private float XRange;
    [SerializeField] private int shotsPerBurst;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float shotSpeed;
    private bool isAttacking;

    [Header("Patrol")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private int framesBetweenFlips;
    private int framesSinceLastFlip;
    private const float XDelta = 1.36f;//constante con la cual se va a rotar al lemur
    private const float YDelta = 3.73f;//tamaño del lemur
    [SerializeField] private float stickLength;//longitud del palo sobre el cual esta apoyado
    private Transform target;
    private Rigidbody2D rb;
    private Vector2 YRange;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        isAttacking = false;
        YRange = new Vector2(transform.position.y, transform.position.y + stickLength - YDelta);
        rb.velocity = new Vector2(0, patrolSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(!isAttacking){
            framesSinceLastFlip++;
            Vector2 distance = new Vector2(target.position.x-transform.position.x, target.position.y-transform.position.y);
            if(Mathf.Abs(distance.x) < XRange && YRange.x < target.position.y && YRange.y > target.position.y){
                if(Mathf.Abs(distance.y) < 1f){
                    StartCoroutine(Shoot(Mathf.Sign(distance.x)));
                } else{
                    rb.velocity = new Vector2(0, Mathf.Sign(distance.y)*patrolSpeed);
                }
            } else{
                if(transform.position.y > YRange.y){
                    rb.velocity = new Vector2(0, -patrolSpeed);
                }else if(transform.position.y < YRange.x){
                    rb.velocity = new Vector2(0, patrolSpeed);
                } else if(rb.velocity.y == 0){
                    rb.velocity = new Vector2(0, patrolSpeed);
                }
                
            }
            if(framesSinceLastFlip > framesBetweenFlips){
                XFlip();
                framesSinceLastFlip = 0;
            }
            
        }
    }

    private void XFlip()
    {
        transform.position = new Vector2(transform.position.x + Mathf.Sign(transform.localScale.x)*XDelta, transform.position.y);
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private IEnumerator Shoot(float distance)
    {
        rb.velocity = Vector2.zero;
        isAttacking = true;

        for (int i = 0; i < shotsPerBurst; i++)
        {
            for (int j = 0; j < timeBetweenShotsFrames; j++)
            {
                yield return new WaitForFixedUpdate();
            }
            int projectile = FindProjectile();
            projectiles[projectile].transform.position = transform.position;
            projectiles[projectile].GetComponent<EnemyProjectile>().Cast(1, new Vector2(Mathf.Sign(distance)*shotSpeed, 0));
            
        }

        //esperar despues de terminar el ataque
        for (int i = 0; i < attackCooldownFrames; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        isAttacking = false;
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
