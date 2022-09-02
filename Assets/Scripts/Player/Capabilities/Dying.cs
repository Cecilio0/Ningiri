using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dying : MonoBehaviour
{

    public Collider2D playerCollider;
    public GameObject respawn;
    public LayerMask layerEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollider.IsTouchingLayers(layerEnemy))
        {
            gameObject.SetActive(false);
            gameObject.transform.position = respawn.transform.position;
            gameObject.SetActive(true);

        }
    }
}
