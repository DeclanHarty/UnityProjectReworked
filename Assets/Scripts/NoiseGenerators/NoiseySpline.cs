using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class NoiseySpline : MonoBehaviour
{
    [Min(1)]
    public int scale;

    public float maxVerticalDisplacement;
    public float maxHorizontalDisplacement;

    public Vector3 startPos;
    public Vector3 endPos;

    public SplineCurve spline;

    public static Vector3[] CreateNoiseySplineControlPositionsWith2D_Displacement(Vector3 startPos, Vector3 endPos, int scale, float maxVerticalDisplacement, float maxHorizontalDisplacement){
        Vector3[] controlPositions = new Vector3[scale * 3 + 1];
        controlPositions[0] = startPos;
        controlPositions[scale * 3] = endPos;


        Vector3 tangentDirection = (endPos - startPos).normalized;
        float distance = (endPos - startPos).magnitude;
        Vector3 normalDirection = new Vector3(-tangentDirection.y, tangentDirection.x, 0).normalized;

        float distancePerControlPoint = distance/(controlPositions.Length - 1);

        for(int i = 1; i < scale * 3; i++){
            float OneDimDisplacement = Random.Range(-maxVerticalDisplacement, maxVerticalDisplacement);
            Vector3 TwoDimDisplacement = new Vector3(Random.Range(-maxHorizontalDisplacement, maxHorizontalDisplacement), Random.Range(-maxVerticalDisplacement, maxVerticalDisplacement), 0);

            controlPositions[i] = startPos + i * distancePerControlPoint * tangentDirection + TwoDimDisplacement;
        }

        return controlPositions;
    }

    public static Vector3[] CreateNoiseySplineControlPositionsWith1D_Displacement(Vector3 startPos, Vector3 endPos, int numOfSegments, float scale, float maxDisplacement){
        Vector3[] controlPositions = new Vector3[numOfSegments * 3 + 1]; // Initializes the control Position Array
        controlPositions[0] = startPos;
        controlPositions[numOfSegments * 3] = endPos;

        float noiseStartX = Random.Range(0f,1f);

        Vector3 tangentDirection = (endPos - startPos).normalized;
        float distance = (endPos - startPos).magnitude;
        Vector3 normalDirection = new Vector3(-tangentDirection.y, tangentDirection.x, 0).normalized;

        float distancePerControlPoint = distance/(controlPositions.Length - 1);

        for(int i = 1; i < numOfSegments * 3; i++){
            //float OneDimDisplacement = (Mathf.PerlinNoise(noiseStartX + i*scale, 0) * 2 - 1) * maxDisplacement;
            float OneDimDisplacement = (Mathf.PerlinNoise(noiseStartX + (i/( scale * (endPos - startPos).magnitude)), 0) * 2 - 1) * maxDisplacement;
            controlPositions[i] = startPos + i * distancePerControlPoint * tangentDirection + OneDimDisplacement * normalDirection;
        }

        return controlPositions;
    }

    [ContextMenu("GenerateSpline")]
    public void GenerateSpline(){
        spline.SetControlPointPositions(CreateNoiseySplineControlPositionsWith2D_Displacement(startPos, endPos, scale, maxVerticalDisplacement, maxHorizontalDisplacement));
    }
}
