using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public string playerName;

    [SerializeField] int maxHealth;
    

    [Header("Debug don't touch")]
    [SerializeField] int health;

    private void Start()
    {
        App.playerManager.AddPlayer(this);
        health = maxHealth;
    }


    public void Heal(int addHealth)
    {
        health += addHealth;
        Mathf.Clamp(health, -maxHealth, maxHealth);
    }

    public void Damage(int subHealth)
    {
        health -= subHealth;
        if (health <= 0)
        {
            App.playerManager.EnqueuePlayer(this, playerName);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int value)
    {
        health = value;
    }
}

