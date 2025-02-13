using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 CollectMovementKeyInput(){
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public Vector2 CollectMousePos(){
        return Input.mousePosition;
    }

    public bool IsMouse1Down(){
        return Input.GetButtonDown("Fire1");
    }

    public bool IsMouse2Down(){
        return Input.GetButtonDown("Fire2");
    }

    public bool EPressed(){
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool IsSpacebarHeld(){
        return Input.GetKey(KeyCode.Space);
    }

    public bool IsSpacebarPressed(){
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool LPressed(){
        return Input.GetKeyDown(KeyCode.L);
    }

    public bool EscPressed(){
        return Input.GetKeyDown(KeyCode.Escape);
    }
}
