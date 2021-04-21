using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : JumpCheck
{
    [SerializeField] float coyoteeSeconds = .5f;
    //[SerializeField] int coyoteeFrames;
    bool isJumped = false;

    public void SetJump()
    {
        isJumped = true;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        StopCoroutine(CoyoteeJump());
        isJumped = false;
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