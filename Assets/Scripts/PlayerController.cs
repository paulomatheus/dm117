using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d; 
    Vector2 input;
    public float speed = 2f;
    public float jumpForce = 2f;
    bool onGround;
    bool onRamp;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb2d.linearVelocityX = input.x * speed;
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        if (input.x == 0.0) return;
        
        int faceDirection = input.x > 0 ? 1 : -1;
        SetFaceDirection(faceDirection);
        Debug.Log("Moving: " + input);
    }

    public void OnJump(InputValue value)
    {
        if (!onGround && !onRamp) return;

        Debug.Log("Jumped");
        rb2d.linearVelocityY = jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
        if (collision.gameObject.CompareTag("Ramps"))
        {
            onRamp = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
        if (collision.gameObject.CompareTag("Ramps"))
        {
            onRamp = false;
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
