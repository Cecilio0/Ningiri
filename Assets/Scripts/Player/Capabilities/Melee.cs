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
    // Start is called before the first frame update
    void Awake()
    {
        inputs = GetComponent<PlayerInput>();
        cooldownTimer = attackCooldown;
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
            enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
        Debug.Log("Ataca");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
