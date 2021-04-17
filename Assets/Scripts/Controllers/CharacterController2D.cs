using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] int doubleJumpCount;

    Rigidbody2D rb;
    GroundCheck gc;

    int jumpCount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = GetComponentInChildren<GroundCheck>();
    }

    private void FixedUpdate()
    {
        var movement = Input.GetAxis("Horizontal");

        if (movement != 0)
        {
            rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
        }
        else if (Mathf.Abs(rb.velocity.x) > 0.001f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (gc.IsGrounded())
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpCount = doubleJumpCount;
            }
            else if (jumpCount > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpCount--;
            }
        }
        
    }
}