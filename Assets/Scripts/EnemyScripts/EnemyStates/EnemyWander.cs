using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : EnemyState
{
    public override void FixedUpdateEnemy(Vector2 playerPosition)
    {
        RaycastHit2D enemyLosCheck = Physics2D.Raycast(enemy.rb.position, (playerPosition - enemy.rb.position).normalized, enemy.visionDistance, enemy.playerMask);

        if(enemyLosCheck && enemyLosCheck.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
            enemy.lastKnownPlayerPosition = playerPosition;
            enemy.SwitchState(new EnemyChase());
        }  
    }

    public override void OnStateChange()
    {
        enemy.ClearAllVelocity();
    }

    public override void UpdateEnemy(Vector2 playerPosition)
    {
        return;
    }
}