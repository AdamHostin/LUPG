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

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!isJumped)
        {
            Debug.Log("Coyotee");
            StartCoroutine(CoyoteeJump());
        }
        else
        {
            canJump = false;
            isJumped = false;
        }
        

    }

    IEnumerator CoyoteeJump()
    {

        //yield return coyoteeFrames;

        yield return new WaitForSeconds(coyoteeSeconds);

        canJump = false;
    }
}