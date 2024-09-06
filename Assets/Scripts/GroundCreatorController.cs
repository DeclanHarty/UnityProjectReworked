using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GroundCreatorController : MonoBehaviour
{
    public int width;
    public int height;
    public float cutoff;
    public int CAIterations;

    public float tileSize;
    public GameObject tile;

    private bool[] map;

    public GameObject[] tiles;

    public void CreateMap(){
        DestroyTiles(tiles);
        tiles = new GameObject[width * height];
        map = StaticNoiseGenerator.GenerateStaticBWNoise(width, height, cutoff);

        for(int i = 0; i < CAIterations; i++){
            map = CaveCA.RunTurn(map, width);
        }

        for(int i = 0; i < map.Length; i++){
            if(map[i]){
                GameObject newTile = Instantiate(tile, new Vector3((i % width * tileSize) - (.5f * width * tileSize), -Mathf.Floor(i / width) * tileSize + (.5f * height * tileSize)), Quaternion.identity);
                newTile.transform.localScale = new Vector3(tileSize, tileSize);
                tiles[i] = newTile;
            }
        }
    }

    void Start(){
        // createMap();
    }

    void DestroyTiles(GameObject[] gameObjects){
        for(int i = 0; i < gameObjects.Length; i++){
            DestroyImmediate(gameObjects[i]);
        }
    }

    public void Reset(){
        DestroyTiles(tiles);
    }
}
