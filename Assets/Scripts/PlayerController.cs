using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;
    Vector2 inputMove;
    InputAction moveAction;
    InputAction jumpAction;
    PlayerInput playerInput;
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpForce = 5f;
    bool onGround;
    bool onRamp;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        jumpAction.performed += OnJumpPerformed;

    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!onGround && !onRamp) return;
        rb2d.linearVelocityY = jumpForce;
    }

    void Update()
    {
        inputMove = moveAction.ReadValue<Vector2>();

        animator.SetFloat("inputMoveX", Mathf.Abs(inputMove.x));
        animator.SetFloat("velocityY", rb2d.linearVelocityY);
    }

    void FixedUpdate()
    {
        rb2d.linearVelocityX = inputMove.x * speed; 
    }

    public void OnMove(InputValue value)
    {
        inputMove = value.Get<Vector2>();
        if (inputMove.x == 0.0) return;
        
        int faceDirection = inputMove.x > 0 ? 1 : -1;
        SetFaceDirection(faceDirection);
        Debug.Log("Moving: " + inputMove);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            animator.SetBool("isGrounded", true);
        }
        if (collision.gameObject.CompareTag("Ramps"))
        {
            onRamp = true;
            animator.SetBool("isGrounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
            animator.SetBool("isGrounded", false);
        }
        if (collision.gameObject.CompareTag("Ramps"))
        {
            onRamp = false;
            animator.SetBool("isGrounded", false);
        }
    }

    void SetFaceDirection(int faceDirection)
    {

        int faceFactor = Math.Sign(faceDirection) == Math.Sign(transform.localScale.x) ? 1 : -1;
        transform.localScale = new Vector3(
            transform.localScale.x * faceFactor,
            transform.localScale.y,
            transform.localScale.z);
    }
}
