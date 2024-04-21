// This and other movement-related scripts from https://youtu.be/f473C43s8nE
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private Transform orientation;
    private float slowDownTime = 2.5f;

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    private float defaultMoveSpeed;
    public float DefaultMoveSpeed
    {
        get => defaultMoveSpeed;
    }

    private bool doSpeedControl = true;
    public bool DoSpeedControl
    {
        get => doSpeedControl;
        set => doSpeedControl = value;
    }

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

    [SerializeField]
    private float playerHeight = 2f;

    private bool grounded;
    public bool Grounded
    {
        get { return grounded; }
        set { grounded = value; }
    }

    [Header("Slopes")]
    [SerializeField]
    private float maxSlope;

    [Header("Teleporting")]
    [SerializeField]
    private float lowestPoint = -5f;
    private RaycastHit slopeHit;

    [Header("Audio")]
    [SerializeField]
    private AudioSource footsteps;

    [SerializeField]
    private AudioSource jumpSound;

    [SerializeField]
    private AudioSource fallSound;

    private float xInput;
    private float zInput;
    private Vector3 moveDirection;

    public Rigidbody rb { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        defaultMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        // If the player is on a slope it also means they're grounded
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

        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * zInput + orientation.right * xInput;
    }

    private void FixedUpdate()
    {
        MovePlayer();

        // Handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;

        // Reset spawnpoint when player dies
        if (transform.position.y <= lowestPoint)
        {
            ResetPosition();
        }

        SpeedControl();
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        if (OnSlope())
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
            rb.AddForce(10f * moveSpeed * slopeDirection, ForceMode.Acceleration);
        }
        else if (grounded)
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
        else
            rb.AddForce(
                10f * airMultiplier * moveSpeed * moveDirection.normalized,
                ForceMode.Acceleration
            );

        // Prevents the player from sliding off slopes
        rb.useGravity = !OnSlope();
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        jumpSound.pitch = Random.Range(0.8f, 1f);
        jumpSound.Play();

        StartCoroutine(FallToGround());
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 2, 0);
        rb.velocity = Vector3.zero;

        // Sync the transforms; This is required otherwise the code won't work half of the time
        Physics.SyncTransforms();
    }

    // Resets the move to slope variable when the player is grounded again
    public IEnumerator FallToGround()
    {
        // A delay before waiting until the player is grounded again
        // Prevents the code from detecting a grounded state when the player just jumped
        yield return new WaitForSeconds(0.3f);

        // Wait until the player is grounded
        yield return new WaitUntil(() => grounded);

        // Play a sound:
        fallSound.pitch = Random.Range(0.8f, 1f);
        fallSound.Play();
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
            Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);

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


    // Unrealistc but fun deacceleration
    // This coroutine is started in the ProjectileCollision class
    // -- Since the player needs to speed up when hit by a projectile, but also must slow down
    public IEnumerator SlowDown()
    {
        for (float t = 0.0f; t < 1f; t += Time.deltaTime / slowDownTime)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, defaultMoveSpeed, t);
            yield return null;
        }

        moveSpeed = defaultMoveSpeed;
    }
}
