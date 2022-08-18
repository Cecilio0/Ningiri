using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private PlayerControls playerControls;//para tener los inputs genericos que creamos
    public float playerSpeed;
    public float jumpHeight;
    private Transform tr;
    private Rigidbody2D rb;
    private bool isGrounded;


    private void Awake()
    {
        playerControls = new PlayerControls();

        playerSpeed = 1.0f;
        jumpHeight = 1.0f;

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Start()
    {
        
    }

    private void Update()
    {
        Vector2 move = playerControls.Land.Horizontal.ReadValue<Vector2>();
        bool jump = playerControls.Land.Jump.WasPressedThisFrame();

        if (move.x != 0)
        {
            tr.right = new Vector2(move.x, 0);
            rb.velocity = new Vector2(playerSpeed*move.normalized.x, rb.velocity.y);
        }

        if (jump)
        {
            Debug.Log("salta");
        }
    }

    private void Jump()
    {

    }

}
