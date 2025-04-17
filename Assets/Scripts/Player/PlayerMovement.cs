using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Setup")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;

    [Header("Physics Setup")]
    public bool usePhysicsGravity;
    public float gravityForce;
    public float settlingForce;
    public float bounceStrength = 5f;
    public float bounceDuration = 0.2f;

    [Header("Unity Setup")]
    public Transform groundCheck;
    public LayerMask standingMask;
    public float groundCheckRadius;
    public float groundedGracePeriod;

    // for physics checks
    private float yVelocity;
    private bool isGrounded;
    private bool checkForGround;
    private bool isBouncing;

    // for dash logic
    private bool isDashing;
    private Vector3 dashDir;

    // for character controller
    private CharacterController controller;
    private InputSystem_Actions actions;
    private InputAction move;
    private InputAction jump;
    private InputAction dash;

    private PlayerStats stats;

    void Awake()
    {
        actions = new InputSystem_Actions(); // assign in here bc OnEnable called before Start but not Awake

        if(usePhysicsGravity) {
            gravityForce = Physics.gravity.magnitude;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        stats = GetComponent<PlayerStats>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // check for ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, standingMask);

        if(!isDashing) {
            Move();
            Gravity();
        }
    }

    private void Move()
    {
        Vector2 readMove = move.ReadValue<Vector2>();

        Vector3 moveVec = transform.forward * readMove.y + transform.right * readMove.x;
        moveVec *= Time.deltaTime * moveSpeed;
        controller.Move(moveVec);
    }

    private void Gravity()
    {
        if(isGrounded && checkForGround) {
            yVelocity = settlingForce;
        }
        else {
            yVelocity -= gravityForce * Time.deltaTime;
        }

        controller.Move(Time.deltaTime * yVelocity * Vector3.up);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(!isGrounded) {
            return;
        }

        yVelocity = Mathf.Sqrt(jumpHeight * 2 * gravityForce);
        checkForGround = false;
        StartCoroutine(nameof(JumpGracePeriod));
    }

    private IEnumerator JumpGracePeriod()
    {
        yield return new WaitForSeconds(groundedGracePeriod);
        checkForGround = true;
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if(!isDashing) {
            // get movement direction
            Vector2 inputDir = move.ReadValue<Vector2>();
            Vector3 moveDir = transform.forward * inputDir.y + transform.right * inputDir.x;

            // default dir to forward if no input
            if(moveDir.sqrMagnitude < 0.01f) {
                moveDir = transform.forward;
            }

            dashDir = moveDir.normalized;
            isDashing = true;
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        float elapsedTime = 0f;
        while(elapsedTime < dashDuration) {
            controller.Move(dashDir * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "cactus" && !isBouncing) {
            Debug.Log("hit cactus");
            stats.TakeDamage();

            Vector3 bounceDir = hit.normal;
            // bounceDir.y = 0f; // ignore vertical bounce

            StartCoroutine(BouncePlayer(bounceDir.normalized));
        }
    }

    IEnumerator BouncePlayer(Vector3 dir)
    {
        isBouncing = true;
        float elapsedTime = 0f;

        while(elapsedTime < bounceDuration) {
            controller.Move(dir * (bounceStrength * Time.deltaTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isBouncing = false;
    }

    void OnEnable()
    {
        // input system boilerplate
        move = actions.Player.Move;
        move.Enable();

        jump = actions.Player.Jump;
        jump.Enable();
        jump.performed += Jump;

        dash = actions.Player.Dash;
        dash.Enable();
        dash.performed += Dash;
    }

    void OnDisable()
    {
        // input system boilerplate
        move.Disable();
        jump.Disable();
        dash.Disable();
    }
}
