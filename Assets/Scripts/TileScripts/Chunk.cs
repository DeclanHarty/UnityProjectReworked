using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int width;
    public int height;

    public bool[] map;
    
    public GameObject tilePrefab;
    public float tileSize;

    public GameObject[] tiles;

    void Awake(){
        GetComponent<BoxCollider2D>().size = new Vector2(width * tileSize, height * tileSize);
    }

    public void InstantiateChunk(){
        DestroyChunk();
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                int index = y * width + x;
                if(map[index]){
                    tiles[index] = Instantiate(tilePrefab, new Vector3(x - (width * tileSize / 2),  -y + (height * tileSize / 2)) + gameObject.transform.position, Quaternion.identity);
                    tiles[index].transform.SetParent(gameObject.transform);
                    tiles[index].GetComponent<Tile>().SetTile(index, this);
                }
            }
        }
    }

    public void DestroyChunk(){
        for(int i = 0; i < tiles.Length; i++){
            if(tiles[i] != null){
                    if(Application.isEditor){
                        DestroyImmediate(tiles[i]);
                    }else{
                        Destroy(tiles[i]);
                    }
                
                tiles[i] = null;
            }
        }
    }

    public void SetChunkSize(int width, int height){
        this.width = width;
        this.height = height;
        this.tiles = new GameObject[width * height];
    }

    public void SetMap(bool[] map){
        this.map = map;
    }

    public void SetTile(GameObject tile, float tileSize){
        this.tilePrefab = tile;
        this.tileSize = tileSize;
    }

    public void NotifyTileIsBroken(int indexOfTile){
        map[indexOfTile] = false;
        tiles[indexOfTile] = null;
    }
}
