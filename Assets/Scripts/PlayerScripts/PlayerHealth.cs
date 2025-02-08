using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerManager playerManager;
    public int maxHealth;
    public int currentHealth;

    public void TakeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            NotifyPlayerDied();
        }
    }

    public void Health(int healthPoints){
        currentHealth += healthPoints;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void NotifyPlayerDied(){
        playerManager.PlayerDied();
    }
}
