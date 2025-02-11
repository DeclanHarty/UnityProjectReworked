using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyChase : TrackingEnemyState
{
    private float attackRange = .5f;

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
            enemy.rb.MovePosition(Vector2.MoveTowards(enemy.rb.position, playerPosition, enemy.enemyInfo.speed * Time.deltaTime));
            if(Vector2.Distance(enemy.rb.position, playerPosition) < attackRange){
                enemy.SwitchState(new EnemyAttacking());
            }
        }else{
            TrackPath(playerPosition);
            enemy.rb.MovePosition(Vector2.MoveTowards(enemy.rb.position, enemy.lastSeenPositions[0], enemy.enemyInfo.speed * Time.deltaTime));
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
}