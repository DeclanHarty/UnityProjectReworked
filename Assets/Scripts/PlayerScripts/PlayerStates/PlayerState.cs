using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{   
    public PlayerManager playerManager;
    public void InjectPlayerManager(PlayerManager playerManager){
        this.playerManager = playerManager;
    }
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
    public abstract void OnStateChange();
}
