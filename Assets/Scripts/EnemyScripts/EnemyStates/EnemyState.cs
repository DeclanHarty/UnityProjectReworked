using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    public Enemy enemy;

    public void InjectEnemy(Enemy enemy){
        this.enemy = enemy;
    }
    public abstract void UpdateEnemy(Vector2 playerPosition);

    public abstract void FixedUpdateEnemy(Vector2 playerPosition);
}
