using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    public int currentHealth;

    public GameObject chaser;

    public TilemapManager tilemapManager;
    public EnemyManager enemyManager;

    public List<Vector2Int> movementPoints;

    public Rigidbody2D rb;

    public float movementPointTolerence;

    public float timeSinceLastPathfind;
    public float timeBetweenPathfind;

    public void Awake(){
        currentHealth = enemyInfo.maxHealth;
        // GetMovementPoints();
    }
    public void TakeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Die();
        }
    }

    [ContextMenu("GetMovementPointsToTarget")]
    public void GetMovementPoints(Vector2 goalPosition){
        movementPoints = enemyManager.GetAStarPoints(transform.position, goalPosition);
    }

    public void StateUpdate(Vector2 playerPosition){
        timeSinceLastPathfind += Time.deltaTime;
        if(timeSinceLastPathfind > timeBetweenPathfind){
            timeSinceLastPathfind = 0;
            GetMovementPoints(playerPosition);
        }
    }

    public void StateFixedUpdate(Vector2 playerPosition){
        rb.MovePosition(Vector2.MoveTowards(transform.position, movementPoints[0], enemyInfo.speed * Time.deltaTime));
        if(movementPoints.Count > 0 && Vector2.Distance(rb.position, movementPoints[0]) < movementPointTolerence){
            movementPoints.RemoveAt(0);
        }
    }

    public void InjectEnemyManager(EnemyManager enemyManager){
        this.enemyManager = enemyManager;
    }

    public Vector2 GetPosition(){
        return rb.position;
    }

    public void DeleteEnemy(){
        Destroy(gameObject);
    }

    public void Die(){
        enemyManager.NotifyEnemyDeath(this);
        Destroy(gameObject);
    }
}
