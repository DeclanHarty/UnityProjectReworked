using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyChase : TrackingEnemyState
{
    private float attackRange = .5f;

    private float STEER_CHECK_DISTANCE = 1f;
    private float FORCE_COEFFECIENT = 2;

    public override void FixedUpdateEnemy(Vector2 playerPosition)
    {
        // // Checks to see if enemy has line of sight with player
        // RaycastHit2D enemyLosCheck = Physics2D.Raycast(enemy.rb.position, (playerPosition - enemy.rb.position).normalized, enemy.visionDistance, enemy.playerMask); 

        // // If enemy does have LOS then the enemy moves directly to the player and clears last seen positions
        // if(enemyLosCheck && enemyLosCheck.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
        //     enemy.lastKnownPlayerPosition = playerPosition;
        //     enemy.lastSeenPositions.Clear();
        //     enemy.rb.MovePosition(Vector2.MoveTowards(enemy.rb.position, playerPosition, enemy.enemyInfo.speed * Time.deltaTime));
        bool enemyHasLineOfSight = HasLineOfSight(playerPosition);
        if(enemyHasLineOfSight){
            Vector2 target = ((playerPosition + GetRandomDisplacement() - enemy.rb.position ).normalized + GetSteeringForce()).normalized;
            Debug.Log(GetSteeringForce());
            enemy.currentVelocity = Vector2.MoveTowards(enemy.currentVelocity, target * enemy.enemyInfo.speed, enemy.enemyInfo.acceleration * Time.deltaTime);
            enemy.rb.velocity = enemy.currentVelocity;
            
            //enemy.rb.MovePosition(Vector2.MoveTowards(enemy.rb.position, playerPosition, enemy.enemyInfo.speed * Time.deltaTime));
            if(Vector2.Distance(enemy.rb.position, playerPosition) < attackRange){
                enemy.SwitchState(new EnemyAttacking());
            }
        }else{
            TrackPath(playerPosition);
            
            Vector2 target = ((enemy.lastSeenPositions[0]+ GetRandomDisplacement() - enemy.rb.position).normalized + GetSteeringForce()).normalized;
            enemy.currentVelocity = Vector2.MoveTowards(enemy.currentVelocity, target * enemy.enemyInfo.speed, enemy.enemyInfo.acceleration * Time.deltaTime);
            enemy.rb.velocity = enemy.currentVelocity;

            //enemy.rb.MovePosition(Vector2.MoveTowards(enemy.rb.position, enemy.lastSeenPositions[0], enemy.enemyInfo.speed * Time.deltaTime));
            if(Vector2.Distance(enemy.rb.position, enemy.lastSeenPositions[0]) < enemy.movementPointTolerence){
                enemy.lastSeenPositions.RemoveAt(0);
                if(enemy.lastSeenPositions.Count == 0){
                    enemy.SwitchState(new EnemyWander());
                    enemy.playerEscaped = false;
                }
            }

        }
    }

    public override void OnStateChange()
    {
        return;
    }

    public override void UpdateEnemy(Vector2 playerPosition)
    {
        return;
    }

    public Vector2 GetSteeringForce(){
        Vector2 steeringForce;

        Quaternion leftSteerCheckRotation = Quaternion.AngleAxis(45, Vector3.forward);
        
        // Perform 2 seperate Raycasts that check for the ground layer
        Vector2 leftSteerCheckDirection = leftSteerCheckRotation * enemy.GetEnemyDirection();

        Vector2 leftSteerCheckForce = Vector2.zero;
        RaycastHit2D leftSteeringCheckHit = Physics2D.Raycast(enemy.rb.position, leftSteerCheckDirection, STEER_CHECK_DISTANCE, enemy.groundMask);
        if(leftSteeringCheckHit){
            float leftSteerCheckDepth = STEER_CHECK_DISTANCE - leftSteeringCheckHit.distance;
            leftSteerCheckForce = Quaternion.AngleAxis(-90, Vector3.forward) * leftSteerCheckDirection * leftSteerCheckDepth * FORCE_COEFFECIENT;
        }

        Quaternion rightSteerCheckRotation = Quaternion.AngleAxis(-45, Vector3.forward);
        Vector2 rightSteerCheckDirection = rightSteerCheckRotation * enemy.GetEnemyDirection();

        Vector2 rightSteerCheckForce = Vector2.zero;
        RaycastHit2D rightSteeringCheckHit = Physics2D.Raycast(enemy.rb.position, rightSteerCheckDirection, STEER_CHECK_DISTANCE, enemy.groundMask);
        if(rightSteeringCheckHit){
            float rightSteerCheckDepth = STEER_CHECK_DISTANCE - rightSteeringCheckHit.distance;
            rightSteerCheckForce = Quaternion.AngleAxis(90, Vector3.forward) * leftSteerCheckDirection * rightSteerCheckDepth * FORCE_COEFFECIENT;
        }

        steeringForce = leftSteerCheckForce + rightSteerCheckForce;

        //Debug Lines
        Debug.DrawLine(enemy.rb.position, leftSteerCheckDirection * STEER_CHECK_DISTANCE + enemy.rb.position, Color.red);
        Debug.DrawLine(enemy.rb.position, rightSteerCheckDirection * STEER_CHECK_DISTANCE + enemy.rb.position, Color.red);
        

        return steeringForce;
    }

    public Vector2 GetRandomDisplacement(){
        float displacementRadius = Random.Range(0, enemy.maxTargetDisplacement);
        float angle = Random.Range(0f, 360f);

        Vector2 randomPositionDeviation = Quaternion.AngleAxis(angle, Vector3.forward) * new Vector2(displacementRadius, 0);

        return randomPositionDeviation;
    }
}