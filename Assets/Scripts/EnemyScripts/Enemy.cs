using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    public int currentHealth;

    public void Awake(){
        currentHealth = enemyInfo.maxHealth;
    }
    public void TakeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Destroy(gameObject);
        }
    }
}
