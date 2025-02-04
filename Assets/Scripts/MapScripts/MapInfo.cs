using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public Vector2Int spawnPosInTilemap;
    public Vector2Int roomCenters;
    public int[,] map;


    public MapInfo(Vector2Int spawnPosInTilemap, int[,] map){
        this.spawnPosInTilemap = spawnPosInTilemap;
        this.map = map;
    }

    public Vector2Int GetSpawnPos(){
        return spawnPosInTilemap;
    }

    public int[,] GetMap(){
        return map;
    }
}
