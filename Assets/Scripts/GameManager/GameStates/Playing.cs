using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playing : GameState
{
    public override void OnStateChange()
    {
        return;
    }

    public override void StateFixedUpdate()
    {
        gameManager.playerManager.FixedUpdatePlayer();
        gameManager.enemyManager.FixedUpdateManager(gameManager.playerManager.GetPlayerPosition());

        gameManager.enemyManager.UpdateManager(gameManager.playerManager.GetPlayerPosition());
    }

    public override void StateUpdate()
    {
        Vector2 playerPos = gameManager.playerManager.GetPlayerPosition();
        gameManager.playerManager.UpdatePlayer();
        gameManager.cameraController?.MoveCamera(playerPos);

        gameManager.enemyManager.UpdateManager(gameManager.playerManager.GetPlayerPosition());
    }
}