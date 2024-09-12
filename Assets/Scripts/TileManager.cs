using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public float tileSize;
    public GameObject tile;  

    public int chunkWidth;
    public int chunkHeight;
    
    class Chunk{
        int width;
        int height;

        public bool[] map;

        public GameObject[] tiles;

        public Chunk(int width, int height, bool[] map, GameObject tile, float tileSize, GameObject parent){
            this.width = width;
            this.height = height;

            this.map = map;

            tiles = new GameObject[map.Length];
        }

        public void InstantiateChunk(GameObject tile, float tileSize, GameObject parent){
            for(int y = 0; y < height; y++){
                for(int x = 0; x < width; x++){
                    int index = y * width + x;
                    if(map[index]){
                        tiles[index] = Instantiate(tile, new Vector3(x - (width * tileSize / 2),  -y + (height * tileSize / 2)) + parent.transform.position, Quaternion.identity);
                        tiles[index].transform.SetParent(parent.transform);
                    }
             
                }
            }
        }
    }
}
