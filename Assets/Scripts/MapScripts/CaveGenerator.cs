using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public TilemapManager tilemapManager;
    public static bool[,] GenerateCaveMap(int widthInTiles, int heightInTiles, int numberOfRooms, int maxStartRadius, int minStartRadius, float maxRadiusChange){
        bool[,] map = new bool[widthInTiles, heightInTiles];
        for(int i = 0; i < widthInTiles; i++){
            for(int j = 0; j < heightInTiles; j++){
                map[i,j] = true;
            }

        }

        for(int i = 0; i < numberOfRooms; i++){
            float radius = Random.Range(minStartRadius, maxStartRadius);
            float noisePos = Random.Range(0f, 100f);

            Vector2 pos = new Vector2(Random.Range(Mathf.CeilToInt(maxStartRadius * 1.5f), Mathf.FloorToInt(widthInTiles - maxStartRadius * 1.5f)), Random.Range(Mathf.CeilToInt(maxStartRadius * 1.5f), Mathf.FloorToInt(heightInTiles - maxStartRadius * 1.5f)));

            Vector2[] circlePoints = NoiseyCircle.CreateNoiseyCircle(radius, maxRadiusChange, noisePos, 2f, 28, pos);
            Vector2Int[] roundedPoints = Scanline.ConvertFloatPolygonToIntPolygon(circlePoints);
            List<Vector2> tiles = Scanline.PolygonFill(roundedPoints);
            Vector2Int[] roundedTiles = Scanline.ConvertFloatPolygonToIntPolygon(tiles.ToArray());

            foreach(Vector2Int tile in roundedTiles){
                map[tile.x,tile.y] = false; 
            }                                                    
        }

        foreach(bool tile in map){
            Debug.Log(tile);
        }

        return map;
    }  
    [ContextMenu("Create Cave")]
    public void SetMap(){
        bool[,] map = GenerateCaveMap(200, 1000, 10, 15,10);
        tilemapManager.Set2DMap(map, 200, 1000);

    }

}
