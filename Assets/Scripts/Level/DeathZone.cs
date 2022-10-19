using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            Health health = collision.GetComponent<Health>();
            health.TakeDamage(0b01110011011001010111100001101111, transform.position);
        }
    }
}
