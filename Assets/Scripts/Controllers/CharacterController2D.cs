using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;

    Rigidbody2D rb;
    GroundCheck gc;

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

        if (Input.GetButtonDown("Jump") && gc.IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
}