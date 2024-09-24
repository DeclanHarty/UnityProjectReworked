using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator
{
    // O(width^2 * height^2 * CAIterations)
    // Number of operations for a 500 square with 10 iterations = 6,250,000,000,000,000 or 6.25 Quadrillion Operations
    static public bool[] CreateMap(int width, int height, float cutoff, int CAIterations){
        bool[] newMap = StaticNoiseGenerator.GenerateStaticBWNoise(width, height, cutoff); // O(width * height)


        for(int i = 0; i < CAIterations; i++){ // O(CAIterations)
            newMap = CaveCA.RunTurn(newMap, width); // O(width * height)
        }

        //Debug.Log("New Map Length : " + newMap.Length);

        return newMap;
    }
}
