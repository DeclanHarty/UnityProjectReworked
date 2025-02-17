using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : GameState
{
    public override void OnStateEnter()
    {
        gameManager.enemyManager.StopAllCoroutines();
        gameManager.playerManager.ClearAllPlayerVelocity();
        gameManager.uIManager.ActivateGameOverScreen();
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void StateFixedUpdate()
    {
        return;
    }

    public override void StateUpdate()
    {
        return;
    }
}