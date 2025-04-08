using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    public int currentHealth;
    public EnemyManager enemyManager;

    public Rigidbody2D rb;

    public GameObject enemySprite;

    public float movementPointTolerence;

    public EnemyState enemyState;
    public float visionDistance;
    public LayerMask playerMask;
    public LayerMask groundMask;

    [Header("Pathing Variables")]
    public Vector2 lastKnownPlayerPosition;
    public bool playerEscaped = false;
    public List<Vector2> lastSeenPositions = new List<Vector2>();
    public float maxPsycheDistance;

    [Header("Physics Variables")]
    public Vector2 currentVelocity;
    public float maxTargetDisplacement;


    public void Awake(){
        currentHealth = enemyInfo.maxHealth;
        enemyState = new EnemyWander();
        enemyState.InjectEnemy(this);
    }
    public void TakeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Die();
        }
    }

    public void StateUpdate(Vector2 playerPosition){
        enemyState.UpdateEnemy(playerPosition);

    }

    public void ClearAllVelocity(){
        rb.velocity = Vector2.zero;
        currentVelocity = Vector2.zero;
    }

    public void StopRBVelocity(){
        rb.velocity = Vector2.zero;
    }

    public void StateFixedUpdate(Vector2 playerPosition){
        enemyState.FixedUpdateEnemy(playerPosition);
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

    public void EnemyDamagedPlayer(int damage){
        enemyManager.EnemyDamagedPlayer(damage);
    }

    public void SwitchState(EnemyState enemyState){
        this.enemyState = enemyState;
        this.enemyState.InjectEnemy(this);
        this.enemyState.OnStateChange();
    }

    public void HandleCoroutine(IEnumerator coroutine){
        StartCoroutine(coroutine);
    }

    public Vector2 GetEnemyDirection(){
        if(currentVelocity.normalized != Vector2.zero){
            return currentVelocity.normalized;
        }else{
            return Vector2.right;
        }
    }
}
