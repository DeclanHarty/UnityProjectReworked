using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoiseyCircleLineRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float radius;
    public float noisePercentageOfRadius;
    public float noiseStartPos;
    public float scale;
    public int numOfVertices;

    public void CreateCircle(){
        // Vector2[] vertices2D = NoiseyCircle.CreateNoiseyCircle(radius, noisePercentageOfRadius, noiseStartPos, scale, numOfVertices);

        // Vector3[] vertices3D = new Vector3[vertices2D.Length + 1];
        // for(int i = 0; i < vertices2D.Length; i++){
        //     vertices3D[i] = new Vector3(vertices2D[i].x, vertices2D[i].y);
        // }
        // vertices3D[vertices2D.Length] = vertices3D[0];

        // lineRenderer.positionCount = vertices3D.Length;
        // lineRenderer.SetPositions(vertices3D);
    }
}
