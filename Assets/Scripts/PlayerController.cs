using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d; 
    public float speed = 2f;
    public float jumpForce = 5f;
    Vector2 input;
    bool onGround;
    bool onWall;

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
    }

    public void OnJump(InputValue value)
    {
        if (!onGround && !onWall) return;

        if (onWall) onWall = false;

        Debug.Log("Jumped");
        rb2d.linearVelocityY = jumpForce;
    }

    void SetFaceDirection(int faceDirection)
    {
        int faceFactor = faceDirection == transform.localScale.x ? 1 : -1;

        transform.localScale = new Vector3(
            transform.localScale.x * faceFactor,
            transform.localScale.y,
            transform.localScale.z);
    }
}
