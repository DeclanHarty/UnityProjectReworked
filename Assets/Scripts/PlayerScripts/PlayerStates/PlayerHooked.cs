using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHooked : PlayerState
{
    public override void OnStateChange()
    {
        playerManager.movement.ClearVelocity();
    }

    public override void StateFixedUpdate()
    {
        playerManager.hookController.MoveTowardsHook();
    }

    public override void StateUpdate()
    {
        if(playerManager.inputManager.IsMouse2Down()){
            playerManager.hookController.ReleaseHook();
            playerManager.SwitchState(new FreeMovement());
            return;
        }

        if(playerManager.inputManager.IsSpacebarPressed()){
            playerManager.hookController.ReleaseHook();
            playerManager.SwitchState(new FreeMovement());
            playerManager.movement.Jump(true);
            return;
        }
    }
}
