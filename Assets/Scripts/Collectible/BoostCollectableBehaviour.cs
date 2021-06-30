using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCollectableBehaviour : MonoBehaviour
{
    [SerializeField] float boostedMoveSpeed;
    [SerializeField] float boostedTime;
    [SerializeField] bool isSpeedUp;

    [Header("Dont touch")]
    [SerializeField] CollectibleController collectibleController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;    
        collision.gameObject.GetComponent<CharacterController2D>().Boost(boostedMoveSpeed, boostedTime);
        collectibleController.DisableCollectible();

        if (isSpeedUp)
            App.audioManager.Play("SpeedUp");
        else
            App.audioManager.Play("SlowDown");

    }
}
