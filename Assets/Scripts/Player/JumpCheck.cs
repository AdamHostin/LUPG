using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    protected bool canJump = false;
    [SerializeField] protected string[] tags;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var tag in tags)
        {
            if (collision.CompareTag(tag))
                canJump = true;
        }
        
    }

    protected virtual void  OnTriggerExit2D(Collider2D collision)
    {
        foreach (var tag in tags)
        {
            if (collision.CompareTag(tag))
                canJump = false;
        }
    }

    public bool CanJump()
    {
        return canJump;
    }
}
