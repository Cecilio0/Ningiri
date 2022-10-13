using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRestore : MonoBehaviour
{

    [SerializeField] private float healthRestored;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            collision.GetComponent<Health>().RestoreHealth(healthRestored);
            gameObject.SetActive(false);
        }
    }
}
