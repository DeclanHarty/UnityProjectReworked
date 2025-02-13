using System;
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

    [NonSerialized]
    public InputManager inputManager;
    [NonSerialized]
    public UIManager uiManager;

    public Movement movement;
    public HookController hookController;
    public PlayerObjectController playerObjectController;
    public HeldItemController heldItemController;
    public PlayerHealth playerHealth;

    // Change this to a proper State and Strategy System
    public PlayerState playerState;

    void Awake()
    {
        heldItemController.SetItem(new Gun());
    }

    public void UpdatePlayer(){
        playerState.StateUpdate();
        heldItemController.UpdateItem();
        
    }

    public void FixedUpdatePlayer(){
        playerState.StateFixedUpdate();
    }


    public Vector2 GetPlayerPosition(){
        return movement.GetPlayerPosition();
    }


    public void SwitchState(PlayerState newState){
        this.playerState = newState;
        this.playerState.InjectPlayerManager(this);
        this.playerState.OnStateChange();
    }

    public void SetPlayerPosition(Vector2 position){
        movement.SetPlayerPosition(position);
    }

    public void PlayerDied(){
        return;
    }

    public bool TakeDamage(int damage){
        float healthPercentage = playerHealth.TakeDamage(damage);
        uiManager.UpdatePlayerHealthBar(healthPercentage);

        if(healthPercentage <= 0){
            return true;
        }else{
            return false;
        }
    }

    public void ClearAllPlayerVelocity(){
        movement.ClearAllVelocity();
    }

    public void ClearRBVelocity(){
        movement.rb.velocity = Vector2.zero;
    }

    public void InjectInputManager(InputManager inputManager){
        this.inputManager = inputManager;
    }

    public void InjectUIManager(UIManager uiManager){
        this.uiManager = uiManager;
    }
}
