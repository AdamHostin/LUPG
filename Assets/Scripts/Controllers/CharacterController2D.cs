using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float movementSmoothing = .3f;
    [SerializeField] float jumpForce;
    [SerializeField] int doubleJumpCount;
    [SerializeField] float wallJumpInterval;
    [SerializeField] float dashValue;
    [SerializeField] bool isDashStraight;
    [SerializeField] float dashBorder;
    [SerializeField] float blockTime;
    [SerializeField] float timeBetweenBlocks;
    [SerializeField] float smallRepulsionForce;
    [SerializeField] float bigRepulsionForce;
    [SerializeField] int jumpOnHeal;
    [SerializeField] int jumpOnDamage;
    [SerializeField] DashType dashType;
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


    
    Vector3 targetVelocity, lastVelocity = Vector3.zero;
    float targetVelocityX = 0f;

    [SerializeField] bool canBlock = true;
    [SerializeField] bool isBlocked = false;
    [SerializeField] bool isDashed = false;

    int playerIndex = 0;

    PlayerInput playerInput;
    PlayerAvatar playerAvatar;
    int avatarIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
        
        gc = GetComponentInChildren<GroundCheck>();
        wc = GetComponentInChildren<WallCheck>();
        playerHealth = GetComponent<PlayerHealth>();

        playerAvatar = App.lobbyScreen.GetAvatar(this);
        if (playerAvatar == null)
            Destroy(gameObject);

        playerIndex = App.playerManager.GetPlayerIndex();
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
                else if (playerController.IsBlocked())
                {
                    Debug.Log("pow " + CalculateDirection(collision.gameObject.transform.position, transform.position));
                    //One dashed one blocked
                    Repulse(CalculateDirection(collision.gameObject.transform.position, transform.position), bigRepulsionForce);
                }
                else if (!playerController.IsDashed())
                {
                    //One dashed
                    playerController.Repulse(CalculateDirection(transform.position, collision.gameObject.transform.position), bigRepulsionForce);
                }
            }
            else
            {
                if (playerController.IsBlocked())
                {
                    //The other blocks
                    Repulse(CalculateDirection(collision.gameObject.transform.position, transform.position), smallRepulsionForce);
                }
                else if (!playerController.IsDashed() && playerIndex > playerController.GetPlayerIndex())
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

        targetVelocityX = movement * movementSpeed;
        ResolveFacing();
    }

    public void ManageDash(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!isBlocked)
        {

            switch (dashType)
            {
                case DashType.straight:
                    DashStraight();
                    break;
                case DashType.hight:
                    DashWithHight();
                    break;
                case DashType.combined:
                    DashCombined();
                    break;
                default:
                    break;
            }
            
        }
        StartCoroutine(ManageDashState());
    }

    void DashWithHight()
    {
        if (isFaceRight)
        {
            rb.AddForce(new Vector2(1, rb.velocity.normalized.y) * dashValue);
        }
        else
        {
            rb.AddForce(new Vector2(-1, rb.velocity.normalized.y) * dashValue);
        }
    }

    void DashCombined()
    {
        if (rb.velocity.y > 0)
        {
            DashWithHight();
        }
        else
        {
            DashStraight();
        }
    }

    void DashStraight()
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

        if (!isDashed && canBlock)
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
        Debug.Log("isPressed");
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
        isDashed = true;

        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => Mathf.Abs(rb.velocity.x) < dashBorder);

        isDashed = false;
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

    public void SetAvatarIndex(int index)
    {
        avatarIndex = index;
    }

    private void FixedUpdate()
    {
        targetVelocity = new Vector2(targetVelocityX, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref lastVelocity, movementSmoothing);
    }

    public void TogglePlayerInput(bool value)
    {
        playerInput.enabled = value;
    }

    public void ChooseCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return; 

        if (App.lobbyScreen.CanChoose())
        {
            if ((int) context.ReadValue<float>() > 0)
            {
                playerAvatar.IncrementImage();
            }
            else
            {
                playerAvatar.DecrementImage();
            }
        }
    }

    public void ReadyUp(InputAction.CallbackContext context)
    {
        if (App.lobbyScreen.CanChoose())
        {
            playerAvatar.ToggleReady();
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}