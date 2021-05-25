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
    [SerializeField] bool dashType;
    [SerializeField] float dashBorder;
    [SerializeField] float blockTime;
    [SerializeField] float timeBetweenBlocks;
    [SerializeField] float smallRepulsionForce;
    [SerializeField] float bigRepulsionForce;
    [SerializeField] int jumpOnHeal;
    [SerializeField] int jumpOnDamage;
    [Header("Debug don't touch")]
    [SerializeField] bool isFaceRight = true;
    Rigidbody2D rb;
    GroundCheck gc;
    WallCheck wc;
    PlayerHealth playerHealth;
    

    int jumpCount;
    bool canWallJump = true;
    public bool isJumping = false;

    int movement;


    float movementSmoothing = .3f;
    Vector3 targetVelocity, lastVelocity = Vector3.zero;

    [SerializeField] bool canBlock = true;
    [SerializeField] bool isBlocked = false;
    [SerializeField] bool isDashed = false;

    int playerIndex = 0;

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
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterController2D playerController = collision.gameObject.GetComponent<CharacterController2D>();
            PlayerHealth otherPlayerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (isDashed)
            {
                if (playerController.IsDashed() && playerIndex > playerController.GetPlayerIndex())
                {
                    //Both dashed
                    int targetHealth = (playerHealth.GetHealth() + otherPlayerHealth.GetHealth()) / 2;
                    playerHealth.SetHealth(targetHealth);
                    otherPlayerHealth.SetHealth(targetHealth);
                }
                else if (!playerController.IsDashed())
                {
                    //One dashed
                    playerController.Repulse(CalculateDirection(transform.position, collision.gameObject.transform.position), bigRepulsionForce);
                }
                else if (playerController.IsBlocked())
                {
                    //One dashed one blocked
                    Repulse(CalculateDirection(collision.gameObject.transform.position, transform.position), bigRepulsionForce);
                }
            }
            else
            {
                if (playerController.IsBlocked())
                {
                    //The other blocks
                    Repulse(CalculateDirection(collision.gameObject.transform.position, transform.position), smallRepulsionForce);
                }
                else if (!playerController.IsBlocked() && !playerController.IsDashed() && playerIndex > playerController.GetPlayerIndex())
                {
                    //Just run into each other
                    Repulse(CalculateDirection(collision.gameObject.transform.position, transform.position), smallRepulsionForce);
                    playerController.Repulse(CalculateDirection(transform.position, collision.gameObject.transform.position), smallRepulsionForce);
                }
            }
        }
    }

    public void JumpOnPlayer(GameObject player)
    {
        CharacterController2D playerController = player.GetComponent<CharacterController2D>();
        PlayerHealth otherPlayerHealth = player.GetComponent<PlayerHealth>();

        if (!isDashed && !playerController.IsDashed())
        {
            otherPlayerHealth.Heal(jumpOnHeal);
            playerHealth.Damage(jumpOnDamage);
        }
    }

    public void Repulse(Vector3 direction, float magnitude)
    {
        rb.AddForce(direction * magnitude, ForceMode2D.Impulse);
    }

    public Vector3 CalculateDirection(Vector3 origin, Vector3 target)
    {
        return Vector3.Normalize(target - origin);
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

        if (!isBlocked)
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
        }
        StartCoroutine(ManageDashState());
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

    public void ManageBlock(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (Input.GetKeyDown(KeyCode.E) && !isDashed && canBlock)
        {
            StartCoroutine(ManageBlockState());
            //Block animation
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

    IEnumerator ManageDashState()
    {
        isDashed = true; Debug.Log("dash true");

        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => Mathf.Abs(rb.velocity.x) < dashBorder);

        isDashed = false; Debug.Log("dash false");
    }

    IEnumerator ManageBlockState()
    {
        isBlocked = true;
        canBlock = false;
        yield return new WaitForSeconds(blockTime);
        isBlocked = false;

        yield return new WaitForSeconds(timeBetweenBlocks);
        canBlock = true;
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

    public bool IsBlocked()
    {
        return isBlocked;
    }

    private void FixedUpdate()
    {
         rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref lastVelocity, movementSmoothing);
    }
}