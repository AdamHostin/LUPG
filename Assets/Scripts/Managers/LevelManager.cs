using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        App.playerManager.SpawnPlayers(GetComponentInChildren<SpawnPoints>());
        //TODO odpocitavnie
        App.playerManager.StartDealingDamage();
    }
}