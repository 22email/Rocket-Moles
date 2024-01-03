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

    [Header("Jumping")]
    [SerializeField]
    private float jumpForce;
    private bool wishJump;

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

    private bool moveToSlope = true;
    public bool MoveToSlope
    {
        get => moveToSlope;
        set => moveToSlope = value;
    }

    [SerializeField]
    private float playerHeight = 2f;

    [Header("Teleporting")]
    [SerializeField]
    private float lowestPoint = -5f;
    private RaycastHit slopeHit;

    [Header("Audio")]
    [SerializeField]
    private AudioSource footsteps;
    [SerializeField]
    private AudioSource jumpSound;

    private float xInput;
    private float zInput;
    private Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // ground check
        grounded =
            Physics.Raycast(
                transform.position,
                Vector3.down,
                playerHeight * 0.5f + 0.2f,
                whatIsGround
            ) || OnSlope();

        GetInput();

        if (grounded && (moveDirection.sqrMagnitude > 0))
        {
            footsteps.enabled = true;
            footsteps.pitch = Mathf.Lerp(0.8f, 1f, Mathf.PerlinNoise1D(Time.time));
        }
        else
        {
            footsteps.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;

        if (transform.position.y <= lowestPoint)
        {
            ResetPosition();
        }

        SpeedControl();
    }

    private void GetInput()
    {
        // Jump buffering
        // Buffers the jump when jump key is held down
        // Similar to how quake handles jumping
        if (Input.GetKeyDown(jumpKey) && !wishJump)
            wishJump = true;

        if (Input.GetKeyUp(jumpKey))
            wishJump = false;

        if (wishJump && grounded)
        {
            Jump();
            wishJump = false;
        }

        // Calculates the movement direction given the player's orientation and input on the X (A & D) and Z (W & S) axis
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
        // Multiply the forward vector with the Z axis since it represents moving backing and forth
        // Multiply the rightward vector with the X axis since it represents moving right and left
        // Keep in mind that right is positive
        moveDirection = orientation.forward * zInput + orientation.right * xInput;
    }

    private void MovePlayer()
    {
        // calculate movement direction

        if (OnSlope())
        {
            rb.AddForce(GetSlopeDirection() * moveSpeed * 10f, ForceMode.Acceleration);
            if (rb.velocity.y > 0 && moveToSlope)
            {
                rb.velocity = GetSlopeDirection() * rb.velocity.magnitude;
            }
        }
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Acceleration);
        // in air
        else if (!grounded)
            rb.AddForce(
                moveDirection.normalized * moveSpeed * 10f * airMultiplier,
                ForceMode.Acceleration
            );

        rb.useGravity = !OnSlope();
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 2, 0);
        rb.velocity = Vector3.zero;

        Physics.SyncTransforms();
    }

    // Jump
    private void Jump()
    {
        // rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        moveToSlope = false;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpSound.pitch = Random.Range(0.8f, 1f);
        jumpSound.Play();

        StartCoroutine(ResetMoveToSlope());
    }

    // Resets the move to slope variable when the player is grounded again
    public IEnumerator ResetMoveToSlope()
    {
        // Wait until the player is grounded
        yield return new WaitUntil(() => grounded);

        // Add a small delay (this doesn't feel right)
        yield return new WaitForSeconds(0.1f);
        moveToSlope = true;
    }

    // Limits the speed of the player to the moveSpeed variable
    // This method is called in FixedUpdate() since it polls the Rigidbody's velocity
    private void SpeedControl()
    {
        if (OnSlope())
        {
            if (rb.velocity.sqrMagnitude > moveSpeed * moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Limit velocity if needed
            if (flatVel.sqrMagnitude > moveSpeed * moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private bool OnSlope()
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

    private Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
