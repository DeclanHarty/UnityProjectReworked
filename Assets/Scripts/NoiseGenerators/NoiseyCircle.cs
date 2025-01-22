using System;
using System.Collections;
using UnityEngine;

public class NoiseyCircle
{
    public static Vector2[] CreateNoiseyCircle(float radius, float noisePercentageOfRadius, float noiseStartPos, float scale, int numOfVertices, Vector2 centerPos){
        Vector2[] vertices = new Vector2[numOfVertices];

        float angleBetweenVertices = 2 * Mathf.PI / numOfVertices;

        for(int i = 0; i < numOfVertices; i++){
            float noise = Mathf.PerlinNoise((i + noiseStartPos) / scale, 0);


            float distanceFromCenter = radius + (radius * noisePercentageOfRadius * (noise * 2 - 1));
            Vector2 distanceVector = new Vector2(distanceFromCenter, 0);
            float angle = angleBetweenVertices * i;

            vertices[i] = new Vector2(centerPos.x + (Mathf.Cos(angle)*distanceVector.x - Mathf.Sin(angle)*distanceVector.y), centerPos.y + (Mathf.Sin(angle)*distanceVector.x + Mathf.Cos(angle)*distanceVector.y));
        }

        return vertices;
    }
}
