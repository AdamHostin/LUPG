using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    bool canJump = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jumpable"))
            canJump = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Jumpable"))
            canJump = false;
    }

    public bool CanJump()
    {
        return canJump;
    }
}
