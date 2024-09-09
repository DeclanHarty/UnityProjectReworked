using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public float tileSize;
    public GameObject tile;  

    public GameObject[] tiles;

    public void InstantiateTiles(){
        Reset();
        for(int i = 0; i < map.Length; i++){
            if(map[i]){
                GameObject newTile = Instantiate(tile, new Vector3((i % width * tileSize) - (.5f * width * tileSize), -Mathf.Floor(i / width) * tileSize + (.5f * height * tileSize)), Quaternion.identity);
                newTile.transform.localScale = new Vector3(tileSize, tileSize);
                newTile.transform.SetParent(groundParent.transform);
                tiles[i] = newTile;
            }
        }
    }
}
