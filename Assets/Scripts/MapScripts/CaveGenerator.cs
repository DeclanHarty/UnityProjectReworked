using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public TilemapManager tilemapManager;

    public const float CYCLE_TUNNEL_CHANCE = .2f;
    public static bool[,] GenerateCaveMap(Vector2Int chunkDim, Vector2Int mapDimInChunks, int maxStartRadius, int minStartRadius, float maxRadiusChange){
        bool[,] map = new bool[chunkDim.x * mapDimInChunks.x, chunkDim.y * mapDimInChunks.y];

        HashSet<Vector2> empty_tiles = new HashSet<Vector2>();

        IPoint[] room_centers = new IPoint[mapDimInChunks.x * mapDimInChunks.y];

        // Each chunk in the map contains one 'room'
        // This loops through each chunk and creates the room for that chunk
        for(int y = 0; y < mapDimInChunks.y; y++){
            for(int x = 0; x < mapDimInChunks.x; x++){
                // Determines the starting radius of the base circle for the room
                float radius = Random.Range(minStartRadius, maxStartRadius);
                // Finds the position for the Perlin Noise
                float noisePos = Random.Range(0f, 100f);

                // gets the position of the room
                Vector2 pos = new Vector2(chunkDim.x * x, chunkDim.y * y) + new Vector2(Random.Range(Mathf.CeilToInt(maxStartRadius * (1 + maxRadiusChange) + 1), Mathf.FloorToInt(chunkDim.x - maxStartRadius * (1 + maxRadiusChange) - 1)), Random.Range(Mathf.CeilToInt(maxStartRadius * (1 + maxRadiusChange) + 1), Mathf.FloorToInt(chunkDim.y - maxStartRadius * (1 + maxRadiusChange) - 1)));

                // Adds Room to list of room centers for calculating cave tunnels
                room_centers[y * mapDimInChunks.x + x] = new Point(pos.x, pos.y);

                // Calculates the vertices of the circle to be sent to the scanline algotrithm for getting the infill of the room
                Vector2[] circlePoints = NoiseyCircle.CreateNoiseyCircle(radius, maxRadiusChange, noisePos, 2f, 28, pos);
                Vector2Int[] roundedPoints = Scanline.ConvertFloatPolygonToIntPolygon(circlePoints);
                List<Vector2> tiles = Scanline.PolygonFill(roundedPoints);
                Vector2Int[] roundedTiles = Scanline.ConvertFloatPolygonToIntPolygon(tiles.ToArray());

                // Adds the determined air tiles to the emptytiles set
                foreach(Vector2Int tile in roundedTiles){
                    empty_tiles.Add(new Vector2(tile.x, tile.y)); 
                }                                                    
            }
        }

        // Generate Delaunator
        Delaunator delaunator = new Delaunator(room_centers);

        // Create Graph from Delauny Triangulation
        double[,] adjacencyGraph = PrimsAlgo.CreateWeightedAdjacencyMatrix(room_centers, Enumerable.ToArray(delaunator.GetEdges()));

        int[,] mst = PrimsAlgo.PrimMST(adjacencyGraph);
        
        // Set used to keep track of created tunnels and prevent overlapping
        HashSet<Vector2Int> createdTunnels = new HashSet<Vector2Int>();

        // Generate the MST Tunnels
        for(int x = 0; x < mst.GetLength(0); x++){
            for(int y = 0; y < mst.GetLength(1); y++){
                if(mst[x,y] != 0 && !createdTunnels.Contains(new Vector2Int(x,y))){
                    //Create The Tunnel
                    List<Vector2> raster = TunnelCreator.TunnelRaster(room_centers[x].ToVector2(), room_centers[y].ToVector2(), 5);
                    Vector2Int[] roundedTiles = Scanline.ConvertFloatPolygonToIntPolygon(raster.ToArray());

                    foreach(Vector2Int tile in roundedTiles){
                        empty_tiles.Add(new Vector2(tile.x, tile.y)); 
                    }

                    //Add connection to createdTunnels set
                    createdTunnels.Add(new Vector2Int(x,y));
                    createdTunnels.Add(new Vector2Int(y,x));
                }
            }
        }

        // Add the extra cycling tunnels
        for(int x = 0; x < mst.GetLength(0); x++){
            for(int y = 0; y < mst.GetLength(1); y++){
                if(adjacencyGraph[x,y] != 0 && !createdTunnels.Contains(new Vector2Int(x,y))){
                    // Determine if cycle tunnel is a success
                    if(Random.Range(0f,1f) < CYCLE_TUNNEL_CHANCE){
                        //Create The Tunnel
                        List<Vector2> raster = TunnelCreator.TunnelRaster(room_centers[x].ToVector2(), room_centers[y].ToVector2(), 5);
                        Vector2Int[] roundedTiles = Scanline.ConvertFloatPolygonToIntPolygon(raster.ToArray());

                        foreach(Vector2Int tile in roundedTiles){
                            empty_tiles.Add(new Vector2(tile.x, tile.y)); 
                        }

                        //Add connection to createdTunnels set
                        createdTunnels.Add(new Vector2Int(x,y));
                        createdTunnels.Add(new Vector2Int(y,x));
                    }
                }
            }
        }






        // Goes through the given map size and creates finalizes the 2d array representation
        for(int y = 0; y < chunkDim.y * mapDimInChunks.y; y++){
            for(int x = 0; x < chunkDim.x * mapDimInChunks.x; x++){
                if(empty_tiles.Contains(new Vector2Int(x, y))){
                    map[x,y] = false;
                }else{
                    map[x,y] = true;
                }
            }
        }

        return map;
    }  
    [ContextMenu("Create Cave")]
    public void SetMap(){
        bool[,] map = GenerateCaveMap(new Vector2Int(50, 50), new Vector2Int(5,3), 15, 10, .2f);
        tilemapManager.Set2DMap(map, 250, 150);

    }

}
