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
    public bool usingCA;

    public int CAIterations;

    public bool[] map;

    [Header("Noise Values")]
    public Vector2 startSamplePos;
    public float scale;
    public int octaves; 
    public float persistence;
    public float lacunarity;

    public bool[,] Generate2DArrayMap(){
        return LayeredPerlinNoiseGenerator.GenerateBWFractalNoise(mapWidth, mapHeight, startSamplePos, cutoff, scale, octaves, persistence, lacunarity);
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

    public void Set2DMap(bool[,] map){
        List<TileBase> tileList = new List<TileBase>();
        List<Vector3Int> tilePosList = new List<Vector3Int>();
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

    public void CreateMap(){
        ClearMap();
        bool[,] created2DArrayMap = Generate2DArrayMap();
        if(usingCA){
            for(int i = 0; i < CAIterations; i++){
                created2DArrayMap = CaveCA.Run2DTurn(created2DArrayMap, mapWidth, mapHeight);
            }
        }
        Set2DMap(created2DArrayMap);
    }

    public void ClearMap(){
        tilemap.ClearAllTiles();
    }

    public void BreakTile(Vector3Int tilePos){
        tilemap.SetTile(tilePos, null);
    }
}