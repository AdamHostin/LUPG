using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    [SerializeField] protected bool canJump = true;
    [SerializeField] protected bool isGrounded = true;
    [SerializeField] protected string[] tags;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var tag in tags)
        {
            if (collision.CompareTag(tag))
            {
                canJump = true;
                isGrounded = true;
                break;
            }
                
        }
        
    }

    protected virtual void  OnTriggerExit2D(Collider2D collision)
    {
        foreach (var tag in tags)
        {
            if (collision.CompareTag(tag))
            {
                canJump = false;
                isGrounded = false;
                break;
            }          
        }
    }

    public bool CanJump()
    {
        return canJump;
    }
}
