using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float jumpHeight;
    public bool usePhysicsGravity;
    public float gravityForce;
    public float settlingForce;

    public Transform groundCheck;
    public LayerMask standingMask;
    public float groundCheckRadius;
    public float groundedGracePeriod;

    private float yVelocity;
    private bool isGrounded;
    private bool checkForGround;

    private CharacterController controller;
    private InputSystem_Actions actions;
    private InputAction move;
    private InputAction jump;

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

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // check for ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, standingMask);

        Move();
        Gravity();
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

    void OnEnable()
    {
        // input system boilerplate
        move = actions.Player.Move;
        move.Enable();

        jump = actions.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
    }

    void OnDisable()
    {
        // input system boilerplate
        move.Disable();
        jump.Disable();
    }
}
