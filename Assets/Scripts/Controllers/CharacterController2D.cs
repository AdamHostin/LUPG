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
    [SerializeField] float dashValue;
    [SerializeField] bool dashType;
    [SerializeField] float dashBorder;
    [Header("Debug don't touch")]
    [SerializeField] bool isFaceRight = true;
    Rigidbody2D rb;
    GroundCheck gc;
    WallCheck wc;
    PlayerHealth playerHealth;
    

    int jumpCount;
    bool canWallJump = true;

    int movement;


    float movementSmoothing = .3f;
    Vector3 targetVelocity, lastVelocity = Vector3.zero;

    [SerializeField] bool isDashed = false;

    int playerIndex = 0;        //Set by some manager when player joins the game

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
        
        rb = GetComponent<Rigidbody2D>();
        gc = GetComponentInChildren<GroundCheck>();
        wc = GetComponentInChildren<WallCheck>();
        playerHealth = GetComponent<PlayerHealth>();

    }

    private void Update()
    {
        movement = (int) Input.GetAxisRaw("Horizontal");
        targetVelocity = new Vector2(movement * movementSpeed, rb.velocity.y);

        ManageDash();

        ManageJump();
        ResolveFacing();


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterController2D playerController = collision.gameObject.GetComponent<CharacterController2D>();
            PlayerHealth otherPlayerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (isDashed && playerController.IsDashed() && playerIndex > playerController.GetPlayerIndex())
            {
                int targetHealth = (playerHealth.GetHealth() + otherPlayerHealth.GetHealth()) / 2;
                playerHealth.SetHealth(targetHealth);
                otherPlayerHealth.SetHealth(targetHealth);
            }
        }
    }

    void ManageDash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (rb.velocity.x > Mathf.Epsilon)
            {
                if (dashType)
                    rb.AddForce(rb.velocity.normalized * dashValue);
                else
                    rb.AddForce(new Vector3(rb.velocity.normalized.x, 0, 0) * dashValue);
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

            StartCoroutine(ManageDashState());
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

    void ManageJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
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
    IEnumerator ManageDashState()
    {
        isDashed = true; Debug.Log("dash true");

        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => Mathf.Abs(rb.velocity.x) < dashBorder);

        isDashed = false; Debug.Log("dash false");
    }

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public bool IsDashed()
    {
        return isDashed;
    }
}