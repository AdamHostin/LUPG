using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCollectableBehaviour : MonoBehaviour
{
    [SerializeField] float boostedMoveSpeed;
    [SerializeField] float boostedTime;

    [Header("Dont touch")]
    [SerializeField] CollectibleController collectibleController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;    
        collision.gameObject.GetComponent<CharacterController2D>().Boost(boostedMoveSpeed, boostedTime);
        collectibleController.DisableCollectible();



    }
}
