using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : PlayerState
{
    public override void OnStateChange()
    {
        
    }
  
    public override void StateFixedUpdate()
    {
        playerManager.movement.Move(playerManager.inputManager.CollectMovementKeyInput(), playerManager.inputManager.IsSpacebarHeld());
    }

    public override void StateUpdate()
    {
        Vector2 mouseDirection = GetMouseDirection();
        Vector2 mousePosInWorld = playerManager.inputManager.CollectMousePos();

        if(playerManager.inputManager.IsSpacebarPressed()) playerManager.movement.Jump();

        if(playerManager.movement.IsSwitchingDirection()){
            int currentDirection = playerManager.movement.GetPlayerDirection();
            if(currentDirection == 1) playerManager.playerObjectController.SetDirectionRight();
            else playerManager.playerObjectController.SetDirectionLeft();
            playerManager.movement.SwitchedDirection();
        }

        if(playerManager.inputManager.EPressed()){
            

            if(playerManager.hookController.ThrowOutHook(mouseDirection)) playerManager.SwitchState(new PlayerHooked());
        }

        if(playerManager.inputManager.IsMouse1Down()) playerManager.heldItemController.OnFire1(mousePosInWorld, mouseDirection);
    }
}
