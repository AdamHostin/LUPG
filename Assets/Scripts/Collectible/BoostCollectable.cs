using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCollectable : MonoBehaviour
{
    [SerializeField] float freezeTime;

    [Header("Dont touch")]
    [SerializeField] CollectibleController collectibleController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        Debug.Log("Freeze collected");
        collision.gameObject.GetComponent<CharacterController2D>().Boost();
        collectibleController.DisableCollectible();



    }
}
