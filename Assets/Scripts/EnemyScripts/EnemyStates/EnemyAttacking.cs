using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyAttacking : TrackingEnemyState
{
    private float chargeUpTime = .5f;
    private float attackCooldown = 1f;
    private Vector2 attackBox = new Vector2(1,1);
    private float attackDistance = .1f;


    public override void FixedUpdateEnemy(Vector2 playerPosition)
    {
        return;
    }

    public override void OnStateChange()
    {
        enemy.HandleCoroutine(Attack(enemy.lastKnownPlayerPosition));
    }

    public override void UpdateEnemy(Vector2 playerPosition)
    {
        if(!HasLineOfSight(playerPosition)){
            TrackPath(playerPosition);
        }
    }

    IEnumerator Attack(Vector2 playerPosition){
        //Waits until attack has charged
        yield return new WaitForSeconds(chargeUpTime);
        Debug.Log("Attack");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(enemy.rb.position, attackBox, Vector2.Angle(Vector2.right, playerPosition - enemy.rb.position), (playerPosition - enemy.rb.position).normalized, attackDistance, enemy.playerMask);
        foreach(RaycastHit2D hit in hits){
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
                enemy.EnemyDamagedPlayer(enemy.enemyInfo.attackDamage);
                break;
            }
        }
        yield return new WaitForSeconds(attackCooldown);
        enemy.SwitchState(new EnemyChase());
    }

    
}