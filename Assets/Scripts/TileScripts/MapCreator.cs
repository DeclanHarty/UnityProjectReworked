using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator
{
    static public bool[] CreateMap(int width, int height, float cutoff, int CAIterations){
        bool[] newMap = StaticNoiseGenerator.GenerateStaticBWNoise(width, height, cutoff);


        for(int i = 0; i < CAIterations; i++){
            newMap = CaveCA.RunTurn(newMap, width);
        }

        //Debug.Log("New Map Length : " + newMap.Length);

        return newMap;

    }
}
