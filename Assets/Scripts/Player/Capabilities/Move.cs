using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    private PlayerControls inputs;

    public float maxSpeed;
    public float maxAcceleration;
    public float maxAirAcceleration;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;

    private Rigidbody2D body;
    private Ground ground;
    private bool onGround;

    private float maxSpeedChange;
    private float acceleration;


    // Start is called before the first frame update
    void Awake()
    {
        inputs = new PlayerControls();
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
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
        if (direction.x < 0)
            transform.localScale = new Vector2( Mathf.Abs(transform.localScale.x), transform.localScale.y);
        else if (direction.x > 0)
            transform.localScale = new Vector2( -Mathf.Abs(transform.localScale.x), transform.localScale.y);
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.Friction, 0f);
    }

    private void FixedUpdate()
    {
        onGround = ground.OnGround;
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        body.velocity = velocity;
    }
}
