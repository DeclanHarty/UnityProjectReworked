using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap tilemap;

    public TileBase tile;

    public float tileSize;

    [Header("Map Attributes")]
    public int mapWidth;
    public int mapHeight;

    [Range(0,1)]
    public float cutoff;

    [Header("CA Attributes")]
    public bool usingCASmoothing;

    public int CAIterations;

    public bool[] map;


    [Header("Noise Values")]
    public Vector2 startSamplePos;
    public float scale;
    public int octaves; 
    public float persistence;
    public float lacunarity;

    [Header("Line Values")]
    public Vector2Int lineStartPos;
    public Vector2Int lineEndPos;

    public GenerationType generationType;

    public bool[,] CreateLayeredNoiseMap(){
        return LayeredPerlinNoiseGenerator.GenerateBWFractalNoise(mapWidth, mapHeight, startSamplePos, cutoff, scale, octaves, persistence, lacunarity);
    }

    public bool[,] CreateRandomMap(){
        return StaticNoiseGenerator.GenerateStaticBWNoise(mapWidth, mapHeight, cutoff);
    }

    public void SetMap(bool[] map){
        List<TileBase> tileList = new List<TileBase>();
        List<Vector3Int> tilePosList = new List<Vector3Int>();
        for(int i = 0; i < map.Length; i++){
            if(map[i]){
                tileList.Add(tile);
                tilePosList.Add(new Vector3Int(i % mapWidth - mapWidth / 2, -(int)Mathf.Floor(i / mapWidth)));
            }
        }
        tilemap.SetTiles(tilePosList.ToArray(), tileList.ToArray());
    }

    public void GenerateRasterizedLine(){
        List<Vector2Int> line = BresenhamLineRasterizer.PlotLine(lineStartPos.x, lineStartPos.y, lineEndPos.x, lineEndPos.y);

        bool[,] newMap = ConvertArrayMapTo2DArrayMap(map, mapWidth, mapHeight);
        foreach(Vector2Int tile in line){
            newMap[tile.x, tile.y] = true;
        }

        Set2DMap(newMap, mapWidth, mapHeight);
    }

    public void EraseRasterizedLine(){
        List<Vector2Int> line = BresenhamLineRasterizer.PlotLine(lineStartPos.x, lineStartPos.y, lineEndPos.x, lineEndPos.y);

        bool[,] newMap = ConvertArrayMapTo2DArrayMap(map, mapWidth, mapHeight);
        foreach(Vector2Int tile in line){
            newMap[tile.x, tile.y] = false;
        }

        Set2DMap(newMap, mapWidth, mapHeight);
    }

    public void Set2DMap(bool[,] map, int mapWidth, int mapHeight){
        this.map = Convert2DArrayMapToArrayMap(map, mapWidth, mapHeight);
        List<TileBase> tileList = new List<TileBase>();
        List<Vector3Int> tilePosList = new List<Vector3Int>();
        tilemap.ClearAllTiles();
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                if(map[x,y]){
                    tileList.Add(tile);
                    tilePosList.Add(new Vector3Int(x - mapWidth / 2, -y));
                }
            }
        }
        
        tilemap.SetTiles(tilePosList.ToArray(), tileList.ToArray());
    }

    public void GenerateRandomCellularAutomataMap(){
        ClearMap();
        bool[,] created2DArrayMap = CreateRandomMap();
        for(int i = 0; i < CAIterations; i++){
            created2DArrayMap = CaveCA.Run2DTurn(created2DArrayMap, mapWidth, mapHeight);
        }
        Set2DMap(created2DArrayMap, mapWidth, mapHeight);
    }

    public void GenerateLayeredNoiseMap(){
        ClearMap();
        bool[,] created2DArrayMap = CreateLayeredNoiseMap();
        if(usingCASmoothing){
            for(int i = 0; i < CAIterations; i++){
                created2DArrayMap = CaveCA.Run2DTurn(created2DArrayMap, mapWidth, mapHeight);
            }
        }
        Set2DMap(created2DArrayMap, mapWidth, mapHeight);
    }

    public void ClearMap(){
        tilemap.ClearAllTiles();
        for(int i = 0; i < map.Length; i++){
            map[i] = false;
        }
    }

    public void GenerateFullMap(){
        bool[,] newMap = new bool[mapWidth, mapHeight];

        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                newMap[x,y] = true;
            }
        }

        Set2DMap(newMap, mapWidth, mapHeight);
    }

    public void BreakTile(Vector3Int tilePos){
        tilemap.SetTile(tilePos, null);
    }

    public static bool[,] ConvertArrayMapTo2DArrayMap(bool[] map, int mapWidth, int mapHeight){
        bool[,] newMap = new bool[mapWidth, mapHeight];

        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                newMap[x,y] = map[y * mapWidth + x];
            }
        }

        return newMap;
    }

    public static bool[] Convert2DArrayMapToArrayMap(bool[,] map, int mapWidth, int mapHeight){
        bool[] newMap = new bool[mapWidth * mapHeight];

        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                newMap[y * mapWidth + x] = map[x,y];
            }
        }

        return newMap;
    }

}

public enum GenerationType{
    CELLULAR_AUTOMATA,
    FRACTAL_NOISE,
    GENERATE_RASTERIZED_LINE,
    ERASE_RASTERIZED_LINE
}