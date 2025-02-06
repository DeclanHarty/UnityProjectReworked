using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Vector2 lastKnownPlayerPosition;
    public bool playerVisible;

    public bool playerEscaped = false;

    public List<Vector2> lastSeenPositions = new List<Vector2>();

    public float maxPsycheDistance;


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
        RaycastHit2D enemyLosCheck = Physics2D.Raycast(rb.position, (playerPosition - rb.position).normalized, visionDistance, playerMask);
        switch(enemyState){
            case EnemyState.WANDER:
                if(enemyLosCheck && enemyLosCheck.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
                    lastKnownPlayerPosition = playerPosition;
                    enemyState = EnemyState.CHASE;
                }  
            break;
            case EnemyState.CHASE:
                if(enemyLosCheck && enemyLosCheck.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
                    lastKnownPlayerPosition = playerPosition;
                    lastSeenPositions.Clear();
                    rb.MovePosition(Vector2.MoveTowards(rb.position, playerPosition, enemyInfo.speed * Time.deltaTime));
                }else{
                    //Check if enemy has just lost LOS
                    if(lastSeenPositions.Count == 0){
                        lastSeenPositions.Add(lastKnownPlayerPosition);
                    }

                    RaycastHit2D lastSeenPositionCheck = Physics2D.Raycast(lastSeenPositions.Last(), (playerPosition - lastSeenPositions.Last()).normalized, visionDistance, playerMask);
                    if(lastSeenPositionCheck && lastSeenPositionCheck.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
                        lastKnownPlayerPosition = playerPosition;
                    }else if(!playerEscaped){
                        lastSeenPositions.Add(lastKnownPlayerPosition);
                        if(Vector2.Distance(lastSeenPositions[0], playerPosition) > maxPsycheDistance){
                            playerEscaped = true;
                        }
                    }


                    rb.MovePosition(Vector2.MoveTowards(rb.position, lastSeenPositions[0], enemyInfo.speed * Time.deltaTime));
                    if(Vector2.Distance(rb.position, lastSeenPositions[0]) < movementPointTolerence){
                        lastSeenPositions.RemoveAt(0);
                        if(lastSeenPositions.Count == 0){
                            enemyState = EnemyState.WANDER;
                            playerEscaped = false;
                        }
                    }

                }

            break;
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

    void OnDrawGizmos(){
        foreach(Vector2 pos in lastSeenPositions){
            Gizmos.DrawSphere(pos, .1f);
        }
    }
}
