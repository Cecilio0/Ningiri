using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Melee : MonoBehaviour
{
    private PlayerInput inputs;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private LayerMask enemyLayer;
    private float cooldownTimer;
    private Shoot shoot;

    [SerializeField] private Transform provisional;//elemento provisional

    // Start is called before the first frame update
    void Awake()
    {
        inputs = GetComponent<PlayerInput>();
        cooldownTimer = attackCooldown;
        shoot = GetComponent<Shoot>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (attackCooldown < cooldownTimer)
        {
            if (inputs.actions["Attack"].WasPressedThisFrame())
            {
                Attack();
                cooldownTimer = 0;
            }
        }
        
    }

    private void Attack()
    {
        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        
        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy is BoxCollider2D)
            {
                if (enemy.tag == "Enemy")
                    enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                else enemy.gameObject.GetComponent<BossHealth>().TakeDamage(attackDamage);
            }
        }
        StartCoroutine(graficoAtaque());
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //completamente provisional
    private IEnumerator graficoAtaque()
    {
        float direccion = transform.localScale.x;
        Quaternion origen = provisional.rotation;
        float iteraciones = 10f;
        float fraccion = 0.05f/iteraciones;
        for (int i = 0; i < iteraciones; i++)
        {
            provisional.eulerAngles = new Vector3(0, 0, 90/iteraciones*i*direccion);
            yield return new WaitForSeconds(fraccion);
        }
        provisional.rotation = origen;
    }

    public void DamageUp(float damageUp)
    {
        attackDamage += damageUp;
    }
}
