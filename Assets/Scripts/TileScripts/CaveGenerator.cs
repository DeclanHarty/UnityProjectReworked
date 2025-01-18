using System;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour{
    public GameObject square;

    public bool[,] CreateMap(Vector2Int mapSize){
        bool[,] map = new bool[mapSize.x, mapSize.y];
        for(int x = 0; x < mapSize.x; x++){
            for(int y = 0; y < mapSize.x; y++){
                map[x,y] = true;
            }
        }

        Vector2[] floatVertices = NoiseyCircle.CreateNoiseyCircle(30, .4f, 0.2f, 3, 30);
        Vector2Int[] vertices = Scanline.ConvertFloatPolygonToIntPolygon(floatVertices);
        
        List<Vector2> raster = Scanline.PolygonFill(vertices);

        foreach(Vector2 pos in raster){
            Instantiate(square, pos, Quaternion.identity);
        }


        return map;
    }

    void Start(){
        CreateMap(new Vector2Int(100,100));
    }
}
