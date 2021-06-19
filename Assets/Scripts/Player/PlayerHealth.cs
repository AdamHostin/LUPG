using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public string playerName;

    [SerializeField] int maxHealth;
    

    [Header("Debug don't touch")]
    [SerializeField] int health;
    [SerializeField] BarController hpBar;
    [SerializeField] Image image;


    private void Start()
    {
        App.playerManager.AddPlayer(this);
        health = maxHealth;
    }


    public void Heal(int addHealth)
    {
        health += addHealth;
        Mathf.Clamp(health, -maxHealth, maxHealth);
        hpBar.OnUIUpdate((float)health / (float)maxHealth);
    }

    public void Damage(int subHealth)
    {
        health -= subHealth;

        if (health <= 0)
        {
            App.playerManager.EnqueuePlayer(this, playerName);
        }
        hpBar?.OnUIUpdate((float)health / (float)maxHealth);
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    public void SetHpBar(BarController bar)
    {
        hpBar = bar;
        hpBar.OnUIUpdate((float)health/ (float)maxHealth);
        hpBar.SetImage(image);
    }

    public void SetImage(Image image)
    {
        this.image = image;
    }
}

