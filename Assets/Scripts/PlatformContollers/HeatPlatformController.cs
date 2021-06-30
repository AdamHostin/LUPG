using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatPlatformController : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float damageFrequency;
    [SerializeField] int maxCountOfUses;
    [SerializeField] float timeToRespawn;
    [Header("Don't touch")]
    [SerializeField] List<PlayerHealth> playersHp = new List<PlayerHealth>();
    [SerializeField] PlatformRespawnController respawnController;
    [SerializeField] int countOfUses;

    private void Start()
    {
        ResetCountOfUses();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (playersHp.Count == 0) StartCoroutine(DamagePlayers());
            playersHp.Add(collision.GetComponent<PlayerHealth>());
            App.audioManager.PlayLoop("HotZone");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersHp.Remove(collision.GetComponent<PlayerHealth>());
            if (playersHp.Count == 0) StopCoroutine(DamagePlayers());
            App.audioManager.Stop("HotZone");
        }
    }

    IEnumerator DamagePlayers()
    {
        while ((countOfUses >= 0))
        {
            foreach (var player in playersHp)
            {
                countOfUses--;
                player.Damage(damage);
            }
            
            yield return new WaitForSeconds(damageFrequency);
        }
        StartCoroutine(respawnController.Fade(timeToRespawn));
        
    }

    public void ResetCountOfUses()
    {
        countOfUses = maxCountOfUses;
    }
}
