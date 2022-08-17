using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    public Transform ground;
    public float playerSpeed = 1.0f;
    public float jumpStrength = 1.0f;
    public float gravity = -10.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("se crea");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(direction);
    }

    private void Move(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * playerSpeed, rb.velocity.y);
    }
}
