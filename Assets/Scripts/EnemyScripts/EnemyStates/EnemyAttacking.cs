using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : EnemyState
{
    private float chargeUpTime;
    private float timeSinceStart;

    public override void FixedUpdateEnemy(Vector2 playerPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateEnemy(Vector2 playerPosition)
    {
        timeSinceStart += Time.deltaTime;
    }

    IEnumerator Attack(){
        while(timeSinceStart < chargeUpTime){
            yield return null;
        }
    }
}