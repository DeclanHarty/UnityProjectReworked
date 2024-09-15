using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    #region Tiles
    public float tileSize;
    public GameObject tile; 

    #endregion

    #region Chunk Params
    public int chunkWidth;
    public int chunkHeight;
    public GameObject chunkPrefab;

    public GameObject[] chunks;

    public HashSet<Chunk> activeChunks = new HashSet<Chunk>();

    public float chunkCheckRadius;
    public LayerMask checkLayerMask;
    #endregion

    #region Map Params
    public int mapWidth;
    public int mapHeight;

    [Range(0,1)]
    public float cutoff;

    public int CAIterations;

    public bool[] map;
    #endregion

    





    void Awake(){
        chunks = new GameObject[0];
    }

    public bool[] CreateMap(int width, int height, float cutoff, int CAIterations){
        bool[] newMap = StaticNoiseGenerator.GenerateStaticBWNoise(width, height, cutoff);


        for(int i = 0; i < CAIterations; i++){
            newMap = CaveCA.RunTurn(newMap, width);
        }

        //Debug.Log("New Map Length : " + newMap.Length);

        return newMap;

    }

    public void GenerateChunks(){
        map = CreateMap(mapWidth, mapHeight, cutoff, CAIterations);
        // Check if the given chunk dimensions evenly fit in the map dimensions
        if(mapWidth % chunkWidth != 0 && mapHeight % chunkHeight != 0){
            throw new ArgumentException("Chunk dimensions must evenly divide into map dimensions");
        }

        int mapWidthInChunks = mapWidth / chunkWidth;
        int mapHeightInChunks = mapHeight / chunkHeight;

        chunks = new GameObject[mapWidthInChunks * mapHeightInChunks];

        for(int y = 0; y < mapHeightInChunks; y++){
            for(int x = 0; x < mapWidthInChunks; x++){
                GameObject chunk = Instantiate(chunkPrefab, new Vector3(x * tileSize * chunkWidth - (tileSize * chunkWidth * (mapWidth / chunkWidth - 1))/2, -y * chunkHeight * tileSize), Quaternion.identity);
                chunks[y * mapWidthInChunks + x] = chunk;

                bool[] chunkMap = new bool[chunkWidth * chunkHeight];
                // Set Map of Chunk from total map
                for(int layer = 0; layer < chunkHeight; layer++){
                    for(int chunkX = 0; chunkX < chunkWidth; chunkX++){
                        // Debug.Log("Chunk Index : " + (layer * chunkWidth + chunkX));
                        // Debug.Log("Chunk Map Length : " + chunkMap.Length);
                        //Debug.Log("Map Index : " + ((y * chunkWidth + layer) * (chunkWidth * mapWidthInChunks) + x * chunkWidth + chunkX));
                        // Debug.Log("Map Length : " + map.Length);
                        chunkMap[layer * chunkWidth + chunkX] = map[((y * chunkWidth + layer) * chunkWidth * mapWidthInChunks) + x * chunkWidth + chunkX];
                        
                    }
                }

                Chunk chunkScript = chunk.GetComponent<Chunk>();
                chunkScript.SetChunkSize(chunkWidth, chunkHeight);
                chunkScript.SetMap(chunkMap);
                chunkScript.SetTile(tile, tileSize);
            }
        }
    }

    public void RemoveChunks(){
        for(int i = 0; i < chunks.Length; i++){
            if(Application.isEditor){
                DestroyImmediate(chunks[i]);
            }else{
                Destroy(chunks[i]);
            }
            chunks[i] = null;
        }
    }

    public void InstantiateAllTilesInChunks(){
        foreach(GameObject chunk in chunks){
            chunk.GetComponent<Chunk>().InstantiateChunk();
        }
    }

    public void DestroyAllTilesInChunks(){
        foreach(GameObject chunk in chunks){
            chunk.GetComponent<Chunk>().DestroyChunk();
        }
    }

    public HashSet<Chunk> GetChunksInRadius(Vector2 playerPos){
       HashSet<Chunk> chunks = new HashSet<Chunk>();

        RaycastHit2D[] hits = Physics2D.CircleCastAll(playerPos, chunkCheckRadius, Vector2.zero, 0, checkLayerMask);

        foreach(RaycastHit2D hit in hits){
            if(hit.collider.tag == "Chunk"){
                chunks.Add(hit.collider.gameObject.GetComponent<Chunk>());
            }
        }


        return chunks;
    }

    public void ChangeActiveChunks(HashSet<Chunk> chunks){
        if(activeChunks.Equals(chunks)){
            return;
        }

        IEnumerable<Chunk> chunksToDeactivate = activeChunks.Except(chunks);

        IEnumerable<Chunk> chunksToActivate = chunks.Except(activeChunks);

        foreach(Chunk chunk in chunksToDeactivate){
            chunk.DestroyChunk();
        }

        foreach(Chunk chunk in chunksToActivate){
            chunk.InstantiateChunk();
        }

        activeChunks = chunks;
    }

    
    
}
