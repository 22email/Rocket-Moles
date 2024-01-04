// This and other movement-related scripts from https://youtu.be/f473C43s8nE
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private Transform orientation;

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

    [SerializeField]
    private AudioSource fallSound;

    private float xInput;
    private float zInput;
    private Vector3 moveDirection;

    public Rigidbody Rb { get; set; }

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Rb.freezeRotation = true;
        defaultMoveSpeed = moveSpeed;
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

    private void FixedUpdate()
    {
        MovePlayer();

        // Handle drag
        if (grounded)
            Rb.drag = groundDrag;
        else
            Rb.drag = 0f;

        // Reset spawnpoint when player dies
        if (transform.position.y <= lowestPoint)
        {
            ResetPosition();
        }

        SpeedControl();
    }

    private void MovePlayer()
    {
        // calculate movement direction

        if (OnSlope())
        {
            Rb.AddForce(GetSlopeDirection() * moveSpeed * 10f, ForceMode.Acceleration);

            if (Rb.velocity.y > 0 && moveToSlope)
            {
                Rb.velocity = GetSlopeDirection() * Rb.velocity.magnitude;
            }
        }
        else if (grounded)
            Rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Acceleration);
        else 
            Rb.AddForce(
                moveDirection.normalized * moveSpeed * 10f * airMultiplier,
                ForceMode.Acceleration
            );

        // Prevents the player from sliding off slopes
        Rb.useGravity = !OnSlope();
    }

    private void Jump()
    {
        // Don't adjust velocity to slope's direction
        moveToSlope = false;

        Rb.velocity = new Vector3(Rb.velocity.x, 0, Rb.velocity.z);
        Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        jumpSound.pitch = Random.Range(0.8f, 1f);
        jumpSound.Play();

        StartCoroutine(FallToGround());
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 2, 0);
        Rb.velocity = Vector3.zero;

        // This is required otherwise the code won't work half of the time
        Physics.SyncTransforms();
    }

    // Resets the move to slope variable when the player is grounded again
    public IEnumerator FallToGround()
    {
        // A delay before waiting until the player is grounded again
        // Prevents the code from detecting when the player
        yield return new WaitForSeconds(0.3f);

        // Wait until the player is grounded
        yield return new WaitUntil(() => grounded);

        // Play a sound:
        fallSound.pitch = Random.Range(0.8f, 1f);
        fallSound.Play();

        // Add a small delay in case
        // yield return new WaitForSeconds(0.1f);
        moveToSlope = true;
    }

    // Limits the speed of the player to the moveSpeed variable
    // This method is called in FixedUpdate() since it polls the Rigidbody's velocity
    private void SpeedControl()
    {
        if (OnSlope())
        {
            if (Rb.velocity.sqrMagnitude > moveSpeed * moveSpeed)
            {
                Rb.velocity = Rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);

            // Limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                Rb.velocity = new Vector3(limitedVel.x, Rb.velocity.y, limitedVel.z);
            }
        }
    }

    // Unrealistc but fun deacceleration
    // This coroutine is started in the ProjectileCollision class
    // -- Since the player needs to speed up when hit by a projectile, but also must slow down
    public IEnumerator SlowDown()
    {
        for (float t = 0.0f; t < 1f; t += Time.deltaTime / 7.0f)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, defaultMoveSpeed, t);
            yield return null;
        }

        moveSpeed = defaultMoveSpeed;
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

    private Vector3 GetSlopeDirection() =>
        Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
}
