using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class StaticNoiseGenerator
{
    public static float[] GenerateStaticNoise(int width, int height){
        float[] staticNoise = new float[width * height];

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                staticNoise[y * width + x] = Random.Range(0f, 1f);
            }
        }

        return staticNoise;
    }

    public static bool[] GenerateStaticBWNoise(int width, int height, float cutoff){
        bool[] staticNoise = new bool[width * height];

        // O(width * height)
        for(int y = 0; y < height; y++){  // O(height)
            for(int x = 0; x < width; x++){ // O(width)
                staticNoise[y * width + x] = Random.Range(0f, 1f) >= cutoff ? false : true;
            }
        }

        return staticNoise;
    }
}
