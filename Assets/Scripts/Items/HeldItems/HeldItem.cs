using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeldItem
{
    public HeldItemController heldItemController;
    public GameObject sprite;
    public Vector2 mousePos;
    public Vector2 mouseDirection;
    public Vector2 currentPosition;

    public abstract void Fire1Action();
    public abstract void Fire2Action();

    public void SetMouseDirection(Vector2 mouseDirection){
        this.mouseDirection = mouseDirection;
    }

    public void SetMousePosition(Vector2 mousePos){
        this.mousePos = mousePos; 
    }

    public void SetCurrentPosition(Vector2 currentPos){
        this.currentPosition = currentPos;
    }

    public void InjectHeldItemController(HeldItemController heldItemController){
        this.heldItemController = heldItemController;
    }

    public abstract void Update();
}
