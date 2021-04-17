using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] int doubleJumpCount;
    [SerializeField] float wallJumpInterval;
    [SerializeField] bool isFaceRight = true;
    Rigidbody2D rb;
    GroundCheck gc;
    WallCheck wc;
    

    int jumpCount;
    bool canWallJump = true;

    int movement;


    float movementSmoothing = .3f;
    Vector3 targetVelocity, lastVelocity = Vector3.zero;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
        
        rb = GetComponent<Rigidbody2D>();
        gc = GetComponentInChildren<GroundCheck>();
        wc = GetComponentInChildren<WallCheck>();

    }

    private void Update()
    {
        movement = (int) Input.GetAxisRaw("Horizontal");
        targetVelocity = new Vector2(movement * movementSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Dash"))
        {
            Debug.Log("Dash");
        }

        ManageJump();
        ResolveFacing();
       
        
    }

    void ResolveFacing()
    {
        if (movement == 0) return;
        else if (movement > 0 && !isFaceRight)
        {
            gameObject.transform.localScale *= (-1);
            isFaceRight = true;
        }
        else if (movement < 0 && isFaceRight)
        {
            gameObject.transform.localScale *= (-1);
            isFaceRight = false;
        }
        

    }

    void ManageJump()
    {
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

    private void FixedUpdate()
    {
         rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref lastVelocity, movementSmoothing);
    }
}