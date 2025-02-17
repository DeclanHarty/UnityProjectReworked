using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerHooked : PlayerState
{
    public override void OnStateChange()
    {
        playerManager.movement.ClearAllVelocity();
    }

    public override void StateFixedUpdate()
    {
        playerManager.hookController.MoveTowardsHook();
    }

    public override void StateUpdate()
    {
        Vector2 mouseDirection = GetMouseDirection();
        Vector2 mousePosInWorld = playerManager.inputManager.CollectMousePos();

        if(playerManager.inputManager.EPressed()){
            if(!playerManager.hookController.ThrowOutHook(GetMouseDirection())){
                playerManager.SwitchState(new FreeMovement());
            }
        }

        if(playerManager.inputManager.IsMouse2Down() || playerManager.inputManager.LPressed()){
            playerManager.hookController.ReleaseHook();
            playerManager.SwitchState(new FreeMovement());
            playerManager.movement.SetVelocity(playerManager.hookController.GetVelocity());
            return;
        }

        if(playerManager.inputManager.IsSpacebarPressed()){
            playerManager.hookController.ReleaseHook();
            playerManager.SwitchState(new FreeMovement());
            playerManager.movement.Jump(true);
            return;
        }

        if(playerManager.inputManager.IsMouse1Down()) playerManager.heldItemController.OnFire1(mousePosInWorld, mouseDirection);
    }
}
