using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : TrackingEnemyState
{
    private float chargeUpTime = .5f;

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
        enemy.SwitchState(new EnemyChase());
    }

    
}