using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVertex : Draggable
{
    private RoomCreatorController roomCreatorController;

    [SerializeField]
    private GameObject sprite;
    private int index;

    public bool isSelected;
    // Start is called before the first frame update

    void OnMouseDown()
    {
        roomCreatorController.RoomVertexSelected(this);
        isSelected = true;
        sprite.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public override void WhenMouseDragged()
    {
        roomCreatorController.VertexMoved(index, GetPosition());
    }

    public Vector2 GetPosition(){
        return transform.position;
    }

    public void SetIndex(int index){
        this.index = index;
    }

    public int GetIndex(){
        return index;
    }

    public void SetRoomController(RoomCreatorController controller){
        roomCreatorController = controller;
    }

    public void Unselect(){
        isSelected = false;
        sprite.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Select(){
        isSelected = true;
        sprite.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
