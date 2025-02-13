using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paused : GameState
{
    public override void OnStateEnter()
    {
        gameManager.uIManager.ActivatePauseScreen();
        gameManager.playerManager.ClearRBVelocity();
    }

    public override void OnStateExit()
    {
        gameManager.uIManager.DeactivatePauseScreen();
    }

    public override void StateFixedUpdate()
    {
        return;
    }

    public override void StateUpdate()
    {
        if(gameManager.inputManager.EscPressed()){
            gameManager.SwitchState(new Playing());
        }
    }
}