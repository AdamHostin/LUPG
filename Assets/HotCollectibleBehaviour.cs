using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotCollectibleBehaviour : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        collision.gameObject.GetComponent<PlayerHealth>().Damage(damage);

    }
}
