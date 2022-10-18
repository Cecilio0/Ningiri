using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    [SerializeField] private float healthUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            collision.GetComponent<Health>().HealthUp(healthUp);
            collision.GetComponent<Health>().RestoreHealth(healthUp);
            gameObject.SetActive(false);
        }
    }
}
