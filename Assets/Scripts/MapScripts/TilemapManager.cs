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
        this.map = map;
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

    public List<Vector2> GetPath(Vector2 start, Vector2 end, int maxDepth){
        //Convert Stant and End Cells to Tilemap Positions
        Vector3Int startCell3 = tilemap.WorldToCell(start);
        Vector2Int startCell = new Vector2Int(startCell3.x, startCell3.y);

        Vector3Int endCell3 = tilemap.WorldToCell(end);
        Vector2Int endCell = new Vector2Int(endCell3.x, endCell3.y);

        
        UnweighetedAdjacencyList<Vector2Int> graph = navGraph;

        List<Vector2Int> cellList = GreedyBestFirst<Vector2Int>.GBFSPath(startCell, endCell, AStar<Vector2Int>.ManhattanDistance, graph, maxDepth);
        // List<Vector2Int> cellList = AStar<Vector2Int>.UWAStarPath(startCell, endCell, AStar<Vector2Int>.DirectDistanceHeuristic, graph, maxDepth);

        List<Vector2> posList = new List<Vector2>();
        foreach(Vector2Int cell in cellList){
            Vector3 pos3 = tilemap.CellToWorld((Vector3Int)cell);
            Vector2 pos = new Vector2(pos3.x, pos3.y);
            posList.Add(pos);
        }

        return posList;
    }

    public Vector2 CellToWorldPosition(Vector2Int cellPos){
        Vector3 pos = tilemap.GetCellCenterWorld((Vector3Int)cellPos);
        return (Vector2)pos;
    }

    public Vector2Int WorldToCellPosition(Vector2 worldPos){
        Vector3Int startCell = tilemap.WorldToCell(worldPos);
        return (Vector2Int)startCell;
    }

    public int GetTileValue(Vector2Int tilePos){
        int tileXPosition = tilePos.x + map.GetLength(0) / 2;
        int tileYPosition = -tilePos.y;
        if(tileXPosition < 0 || tileYPosition < 0 || tileXPosition >= map.GetLength(0) || tileYPosition >= map.GetLength(1)){
            return 0;
        }
        return map[tileXPosition, tileYPosition];
    }

    public Vector3 GetTileSize(){
        return tilemap.cellSize;
    }
}
