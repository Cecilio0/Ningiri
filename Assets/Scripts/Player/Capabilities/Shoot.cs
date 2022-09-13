using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    private PlayerInput inputs;
    private Rigidbody2D body;

    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] projectiles;
    private float cooldownTimer;
    // Start is called before the first frame update
    void Awake()
    {
        inputs = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody2D>();
        cooldownTimer = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (inputs.actions["Shoot"].WasPressedThisFrame() && cooldownTimer > attackCooldown)
            Attack();
        
    }

    private void fixedUpdate()
    {

    }

    private void Attack()
    {
        int projectile = FindProjectile();
        cooldownTimer = 0;
        projectiles[projectile].transform.position = firePoint.position;
        Vector2 direction = inputs.actions["Aim"].ReadValue<Vector2>();
        if (direction != null && direction.sqrMagnitude != 0)
        {
            Debug.Log("curioso");
            projectiles[projectile].GetComponent<Projectile>().Cast(Mathf.Sign(direction.x));
        }
        else 
        {
            projectiles[projectile].GetComponent<Projectile>().Cast(-Mathf.Sign(transform.localScale.x));
        }
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
