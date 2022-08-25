using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemigos : MonoBehaviour
{

    private PlayerControls inputs;

    public float maxSpeed;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;

    private Rigidbody2D body;

    private float maxSpeedChange;
    private float acceleration;


    // Start is called before the first frame update
    void Awake()
    {
        inputs = new PlayerControls();
        body = GetComponent<Rigidbody2D>();
    }

    //para el funcionamiento de los controles
    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = inputs.Land.Horizontal.ReadValue<Vector2>().x;
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed, 0f);
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;

        velocity.x = Time.deltaTime*maxSpeed;
        body.velocity = velocity;
    }
}
