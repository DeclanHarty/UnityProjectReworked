using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PerlinWormLineRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float length;
    public int segments;

    public Vector2 startPos;

    public Vector2 endPos;

    public float noiseScale;

    public float maxAngle;

    public float animSpeed;


    public void createWorm(){
        float[] wormAngles = PerlinWorms.GetWormValues(startPos, endPos, segments, noiseScale);

        float lengthPerSegment = length / segments;

        Vector3 lastPoint = transform.position;
        Vector3[] vertices = new Vector3[segments + 1];
        vertices[0] = lastPoint;
        for(int i = 0; i < segments; i++){
            float angle = maxAngle * wormAngles[i] * 2 - 1;
            lastPoint = lastPoint + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * lengthPerSegment;
            vertices[i+1] = lastPoint;
        }

        lineRenderer.positionCount = vertices.Length;
        lineRenderer.SetPositions(vertices);
    }

    public void createWormFromFloats(float[] anglePercentages){

    }

    // public void createWorm(Vector2 startPos, Vector2 endPos, int segments, float noiseScale){
    //     float[] wormAngles = PerlinWorms.GetWormValues(startPos, endPos, segments, noiseScale);

    //     float lengthPerSegment = length / segments;

    //     Vector3 lastPoint = ;
    //     Debug.Log(transform.position);
    //     Vector3[] vertices = new Vector3[segments + 1];
    //     vertices[0] = lastPoint;
    //     for(int i = 0; i < segments; i++){
    //         float angle = maxAngle * wormAngles[i] * 2 - 1;
    //         lastPoint = lastPoint + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * lengthPerSegment;
    //         vertices[i+1] = lastPoint;
    //     }

    //     lineRenderer.positionCount = vertices.Length;
    //     lineRenderer.SetPositions(vertices);
    // }

    void Update(){
        createWorm();
        startPos += new Vector2(Time.deltaTime * animSpeed, Time.deltaTime * animSpeed);
        endPos += new Vector2(Time.deltaTime * animSpeed, Time.deltaTime * animSpeed);
    }
}
