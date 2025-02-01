using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    public int currentHealth;

    public GameObject chaser;

    public TilemapManager tilemapManager;

    public List<Vector2Int> movementPoints;

    public Rigidbody2D rb;

    public void Awake(){
        currentHealth = enemyInfo.maxHealth;
        // GetMovementPoints();
    }
    public void TakeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Destroy(gameObject);
        }
    }

    [ContextMenu("GetMovementPointsToTarget")]
    public void GetMovementPoints(){
        //movementPoints = tilemapManager.GetAStarPoints(transform.position, chaser.transform.position);
    }

    public void Update(){
        // rb.MovePosition(Vector2.MoveTowards(transform.position, movementPoints[0], enemyInfo.speed * Time.deltaTime));
        // GetMovementPoints();
    }
}
