using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerManager playerManager;
    public int maxHealth;
    public int currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public float TakeDamage(int damage){
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if(currentHealth <= 0){
            return 0;
        }

        return (float)currentHealth / (float)maxHealth;
    }

    public void Heal(int healthPoints){
        currentHealth += healthPoints;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void NotifyPlayerDied(){
        playerManager.PlayerDied();
    }
}
