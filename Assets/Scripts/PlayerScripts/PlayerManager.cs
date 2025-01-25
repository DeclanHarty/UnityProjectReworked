using System.Collections;
using System.Collections.Generic;
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

    public void UpdatePlayer(){
        if(inputManager.IsSpacebarPressed()) movement.Jump();
    }

    public void FixedUpdatePlayer(){
        movement.Move(inputManager.CollectMovementKeyInput(), inputManager.IsSpacebarHeld());
    }
    

    public void InjectInputManager(InputManager inputManager){
        this.inputManager = inputManager;
    }

    public Vector2 GetPlayerPosition(){
        return movement.GetPlayerPosition();
    }
}
