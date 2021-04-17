using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    bool isGrounded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jumpable"))
            isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Jumpable"))
            isGrounded = false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}