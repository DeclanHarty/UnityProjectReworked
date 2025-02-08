using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public Vector2Int spawnPosInTilemap;
    public Vector2Int roomCenters;
    public UnweighetedAdjacencyList<Vector2Int> navGraph;
    public int[,] map;


    public MapInfo(Vector2Int spawnPosInTilemap, int[,] map){
        this.spawnPosInTilemap = spawnPosInTilemap;
        this.map = map;
    }

    public void SetNavGraph(UnweighetedAdjacencyList<Vector2Int> navGraph){
        this.navGraph = navGraph;
    }

    public Vector2Int GetSpawnPos(){
        return spawnPosInTilemap;
    }

    public int[,] GetMap(){
        return map;
    }

    public UnweighetedAdjacencyList<Vector2Int> GetNavGraph(){
        return navGraph;
    }
}
