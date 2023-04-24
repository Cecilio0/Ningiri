using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLingeringProjectile : MonoBehaviour
{
    [SerializeField] private float damage;
    private float direction;

    private Collider2D coll;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D){
            collision.gameObject.GetComponent<Health>().TakeDamage(damage, transform.position);
            this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3){
            rb.velocity = Vector2.zero;
        } else if(collision.gameObject.layer == 6){
            collision.gameObject.GetComponent<Health>().TakeDamage(damage, transform.position);
            this.gameObject.SetActive(false);
        }
    }

    public void Cast(float direction, Vector2 velocity)
    {
        gameObject.SetActive(true);
        this.direction = direction;
        coll.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector2(localScaleX, transform.localScale.y);
        rb.velocity = velocity;
    }


}
