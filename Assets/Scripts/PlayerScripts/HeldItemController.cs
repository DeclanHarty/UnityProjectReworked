using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItemController : MonoBehaviour
{
    public HeldItem heldItem;

    public AmmoIndicator ammoIndicator;

    public void OnFire1(Vector2 mousePos, Vector2 mouseDirection){
        heldItem.SetCurrentPosition(transform.position);
        heldItem.SetMouseDirection(mouseDirection);
        heldItem.Fire1Action();
        
    }

    public void OnFire2(){
        heldItem.Fire2Action();
    }

    public void SetItem(HeldItem item){
        heldItem = item;
        item.InjectHeldItemController(this);
    }

    public void UpdateItem(){
        heldItem.Update();
    }
}
