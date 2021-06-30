using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotCollectibleBehaviour : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] CollectibleController collectibleController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        Debug.Log("Coll");
        //Debug.Log("before damage " + collision.gameObject.GetComponent<PlayerHealth>().GetHealth());
        collision.gameObject.GetComponent<PlayerHealth>().Damage(damage);
        collectibleController.DisableCollectible();
        App.audioManager.Play("HotCollectible");
    }
}
