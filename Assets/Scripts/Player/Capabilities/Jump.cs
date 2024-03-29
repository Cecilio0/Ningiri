using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    private PlayerInput inputs;

    [SerializeField]private float jumpHeight;
    [SerializeField]private int maxAirJumps;
    [SerializeField]private float downwardMovementMultiplier;
    [SerializeField]private float upwardMovementMultiplier;

    private Rigidbody2D body;
    private Ground ground;
    private Vector2 velocity;

    private int jumpPhase;
    private float defaultGravityScale;

    private bool desiredJump;
    private bool onGround;
    [HideInInspector] public bool isKnockedBack;
    // Start is called before the first frame update
    void Awake()
    {
        inputs = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();

        defaultGravityScale = 1f;
        isKnockedBack = false;
    }
    // Update is called once per frame
    void Update()
    {
        //se usa |= ya que update y fixedUpdate() ocurren en intervalos distintos entonces que sea verdadero hasta que entre a fixedUpdate
        if (!isKnockedBack)
            desiredJump |= inputs.actions["Jump"].WasPressedThisFrame();
    }

    private void FixedUpdate()
    {
        onGround = ground.OnGround;
        velocity = body.velocity;

        if (onGround)
        {
            jumpPhase = 0;
        }

        if (desiredJump)
        {
            desiredJump = false;
            JumpAction();
        }

        if (body.velocity.y > 0)
        {
            body.gravityScale = upwardMovementMultiplier;
        } 
        else if(body.velocity.y < 0)
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        else if(body.velocity.y == 0)
        {
            body.gravityScale = defaultGravityScale;
        }

        body.velocity = velocity;
    }

    private void JumpAction()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }

}
