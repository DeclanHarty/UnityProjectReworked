using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class WorleyNoise
{
    public static float[,] GenerateWorleyNoise(int width, int height, int numOfCells, int[] degrees){
        float[,] worleyNoise = new float[width,height];
        Vector2Int[] cellPos = new Vector2Int[numOfCells];
        for(int i = 0; i < numOfCells; i++){
            cellPos[i] = new Vector2Int((int)Mathf.Round(UnityEngine.Random.Range(0, width)), (int)Mathf.Round(UnityEngine.Random.Range(0, height)));
        }

        float minNoiseValue = float.MaxValue;
        float maxNoiseValue = float.MinValue;
        for(int y = 0; y < height; y++){
            for(int x = 0; x < height; x++){
                float[] distances = new float[numOfCells];

                for(int i = 0; i < numOfCells; i++){
                    distances[i] = Mathf.Sqrt(Mathf.Pow(x - cellPos[i].x,2) + Mathf.Pow(y - cellPos[i].y,2));
                }

                Array.Sort(distances);

                float noiseValue = 0;
                for(int i = 0; i < degrees.Length; i++){
                    if(degrees[i] > numOfCells){
                        continue;
                    }
                    noiseValue += distances[degrees[i] - 1];
                }

                if(noiseValue < minNoiseValue){
                    minNoiseValue = noiseValue;

                }

                if(noiseValue > maxNoiseValue){
                    maxNoiseValue = noiseValue;
                }

                worleyNoise[x,y] = noiseValue;
            }
        }

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                worleyNoise[x,y] = Mathf.InverseLerp(minNoiseValue, maxNoiseValue, worleyNoise[x,y]);
            }
        }

        return worleyNoise;
    }


    public static float[,] GenerateBWWorleyNoise(int width, int height, int numOfCells, int[] degrees, float cutoff){
        float[,] worleyNoise = new float[width,height];
        Vector2Int[] cellPos = new Vector2Int[numOfCells];
        for(int i = 0; i < numOfCells; i++){ // O(NumOfCells)
            cellPos[i] = new Vector2Int((int)Mathf.Round(UnityEngine.Random.Range(0, width)), (int)Mathf.Round(UnityEngine.Random.Range(0, height)));
        }

        float minNoiseValue = float.MaxValue;
        float maxNoiseValue = float.MinValue;
        for(int y = 0; y < height; y++){ // O(height)
            for(int x = 0; x < height; x++){ // O(width)
                float[] distances = new float[numOfCells];

                for(int i = 0; i < numOfCells; i++){ // O(NumOfCells)
                    distances[i] = Mathf.Sqrt(Mathf.Pow(x - cellPos[i].x,2) + Mathf.Pow(y - cellPos[i].y,2));
                }

                Array.Sort(distances);

                float noiseValue = 0;
                for(int i = 0; i < degrees.Length; i++){ // O(degrees.Length)
                    if(degrees[i] > numOfCells){
                        continue;
                    }
                    noiseValue += distances[degrees[i] - 1];
                }

                if(noiseValue < minNoiseValue){
                    minNoiseValue = noiseValue;

                }

                if(noiseValue > maxNoiseValue){
                    maxNoiseValue = noiseValue;
                }

                worleyNoise[x,y] = noiseValue;
            }
        }

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                worleyNoise[x,y] = Mathf.InverseLerp(minNoiseValue, maxNoiseValue, worleyNoise[x,y]) <= cutoff ? 1 : 0;
            }
        }

        return worleyNoise;
    }
}
