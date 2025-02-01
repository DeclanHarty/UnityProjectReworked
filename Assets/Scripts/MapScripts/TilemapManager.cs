using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap tilemap;

    public List<TileBase> tiles;

    public int[,] map;


    public UnweighetedAdjacencyList<Vector2Int> navGraph;

    public void Set2DMap(int[,] map){
        int mapWidth = map.GetLength(0);
        int mapHeight = map.GetLength(1);
        List<TileBase> tileList = new List<TileBase>();
        List<Vector3Int> tilePosList = new List<Vector3Int>();
        tilemap.ClearAllTiles();
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                int tile_id = map[x,y];
                if(map[x,y] != -1){
                    tileList.Add(tiles[tile_id]);
                    tilePosList.Add(new Vector3Int(x - mapWidth / 2, -y));
                }
            }
        }
        
        tilemap.SetTiles(tilePosList.ToArray(), tileList.ToArray());
    }

    
    public void SetNavGraph(UnweighetedAdjacencyList<Vector2Int> navGraph){
        this.navGraph = navGraph;
    
    }

    public List<Vector2Int> GetAStarPoints(Vector2 start, Vector2 end){
        //Convert Stant and End Cells to Tilemap Positions
        Vector3Int startCell3 = tilemap.WorldToCell(start);
        Vector2Int startCell = new Vector2Int(startCell3.x, startCell3.y);

        Debug.Log(startCell);

        Vector3Int endCell3 = tilemap.WorldToCell(end);
        Vector2Int endCell = new Vector2Int(endCell3.x, endCell3.y);

        
        UnweighetedAdjacencyList<Vector2Int> graph = navGraph;
        Debug.Log(graph.Count());

        List<Vector2Int> cellList = AStar<Vector2Int>.UWAStarPath(startCell, endCell, AStar<Vector2Int>.DirectDistanceHeuristic, graph);
        List<Vector2Int> posList = new List<Vector2Int>();
        foreach(Vector2Int cell in cellList){
            Vector3 pos3 = tilemap.CellToWorld((Vector3Int)cell);
            posList.Add(new Vector2Int((int)Math.Floor(pos3.x), (int)Math.Floor(pos3.x)));
        }
        return posList;
    }

    public Vector2 CellToWorldPosition(Vector2Int cellPos){
        Vector3 pos = tilemap.CellToWorld((Vector3Int)cellPos);
        return (Vector2)pos;
    }

    public Vector2Int WorldToCellPosition(Vector2 worldPos){
        Vector3Int startCell = tilemap.WorldToCell(worldPos);
        return (Vector2Int)startCell;
    }
}
