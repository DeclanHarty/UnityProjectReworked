using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    public GameManager gameManager;
    public abstract void StateUpdate();

    public abstract void StateFixedUpdate();

    public abstract void OnStateChange();

    public void InjectGameManager(GameManager gameManager){
        this.gameManager = gameManager;
    }
}
