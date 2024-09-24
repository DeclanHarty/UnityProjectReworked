using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredPerlinNoiseGenerator : MonoBehaviour
{
    static public float[,] GenerateNoise(int width, int height, float scale, Vector2 startPos){
        if(scale <= 0) {
            scale = 0.00001f;
        }
        float [,] noiseMap = new float[width, height];
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                noiseMap[x,y] = Mathf.PerlinNoise(x / scale + startPos.x , y / scale + startPos.y);
            }
        }

        return noiseMap;
    }

    static public bool[,] GenerateBWNoise(int width, int height, float cutoff, float scale, Vector2 startPos){
        if(scale <= 0) {
            scale = 0.00001f;
        }
        bool [,] noiseMap = new bool[width, height];
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                noiseMap[x,y] = Mathf.PerlinNoise(x / scale + startPos.x , y / scale + startPos.y) >= cutoff ? true : false ;
            }
        }

        return noiseMap;
    }

    static public float[,] GenerateFractalNoise(int width, int height, Vector2 startCoords, float scale, int octaves, float persistence, float lacunarity){
        

        float[,] layeredNoiseMap = new float[width, height];

        float maxNoiseHeight = 0;
        float minNoiseHeight = 0;

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                

                for(int i = 0; i < octaves; i++){
                    float sampleX = (x + startCoords.x) / scale * frequency;
                    float sampleY = (y + startCoords.y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    perlinValue = perlinValue * amplitude;

                    noiseHeight += perlinValue;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                
                layeredNoiseMap[x,y] = noiseHeight;

                if(noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                }else if(noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }
            }
        }
        

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                layeredNoiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, layeredNoiseMap[x,y]);
   
            }
        }

        return layeredNoiseMap;
    }

    static public bool[,] GenerateBWFractalNoise(int width, int height, Vector2 startCoords, float cutoff, float scale, int octaves, float persistence, float lacunarity){
        

        float[,] layeredNoiseMap = new float[width, height];

        float maxNoiseHeight = 0;
        float minNoiseHeight = 0;

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                

                for(int i = 0; i < octaves; i++){
                    float sampleX = (x + startCoords.x) / scale * frequency;
                    float sampleY = (y + startCoords.y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    perlinValue = perlinValue * amplitude;

                    noiseHeight += perlinValue;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                
                layeredNoiseMap[x,y] = noiseHeight;

                if(noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                }else if(noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }
            }
        }
        
        bool[,] contourLayeredNoiseMap = new bool[width,height];

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                contourLayeredNoiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, layeredNoiseMap[x,y]) >= cutoff ? true : false;
   
            }
        }

        return contourLayeredNoiseMap;
    }
}
