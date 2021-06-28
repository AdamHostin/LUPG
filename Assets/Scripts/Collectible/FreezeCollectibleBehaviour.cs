using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCollectibleBehaviour : MonoBehaviour
{
    [SerializeField] float freezeTime;
    [SerializeField] float freezeSmoothing;

    [Header("Dont touch")]
    [SerializeField] CollectibleController collectibleController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        
        Debug.Log("Freeze collected");
        collision.gameObject.GetComponent<CharacterController2D>().Freeze(freezeTime,freezeSmoothing);
        collectibleController.DisableCollectible();

            
        
    }
}