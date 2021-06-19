using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    [SerializeField] float timeToStart;
    [SerializeField] float yEndPosition;
    [SerializeField] float waterSpeed;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Kill player
        }
    }
}