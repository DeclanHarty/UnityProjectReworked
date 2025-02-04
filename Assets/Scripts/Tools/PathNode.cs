using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public class PathNode : FastPriorityQueueNode
{
    public Vector2Int value { get; private set; }

    public PathNode(Vector2Int value){
        this.value = value;
    }
}
