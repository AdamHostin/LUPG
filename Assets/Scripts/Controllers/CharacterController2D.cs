using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] int doubleJumpCount;
    [SerializeField] float wallJumpInterval;
    [SerializeField] float dashValue;
    [Header("Debug don't touch")]
    [SerializeField] bool isFaceRight = true;
    Rigidbody2D rb;
    GroundCheck gc;
    WallCheck wc;
    

    int jumpCount;
    bool canWallJump = true;
    public bool isJumping = false;

    int movement;


    float movementSmoothing = .3f;
    Vector3 targetVelocity, lastVelocity = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
        
        gc = GetComponentInChildren<GroundCheck>();
        wc = GetComponentInChildren<WallCheck>();

    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = (int)context.ReadValue<float>();

        if (!context.performed)
            movement = 0;

        targetVelocity = new Vector2(movement * movementSpeed, rb.velocity.y);
        ResolveFacing();
    }

    public void ManageDash(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (rb.velocity.x > Mathf.Epsilon)
        {
            rb.AddForce(rb.velocity.normalized * dashValue);
        }
        else
        {
            if (isFaceRight)
            {
                rb.AddForce(new Vector2(1, 0) * dashValue);
            }
            else
            {
                rb.AddForce(new Vector2(-1, 0) * dashValue);
            }
        }
    }

    void ResolveFacing()
    {
        if (movement == 0) return;
        else if (movement > 0 && !isFaceRight)
        {
            transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
            isFaceRight = true;
        }
        else if (movement < 0 && isFaceRight)
        {
            transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
            isFaceRight = false;
        }
        

    }

    public void ManageJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            isJumping = false;
            return;
        }

        isJumping = true;

        if (gc.CanJump())
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpCount = doubleJumpCount;
            gc.SetJump();
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