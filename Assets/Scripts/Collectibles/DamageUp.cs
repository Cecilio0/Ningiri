using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : MonoBehaviour
{
    [SerializeField] private float meleeDamageUp;
    [SerializeField] private float projectileDamageUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            collision.GetComponent<Melee>().DamageUp(meleeDamageUp);
            collision.GetComponent<Shoot>().DamageUp(projectileDamageUp);
            gameObject.SetActive(false);
        }
    }
}
