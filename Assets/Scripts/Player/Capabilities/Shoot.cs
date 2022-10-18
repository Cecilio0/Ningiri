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
    [SerializeField] private Transform oppositeFirePoint;
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

        //Ataca
        int projectile = FindProjectile();
        cooldownTimer = 0;
        
        Vector2 direction = inputs.actions["Aim"].ReadValue<Vector2>();
        if (direction != null && direction.sqrMagnitude != 0)
        {
            if (Mathf.Sign(direction.x) == Mathf.Sign(transform.localScale.x))
            {
                projectiles[projectile].transform.position = oppositeFirePoint.position;
                projectiles[projectile].GetComponent<Projectile>().Cast(Mathf.Sign(direction.x));
            } 
            else 
            {
                projectiles[projectile].transform.position = firePoint.position;
                projectiles[projectile].GetComponent<Projectile>().Cast(Mathf.Sign(direction.x));
            }
            
        }
        else 
        {
            projectiles[projectile].transform.position = firePoint.position;
            projectiles[projectile].GetComponent<Projectile>().Cast(-Mathf.Sign(transform.localScale.x));
        }

        //quita vida
        GetComponent<Health>().RestoreHealth(-0.5f);
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

    public void DamageUp(float damageUp)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.GetComponent<Projectile>().DamageUp(damageUp);
        }
    }
}
