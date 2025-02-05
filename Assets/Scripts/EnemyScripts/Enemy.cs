using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    public int currentHealth;
    public EnemyManager enemyManager;

    public List<Vector2> movementPoints;

    public Rigidbody2D rb;

    public float movementPointTolerence;

    public float timeSinceLastPathfind;
    public float timeBetweenPathfind;

    public EnemyState enemyState;
    public float visionDistance;
    public LayerMask playerMask;
    public Vector2 lastSeenPlayerPosition;
    public bool playerVisible;


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
        movementPoints = enemyManager.GetMovementPoints(transform.position, goalPosition);
    }

    public void StateUpdate(Vector2 playerPosition){
        //timeSinceLastPathfind += Time.deltaTime;

    }

    public void StateFixedUpdate(Vector2 playerPosition){
        RaycastHit2D hit;
        switch(enemyState){
            case EnemyState.WANDER:
                hit = Physics2D.Raycast(rb.position, (playerPosition - rb.position).normalized, visionDistance, playerMask);
                if(hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
                    lastSeenPlayerPosition = hit.point;
                    playerVisible = true;
                    enemyState = EnemyState.CHASE;
                }
            break;
            case EnemyState.CHASE:
                hit = Physics2D.Raycast(rb.position, (playerPosition - rb.position).normalized, visionDistance, playerMask);
                if(hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
                    lastSeenPlayerPosition = hit.point;
                    playerVisible = true;
                    rb.MovePosition(Vector2.MoveTowards(rb.position, playerPosition, enemyInfo.speed * Time.deltaTime));
                }else{
                    playerVisible = false;
                    rb.MovePosition(Vector2.MoveTowards(rb.position, lastSeenPlayerPosition, enemyInfo.speed * Time.deltaTime));
                    if(Vector2.Distance(lastSeenPlayerPosition, rb.position) < movementPointTolerence){
                        enemyState = EnemyState.WANDER;  
                        playerVisible = false;
                        movementPoints.Clear();
                        break;
                    }
                }
                // if(movementPoints.Count > 0){
                //     rb.MovePosition(Vector2.MoveTowards(transform.position, movementPoints[0], enemyInfo.speed * Time.deltaTime));
                //     if(Vector2.Distance(rb.position, movementPoints[0]) < movementPointTolerence){
                //         movementPoints.RemoveAt(0);
                //     }
                // }
            break;
        }
        if(movementPoints.Count > 0){
            rb.MovePosition(Vector2.MoveTowards(transform.position, movementPoints[0], enemyInfo.speed * Time.deltaTime));
            if(Vector2.Distance(rb.position, movementPoints[0]) < movementPointTolerence){
                movementPoints.RemoveAt(0);
            }  
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

    public enum EnemyState{
        WANDER,
        CHASE,
        SEARCH
    } 
}
