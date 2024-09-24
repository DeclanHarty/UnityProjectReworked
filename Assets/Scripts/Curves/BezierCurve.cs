using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public GameObject[] controlPoints;


    public GameObject lineObject;
    public GameObject activeLineObject;

    [Min(1)]
    public int resolution;

    void Start(){
        activeLineObject = Instantiate(lineObject, transform);
    }

    void Update(){
        float tPerVert = 1f/(resolution+1);
        Vector3[] positions = new Vector3[resolution + 2];
        Vector3[] controlPointPositions = new Vector3[controlPoints.Length];
        for(int i = 0; i < controlPoints.Length; i++){
            controlPointPositions[i] = controlPoints[i].transform.position;
        }
        for(int i = 0; i < resolution + 2; i++){
            positions[i] = GetBezierValues(tPerVert * i, controlPointPositions);
        }

        activeLineObject.GetComponent<LineRendererObject>().EnableLine(positions, Color.white);
    }


    Vector3 GetBezierValues(float t, Vector3[] controlPointPositions){
        Vector3 curvePosition = Vector3.zero;
        Vector3[] interpolatedPositions = controlPointPositions;

        for(int i = 0; i < controlPoints.Length - 1; i++){
            Vector3[] newInterpolatedPositions = new Vector3[controlPoints.Length - 1 - i];
            for(int interpIndex = 0; interpIndex < interpolatedPositions.Length - 1; interpIndex++){
                newInterpolatedPositions[interpIndex] = Vector3.Lerp(interpolatedPositions[interpIndex], interpolatedPositions[interpIndex + 1], t);
            }

            interpolatedPositions = newInterpolatedPositions;
        }

        curvePosition = interpolatedPositions[0];

        return curvePosition;
    }
}
