using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : JumpCheck
{
    [SerializeField] float coyoteeSeconds = .5f;
    //[SerializeField] int coyoteeFrames;
    bool isJumped = false;

    CharacterController2D player;

    private void Start()
    {
        player = GetComponentInParent<CharacterController2D>();
    }

    public void SetJump()
    {
        isJumped = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        StopCoroutine(CoyoteeJump());
        isJumped = false;
        if (player.animator != null) player?.animator.SetBool("Jump", false);

        if (collision.gameObject.CompareTag("Player"))
            player.JumpOnPlayer(collision.gameObject);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!isJumped)
        {
            StartCoroutine(CoyoteeJump());
        }
        else
        {
            canJump = false;
        }
        isGrounded = false;


    }

    IEnumerator CoyoteeJump()
    {


        yield return new WaitForSeconds(coyoteeSeconds);
        if (!isGrounded)
        {
            canJump = false;
        }
        
    }
}