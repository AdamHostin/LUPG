using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    [SerializeField] float timeToStart;
    [SerializeField] float yEndPosition;
    [SerializeField] float waterSpeed;

    Stack<Sprite> playerStack = new Stack<Sprite>();

    private void Start()
    {
        StartWaterCountdown();
    }

    public void StartWaterCountdown()
    {
        StartCoroutine(RiseWater());
    }

    IEnumerator RiseWater()
    {
        yield return new WaitForSeconds(timeToStart);

        while (transform.position.y < yEndPosition)
        {
            transform.Translate(Vector3.up * Time.deltaTime * waterSpeed);
            Debug.Log("wait");
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //playerStack.Push(collision.gameObject.GetComponent<PlayerHealth>().GetAvatar());

            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            App.playerManager.KillPlayersBySuddenDeath(player,player.GetAvatar());
            App.playerManager.ClearPlayer(collision.gameObject.GetComponent<PlayerHealth>());
        }
    }
}