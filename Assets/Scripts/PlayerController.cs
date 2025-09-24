using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d; 
    public float speed = 2f;
    Vector2 input;
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
}
