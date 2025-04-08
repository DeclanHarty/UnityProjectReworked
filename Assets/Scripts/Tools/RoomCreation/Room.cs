using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : ScriptableObject
{
    private Vector2[] roomVertices;

    private Vector2[] tunnelPoints;

    public Room(Vector2[] roomVertices, Vector2[] tunnelPoints){
        this.roomVertices = roomVertices;
        this.tunnelPoints = tunnelPoints;
    }


    

}
