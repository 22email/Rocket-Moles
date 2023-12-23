// This and other movement-related scripts from https://youtu.be/f473C43s8nE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float groundDrag;

    [SerializeField]
    private float airMultiplier;

    [SerializeField]
    private float jumpCooldown;

    [Header("Jumping")]
    [SerializeField]
    private float jumpForce;
    private bool wishJump;
    private bool canJump;

    [Header("Keybinds")]
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField]
    private LayerMask whatIsGround;

    private bool grounded;
    public bool Grounded
    {
        get { return grounded; }
        set { grounded = value; }
    }

    [Header("Slopes")]
    [SerializeField]
    private float maxSlope;

    [SerializeField]
    private float playerHeight = 2f;

    [Header("Teleporting")]
    [SerializeField]
    private float lowestPoint = -5f;
    private RaycastHit slopeHit;

    private float xInput;
    private float zInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.3f,
            whatIsGround
        );

        GetInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else if (onSlope())
            rb.drag = groundDrag;
        else
            rb.drag = 0f;

        if (transform.position.y <= lowestPoint)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 2, 0);
        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        // Gets input for x and z axis
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        // jump buffering
        // Buffers the jump when jump key is held down
        if (Input.GetKeyDown(jumpKey) && !wishJump)
            wishJump = true;
        if (Input.GetKeyUp(jumpKey))
            wishJump = false;

        // when to jump
        if (wishJump && grounded && canJump)
        {
            Jump();
            wishJump = false;
            canJump = false;

            // Immediately resets the jump boolean
            // Mostly for slope handling
            Invoke(nameof(ResetJumpCooldown), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * zInput + orientation.right * xInput;

        if (onSlope() && canJump)
        {
            rb.AddForce(getSlopeDir() * moveSpeed * 20f, ForceMode.Acceleration);

            if (rb.velocity.y > 0)
                rb.velocity = new Vector3(rb.velocity.x, -2f, rb.velocity.z);
        }
        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Acceleration);
        // in air
        else if (!grounded)
            rb.AddForce(
                moveDirection.normalized * moveSpeed * 10f * airMultiplier,
                ForceMode.Acceleration
            );

        rb.useGravity = !onSlope();
    }

    // Handle speed control
    private void SpeedControl()
    {
        if (onSlope() && canJump)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    // Jump
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void ResetJumpCooldown()
    {
        canJump = true;
    }

    private bool onSlope()
    {
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out slopeHit,
                playerHeight * 0.5f + 0.3f
            )
        )
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlope && angle != 0;
        }

        return false;
    }

    private Vector3 getSlopeDir()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
