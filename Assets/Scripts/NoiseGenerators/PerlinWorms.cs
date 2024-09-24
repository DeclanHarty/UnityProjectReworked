using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinWorms : MonoBehaviour
{
    public int width;
    public int height;
    public float scale;


    static public float[] GetWormValues(Vector2 startPos, Vector2 endPos, int segments, float noiseScale){
        float[] worm = new float[segments];

        Vector2 slope = (endPos - startPos) / (segments * noiseScale);
        for(int i = 0; i < segments; i++){
            Vector2 noisePos = startPos + i*slope;
            worm[i] = Mathf.PerlinNoise(noisePos.x, noisePos.y);
        }

        return worm;
    }

}
