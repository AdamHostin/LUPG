using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float timeBetweenDamage;
    [SerializeField] int damageAmount;

    

    Queue<string> playerOrder = new Queue<string>();

    List<PlayerHealth> clearPlayers = new List<PlayerHealth>();
    public List<PlayerHealth> players = new List<PlayerHealth>();

    int playerCount = 0;

    private void Awake()
    {
        App.playerManager = this;
    }

    public void StartDealingDamage()
    {
        StartCoroutine(DamagePlayers());
    }   

    public void AddPlayer(PlayerHealth player)
    {
        players.Add(player);
    }

    public void EnqueuePlayer(PlayerHealth player, string name)
    {
        playerOrder.Enqueue(name);
        clearPlayers.Add(player);
    }

    public void HealOthers(PlayerHealth initiator, int healAmount)
    {
        foreach (PlayerHealth player in players)
        {
            if (player == initiator) continue;
            player.Heal(healAmount);
        }
    }

    void ClearDeadPlayers()
    {
        foreach (PlayerHealth playerToDelete in clearPlayers)
        {
            players.Remove(playerToDelete);
        }
        clearPlayers.Clear();
    }

    IEnumerator DamagePlayers()
    {
        while (players.Count != 1)
        {
            foreach (PlayerHealth player in players)
            {
                player.Damage(damageAmount);                
            }

            ClearDeadPlayers();
            yield return new WaitForSeconds(timeBetweenDamage);
        }

        playerOrder.Enqueue(players[0].playerName);
        //Debug
        PrintOrder();

        //End Level/ call win screen / send queue
    }

    // Debug function
    public void PrintOrder()
    {
        while (playerOrder.Count != 0)
        {
            Debug.Log(playerOrder.Dequeue());
        }

    }

    public int GetPlayerIndex()
    {
        return playerCount++;
    }

    public int GetPlayerCount()
    {
        return playerCount;
    }

    public PlayerHealth GetPlayerByIndex(int index)
    {
        return players[index];
    }
}
