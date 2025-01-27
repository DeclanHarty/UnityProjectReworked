using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Vector2 movementKeyInput;
    // bool spacebarPressed;
    // bool spacebarHeld;
    // Vector2 mousePosToScreen;

    public InputManager inputManager;
    public Movement movement;
    public HookController hookController;
    public PlayerObjectController playerObjectController;

    // Change this to a proper State and Strategy System
    public PlayerState playerState;

    public void UpdatePlayer(){
        playerState.StateUpdate();
        
    }

    public void FixedUpdatePlayer(){
        playerState.StateFixedUpdate();
    }

    public void InjectInputManager(InputManager inputManager){
        this.inputManager = inputManager;
    }

    public Vector2 GetPlayerPosition(){
        return movement.GetPlayerPosition();
    }


    public void SwitchState(PlayerState newState){
        this.playerState = newState;
        this.playerState.InjectPlayerManager(this);
        this.playerState.OnStateChange();
    }
}
