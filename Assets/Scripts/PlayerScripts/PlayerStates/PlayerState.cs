using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PlayerState
{   
    public PlayerManager playerManager;
    public void InjectPlayerManager(PlayerManager playerManager){
        this.playerManager = playerManager;
    }

    public Vector2 GetMouseDirection(){
        Vector2 mousePos = playerManager.inputManager.CollectMousePos();
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 playerMouseDifference = mousePosInWorld - playerManager.movement.GetPlayerPosition();
        Vector2 mouseDirection = playerMouseDifference.normalized;

        return mouseDirection;
    }
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
    public abstract void OnStateChange();
}
