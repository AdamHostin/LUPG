using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpModifier : MonoBehaviour
{
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    Rigidbody2D rb;
    CharacterController2D player;
    PlayerInput controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<CharacterController2D>();
        controls = GetComponent<PlayerInput>();
    }

    


    void Update()
    {
        if (rb.velocity.y < -Mathf.Epsilon)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * rb.gravityScale;
        }
        else if (rb.velocity.y  > 0 && !player.isJumping)
        {
            rb.velocity += new Vector2(transform.up.x, transform.up.y) * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime * rb.gravityScale;
        }
    }
}
