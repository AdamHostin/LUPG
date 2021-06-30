using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : MonoBehaviour
{

    [SerializeField] int healAmount;
    [SerializeField] float healFrequency;
    List<PlayerHealth> playersInRange = new List<PlayerHealth>();

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInRange.Add(collision.GetComponent<PlayerHealth>());
            if (playersInRange.Count == 1) StartCoroutine(Heal());
            App.audioManager.PlayLoop("WetZone");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInRange.Remove(collision.GetComponent<PlayerHealth>());
            if (playersInRange.Count == 0) StopCoroutine(Heal());
            App.audioManager.Stop("WetZone");
        }
    }

    IEnumerator Heal()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(healFrequency);
            foreach (var player in playersInRange) player.Heal(healAmount);
        }
        
    }
}
