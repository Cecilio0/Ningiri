using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{

    private bool onGround;
    private float friction;

    public bool OnGround 
    {
        get { return onGround; }
    }

    public float Friction
    {
        get { return friction; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        friction = 0;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }

    }

    private void RetrieveFriction(Collision2D collision)
    {
        friction = 0;
        PhysicsMaterial2D material;
        if (collision.rigidbody != null)
            material = collision.rigidbody.sharedMaterial;
        else return;
        
        if (material != null)
        {
            friction = material.friction;
        }
    }

}
