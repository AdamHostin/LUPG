using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] int doubleJumpCount;
    [SerializeField] float wallJumpInterval;

    Rigidbody2D rb;
    GroundCheck gc;
    WallCheck wc;

    int jumpCount;
    bool canWallJump = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = GetComponentInChildren<GroundCheck>();
        wc = GetComponentInChildren<WallCheck>();
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
            if (gc.CanJump())
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpCount = doubleJumpCount;
            }
            else if (wc.CanJump() && canWallJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                StartCoroutine(WallJumpTimer());
            }
            else if (jumpCount > 0 && !wc.CanJump())
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpCount--;
            }
        }
        
    }

    IEnumerator WallJumpTimer()
    {
        canWallJump = false;
        yield return new WaitForSeconds(wallJumpInterval);
        canWallJump = true;
    }
}