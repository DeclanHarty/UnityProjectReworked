using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int indexInChunkMap;
    public Chunk chunk;


    void Break(){
        chunk.NotifyTileIsBroken(indexInChunkMap);
        Destroy(gameObject);
    }

    void OnMouseDown(){
        Break();
    }

    public void SetTile(int index, Chunk chunk){
        this.indexInChunkMap = index;
        this.chunk = chunk;
    }



}
