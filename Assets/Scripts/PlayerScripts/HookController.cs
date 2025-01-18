using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public GrapplingHookBehavior grapplingHookBehavior;
    private float timeMouse1Held;

    public bool hookIsOut;

    public void UpdateHook(){
        grapplingHookBehavior.UpdateHook();
        
    }

    public void Hook(Vector2 mouseScreenPos, Vector2 playerPos, bool mouse1Pressed, bool mouse2Pressed){
        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = (mouseWorldPoint - playerPos).normalized;

        if(mouse1Pressed && !hookIsOut){
            grapplingHookBehavior.ActivateAndDirect(direction, 1);
            hookIsOut = true;
        } 

        if(mouse2Pressed && hookIsOut){
            ResetHook();
        }

    }

    public void ResetHook(){
        hookIsOut = false;
        grapplingHookBehavior.ResetHook();
    }

    public bool HookIsStuck(){
        return grapplingHookBehavior.isStuck;
    }

    public Vector2 GetHookPosition(){
        return grapplingHookBehavior.GetHookPosition();
    }
}



