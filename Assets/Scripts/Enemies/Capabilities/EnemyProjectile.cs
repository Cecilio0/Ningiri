using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    private float currentLifeTime;
    private float direction;

    private Collider2D coll;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        currentLifeTime = 0;
        gameObject.SetActive(false);
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
        {
            gameObject.SetActive(false);
            coll.enabled = false;
        }
            

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.gameObject.GetComponent<Health>().TakeDamage(damage, transform.position);
        
    }

    public void Cast(float direction, Vector2 velocity)
    {
        gameObject.SetActive(true);
        currentLifeTime = 0;
        this.direction = direction;
        coll.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector2(localScaleX, transform.localScale.y);
        rb.velocity = velocity;
    }


}
