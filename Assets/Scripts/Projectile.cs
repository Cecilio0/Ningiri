using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    private float currentLifeTime;
    private float direction;

    private BoxCollider2D coll;

    private Rigidbody2D body;
    // Start is called before the first frame update
    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        currentLifeTime = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        //para obviar el resto del update en caso que no se este usando el componente
        if(!gameObject.activeInHierarchy) return;

        //para calcular el tiempo que lleva vivo el proyectil
        currentLifeTime += Time.deltaTime;
        
        //si el proyectil lleva mas tiempo vivo que el tiempo que puede pasar vivo
        if (currentLifeTime > lifeTime)
            gameObject.SetActive(false);

        //calcula el movimiento en x que debe hacer el proyectil
        body.velocity = new Vector2(speed*direction, 0);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Enemy")
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        else if (collision.tag == "Boss")
            collision.gameObject.GetComponent<BossHealth>().TakeDamage(damage);
        coll.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Untagged" || collision.collider.tag == "Boss")
        {
            coll.enabled = false;
            gameObject.SetActive(false);
        }

    }

    public void Cast(float direction)
    {
        gameObject.SetActive(true);
        currentLifeTime = 0;
        this.direction = direction;
        coll.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector2(localScaleX, transform.localScale.y);
    }


}
