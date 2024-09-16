using System.Collections.Generic;
using Unity.Mathematics;
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

    public int CAIterations;

    public bool[] map;


    public void GenerateMap(){
        map = MapCreator.CreateMap(mapWidth, mapHeight, cutoff, CAIterations);
    }

    public void SetMap(){
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

    public void CreateMap(){
        GenerateMap();
        SetMap();
    }

    public void ClearMap(){
        tilemap.ClearAllTiles();
    }
}