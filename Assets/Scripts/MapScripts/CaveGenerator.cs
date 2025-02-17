using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class CaveGenerator : MonoBehaviour
{
    public TilemapManager tilemapManager;
    public NavGraph navGraphObject;

    public const int DIRT_TILE_ID = 0;
    public const float CYCLE_TUNNEL_CHANCE = .2f;

    public void Start() {
    }

    public static MapInfo GenerateCaveMap(Vector2Int chunkDim, Vector2Int mapDimInChunks, int maxStartRadius, int minStartRadius, float maxRadiusChange, int extraRooms){
        int[,] map = new int[chunkDim.x * mapDimInChunks.x, chunkDim.y * mapDimInChunks.y];

        for(int x = 0; x < map.GetLength(0); x++){
            for(int y = 0; y < map.GetLength(1); y++){
                map[x,y] = 0;
            }
        }


        IPoint[] room_centers = new IPoint[mapDimInChunks.x * mapDimInChunks.y + extraRooms];

        // Start time for testing
        DateTime before = DateTime.Now;

        // Each chunk in the map contains one 'room'
        // This loops through each chunk and creates the room for that chunk
        for(int y = 0; y < mapDimInChunks.y; y++){
            for(int x = 0; x < mapDimInChunks.x; x++){
                // Determines the starting radius of the base circle for the room
                float radius = UnityEngine.Random.Range(minStartRadius, maxStartRadius);
                // Finds the position for the Perlin Noise
                float noisePos = UnityEngine.Random.Range(0f, 100f);

                // gets the position of the room
                Vector2 pos = new Vector2(chunkDim.x * x, chunkDim.y * y) + new Vector2(UnityEngine.Random.Range(Mathf.CeilToInt(maxStartRadius * (1 + maxRadiusChange / 2) + 1), Mathf.FloorToInt(chunkDim.x - maxStartRadius * (1 + maxRadiusChange / 2) - 1)), UnityEngine.Random.Range(Mathf.CeilToInt(maxStartRadius * (1 + maxRadiusChange / 2) + 1), Mathf.FloorToInt(chunkDim.y - maxStartRadius * (1 + maxRadiusChange / 2) - 1)));

                // Adds Room to list of room centers for calculating cave tunnels
                room_centers[y * mapDimInChunks.x + x] = new Point(pos.x, pos.y);

                // Calculates the vertices of the circle to be sent to the scanline algotrithm for getting the infill of the room
                Vector2[] circlePoints = NoiseyCircle.CreateNoiseyCircle(radius, maxRadiusChange, noisePos, 2f, 28, pos);
                Vector2Int[] roundedPoints = Scanline.ConvertFloatPolygonToIntPolygon(circlePoints);
                List<Vector2> tiles = Scanline.PolygonFill(roundedPoints);
                Vector2Int[] roundedTiles = Scanline.ConvertFloatPolygonToIntPolygon(tiles.ToArray());

                // Sets tiles to false that should be air
                foreach(Vector2Int tile in roundedTiles){
                    map[tile.x, tile.y] = -1;
                }                                                    
            }
        }

        // Adds extra rooms to the cave
        for(int i = 0; i < extraRooms; i++){
            float radius = UnityEngine.Random.Range(minStartRadius, maxStartRadius);
                // Finds the position for the Perlin Noise
                float noisePos = UnityEngine.Random.Range(0f, 100f);

                // gets the position of the room
                Vector2 pos = new Vector2(UnityEngine.Random.Range(Mathf.CeilToInt(maxStartRadius * (1 + maxRadiusChange / 2) + 1), Mathf.FloorToInt(chunkDim.x * mapDimInChunks.x - maxStartRadius * (1 + maxRadiusChange / 2) - 1)), UnityEngine.Random.Range(Mathf.CeilToInt(maxStartRadius * (1 + maxRadiusChange / 2) + 1), Mathf.FloorToInt(chunkDim.y * mapDimInChunks.y - maxStartRadius * (1 + maxRadiusChange / 2) - 1)));
                // Adds Room to list of room centers for calculating cave tunnels
                room_centers[mapDimInChunks.y * mapDimInChunks.x + i] = new Point(pos.x, pos.y);

                // Calculates the vertices of the circle to be sent to the scanline algotrithm for getting the infill of the room
                Vector2[] circlePoints = NoiseyCircle.CreateNoiseyCircle(radius, maxRadiusChange, noisePos, 2f, 28, pos);
                Vector2Int[] roundedPoints = Scanline.ConvertFloatPolygonToIntPolygon(circlePoints);
                List<Vector2> tiles = Scanline.PolygonFill(roundedPoints);
                Vector2Int[] roundedTiles = Scanline.ConvertFloatPolygonToIntPolygon(tiles.ToArray());

                // Adds the determined air tiles to the emptytiles set
                foreach(Vector2Int tile in roundedTiles){
                    map[tile.x, tile.y] = -1;
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
                        map[tile.x, tile.y] = -1;
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
                    if(UnityEngine.Random.Range(0f,1f) < CYCLE_TUNNEL_CHANCE){
                        //Create The Tunnel
                        List<Vector2> raster = TunnelCreator.TunnelRaster(room_centers[x].ToVector2(), room_centers[y].ToVector2(), 5);
                        Vector2Int[] roundedTiles = Scanline.ConvertFloatPolygonToIntPolygon(raster.ToArray());

                        foreach(Vector2Int tile in roundedTiles){
                            map[tile.x, tile.y] = -1; 
                        }

                        //Add connection to createdTunnels set
                        createdTunnels.Add(new Vector2Int(x,y));
                        createdTunnels.Add(new Vector2Int(y,x));
                    }
                }
            }
        }

        // Determines the room the player will spawn in
        int startRoomIndex = UnityEngine.Random.Range(0, mapDimInChunks.x);
        Vector2 playerSpawn__ = room_centers[startRoomIndex].ToVector2();
        Vector2Int playerSpawn = new Vector2Int((int)Math.Round(playerSpawn__.x) - (chunkDim.x * mapDimInChunks.x)   / 2, -(int)Math.Round(playerSpawn__.y));

        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
        Debug.Log("EmptyTile Creation Duration in milliseconds: " + duration.Milliseconds);

        return new MapInfo(playerSpawn, map);
    }  

    [ContextMenu("Create Cave")]
    public static MapInfo CreateMap(Vector2Int chunkDim, Vector2Int mapDimInChunks){
        MapInfo mapInfo;
        mapInfo = GenerateCaveMap(chunkDim, mapDimInChunks, 20, 10, .4f, 3);

        mapInfo.SetNavGraph(CreateNavGraph(mapInfo.GetMap(), chunkDim.x * mapDimInChunks.x));

        return mapInfo;
    }

    [ContextMenu("Test Generation Time")]
    public void TestGenerate(){
        Vector2Int chunkDim = new Vector2Int(75, 75);
        Vector2Int mapDimInChunks = new Vector2Int(5,3);

        MapInfo mapInfo;
        try{
            mapInfo = GenerateCaveMap(chunkDim, mapDimInChunks, 20, 10, .4f, 3);
        }catch(ArgumentOutOfRangeException){
            mapInfo  = GenerateCaveMap(chunkDim, mapDimInChunks, 20, 10, .4f, 3);
        }

        CreateNavGraph(mapInfo.GetMap(), chunkDim.x * mapDimInChunks.x);
    }



    public static bool[,] CreateMapFromEmptyTiles(Vector2Int chunkDim, Vector2Int mapDimInChunks, HashSet<Vector2Int> emptyTiles){
        bool[,] map = new bool[chunkDim.x * mapDimInChunks.x, chunkDim.y * mapDimInChunks.y];

        for(int y = 0; y < chunkDim.y * mapDimInChunks.y; y++){
            for(int x = 0; x < chunkDim.x * mapDimInChunks.x; x++){
                if(emptyTiles.Contains(new Vector2Int(x, y))){
                    map[x,y] = false;
                }else{
                    map[x,y] = true;
                }
            }
        }

        return map;
    }

    public static UnweighetedAdjacencyList<Vector2Int> CreateNavGraph(int[,] map, int mapWidthInTiles){
        
        DateTime before = DateTime.Now;
        UnweighetedAdjacencyList<Vector2Int> navGraph = new UnweighetedAdjacencyList<Vector2Int>();
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                List<Vector2Int> neighbors = new List<Vector2Int>();

                // for(int y = tile.y - 1; y <= tile.y + 1; y++){
                //     for(int x = tile.x - 1; x <= tile.x + 1; x++){
                //         if((x != emptyTiles[i].x || y != emptyTiles[i].y) && emptyTiles.Contains(new Vector2Int(x,y))){
                //             neighbors.Add(new Vector2Int(x - mapWidthInTiles / 2, -y));
                //         }
                //     }
                // }

                AddValidNeighbors(new Vector2Int(x,y), map, neighbors);
                UnweighetedAdjacencyListNode<Vector2Int> node = new UnweighetedAdjacencyListNode<Vector2Int>(new Vector2Int(x - mapWidthInTiles / 2, -y), neighbors);
                navGraph.AddNode(node);
            }
        }
            

        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
        Debug.Log("NavGraph Duration in milliseconds: " + duration.Milliseconds);
        return navGraph;
    }

    public static UnweighetedAdjacencyList<Vector2Int> CreateReducedNavGraph(int[,] map, int mapWidthInTiles){
        
        DateTime before = DateTime.Now;
        UnweighetedAdjacencyList<Vector2Int> navGraph = new UnweighetedAdjacencyList<Vector2Int>();
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        for(int x = 2; x < width; x+=6){
            for(int y = 2; y < height; y+=6){
                List<Vector2Int> neighbors = new List<Vector2Int>();
                AddValidReducedNeighbors(new Vector2Int(x,y), map, neighbors);
                UnweighetedAdjacencyListNode<Vector2Int> node = new UnweighetedAdjacencyListNode<Vector2Int>(new Vector2Int(x - mapWidthInTiles / 2, -y), neighbors);
                navGraph.AddNode(node);
            }
        }
            

        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
        Debug.Log("NavGraph Duration in milliseconds: " + duration.Milliseconds);
        return navGraph;
    }

    public static UnweighetedAdjacencyList<Vector2Int> CreateNavGraphWithFourNeighbors(int[,] map, int mapWidthInTiles){
        
        DateTime before = DateTime.Now;
        UnweighetedAdjacencyList<Vector2Int> navGraph = new UnweighetedAdjacencyList<Vector2Int>();
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                List<Vector2Int> neighbors = new List<Vector2Int>();

                // for(int y = tile.y - 1; y <= tile.y + 1; y++){
                //     for(int x = tile.x - 1; x <= tile.x + 1; x++){
                //         if((x != emptyTiles[i].x || y != emptyTiles[i].y) && emptyTiles.Contains(new Vector2Int(x,y))){
                //             neighbors.Add(new Vector2Int(x - mapWidthInTiles / 2, -y));
                //         }
                //     }
                // }

                AddValidCloseNeighbors(new Vector2Int(x,y), map, neighbors);
                UnweighetedAdjacencyListNode<Vector2Int> node = new UnweighetedAdjacencyListNode<Vector2Int>(new Vector2Int(x - mapWidthInTiles / 2, -y), neighbors);
                navGraph.AddNode(node);
            }
        }
            

        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
        Debug.Log("NavGraph Duration in milliseconds: " + duration.Milliseconds);
        return navGraph;
    }

    public static void AddValidNeighbors(Vector2Int tilePos, int[,] map, List<Vector2Int> neighbors){
        for(int y = tilePos.y - 1; y <= tilePos.y + 1; y++){
                for(int x = tilePos.x - 1; x <= tilePos.x + 1; x++){
                    if(x < 0 || x >= map.GetLength(0) || y < 0 || y >= map.GetLength(1)){
                        continue;
                    }
                    if((x != tilePos.x || y != tilePos.y) && map[x,y] == -1){
                        neighbors.Add(new Vector2Int(x - map.GetLength(0) / 2, -y));
                    }
                }
            }
    }

    public static void AddValidCloseNeighbors(Vector2Int tilePos, int[,] map, List<Vector2Int> neighbors){
        Vector2Int[] neighborDirections = {new Vector2Int(1,0), new Vector2Int(-1,0), new Vector2Int(0,1),new Vector2Int(0,-1)};
        foreach(Vector2Int dir in neighborDirections){
            int x = tilePos.x + dir.x;
            int y = tilePos.y + dir.y;
            if(x < 0 || x >= map.GetLength(0) || y < 0 || y >= map.GetLength(1)){
                continue;
            }
            if(map[x, y] == -1){
                neighbors.Add(new Vector2Int(x - map.GetLength(0) / 2, -y));
            }
        }
            
    }

    public static void AddValidReducedNeighbors(Vector2Int tilePos, int[,] map, List<Vector2Int> neighbors){
        Vector2Int[] neighborDirections = {new Vector2Int(2,0), new Vector2Int(-2,0), new Vector2Int(0,2),new Vector2Int(0,-2), new Vector2Int(2,2), new Vector2Int(2,-2), new Vector2Int(-2,2), new Vector2Int(-2,-2)};
        foreach(Vector2Int dir in neighborDirections){
            int x = tilePos.x + dir.x;
            int y = tilePos.y + dir.y;
            if(x < 0 || x >= map.GetLength(0) || y < 0 || y >= map.GetLength(1)){
                continue;
            }
            if(map[x, y] == -1){
                neighbors.Add(new Vector2Int(x - map.GetLength(0) / 2, -y));
            }
        }
    }
        
    

}
