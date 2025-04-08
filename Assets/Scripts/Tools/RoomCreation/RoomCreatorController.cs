using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomCreatorController : MonoBehaviour
{
    private List<RoomVertex> roomVertices;
    private bool isClosed;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject roomVertexPrefab;

    [SerializeField]
    private Vector2 defaultStartPosition;

    private RoomVertex selectedVertex;
    private bool justSelected;

    void Start()
    {
        roomVertices = new List<RoomVertex>();
        RoomVertex roomVertex = CreateVertex(0, defaultStartPosition);
        roomVertices.Add(roomVertex);

        lineRenderer.positionCount = roomVertices.Count;
        lineRenderer.SetPositions(new Vector3[]{defaultStartPosition});
    }

    public void RoomVertexSelected(RoomVertex roomVertex){
        selectedVertex?.Unselect();
        selectedVertex = roomVertex;
        justSelected = true;
    }


    public void AppendVertex(Vector2 pos){
        int index = roomVertices.Count;
        RoomVertex roomVertex = CreateVertex(index, pos);
        roomVertices.Add(roomVertex);

        lineRenderer.positionCount = index + 1;
        lineRenderer.SetPosition(index, pos);

        if(isClosed){
            lineRenderer.positionCount = roomVertices.Count + 1;
            lineRenderer.SetPosition(roomVertices.Count, roomVertices[0].GetPosition());
        }
    }

    public void InsertVertex(int insertionIndex, Vector2 pos){
        if(insertionIndex < 0 || insertionIndex > roomVertices.Count){
            return;
        }

        RoomVertex roomVertex = CreateVertex(insertionIndex, pos);
        roomVertices.Insert(insertionIndex, roomVertex);

        RoomVertexSelected(roomVertex);
        roomVertex.Select();

        lineRenderer.positionCount = roomVertices.Count;
        lineRenderer.SetPosition(insertionIndex, pos);

        for(int i = insertionIndex + 1; i < roomVertices.Count; i++){
            roomVertices[i].SetIndex(i);
            lineRenderer.SetPosition(i, roomVertices[i].GetPosition());
        }

        if(isClosed){
            lineRenderer.positionCount = roomVertices.Count + 1;
            lineRenderer.SetPosition(roomVertices.Count, roomVertices[0].GetPosition());
        }
    }

    public RoomVertex CreateVertex(int index, Vector2 pos){
        RoomVertex roomVertex = Instantiate(roomVertexPrefab, pos, Quaternion.identity).GetComponent<RoomVertex>();
        roomVertex.SetIndex(index);
        roomVertex.SetRoomController(this);

        return roomVertex;
    }

    public void VertexMoved(int index, Vector2 pos){
        lineRenderer.SetPosition(index, pos);
        if(isClosed && index == 0){
            lineRenderer.SetPosition(roomVertices.Count, pos);
        }
    }

    public void CloseLoop(){
        isClosed = true;
        lineRenderer.positionCount = roomVertices.Count + 1;
        lineRenderer.SetPosition(roomVertices.Count, roomVertices[0].GetPosition());
    }

    public void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        if(Input.GetKeyDown(KeyCode.A)){
            AppendVertex(mousePos);
        }

        if(Input.GetKeyDown(KeyCode.I) && selectedVertex != null){
            InsertVertex(selectedVertex.GetIndex() + 1, mousePos);
        }

        if(Input.GetKeyDown(KeyCode.C) && !isClosed){
            CloseLoop();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !justSelected){
            selectedVertex?.Unselect();
            selectedVertex = null;
        }
        justSelected = false;
    }


}
