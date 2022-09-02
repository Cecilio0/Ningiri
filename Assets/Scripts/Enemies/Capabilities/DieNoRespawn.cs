using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieNoRespawn : MonoBehaviour
{
    public Collider2D objectCollider;
    public EdgeCollider2D deathCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectCollider.IsTouching(deathCollider))
        {
            gameObject.SetActive(false);
        }
    }
}
