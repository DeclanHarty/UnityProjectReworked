using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticBezierCurve : MonoBehaviour
{
    public GameObject[] controlPoints = new GameObject[0];
    public GameObject bezierPoint;
    public Vector3[] controlPointPositions;
    public Vector3[] lerpedPositions;
    public GameObject[] lerpTargets = new GameObject[2];

    [Range(0,1)]
    public float t;

    [Min(3)]
    public int vertices;

    public LineRenderer lineRenderer;
    public GameObject lineObject;
    public GameObject activeLineObject;

    void Start(){
        lerpTargets[0] = Instantiate(controlPoints[0], transform);
        lerpTargets[1] = Instantiate(controlPoints[0], transform);
        activeLineObject = Instantiate(lineObject, transform);
    }

    void Update(){
        controlPointPositions = new Vector3[controlPoints.Length];
        for(int i = 0; i < controlPoints.Length; i++){
            controlPointPositions[i] = controlPoints[i].transform.position;
        } 

        lineRenderer.positionCount = controlPointPositions.Length;
        lineRenderer.SetPositions(controlPointPositions);
        
        Vector3 position = GetBezierValues(t);
        for(int index = 0; index < lerpedPositions.Length; index++){
            lerpTargets[index].GetComponent<ControlPointControllers>().SetPosition(lerpedPositions[index]);
        }
        float tPerVert = 1f/(vertices-1);
        Debug.Log(tPerVert);
        Vector3[] positions = new Vector3[vertices];
        for(int i = 0; i < vertices; i++){
            positions[i] = GetBezierValues(tPerVert * i);
        }
        activeLineObject?.GetComponent<LineRendererObject>().EnableLine(positions, Color.black);

        bezierPoint?.GetComponent<ControlPointControllers>().SetPosition(position);
    }

    Vector3 GetBezierValues(float t){
        lerpedPositions = new Vector3[controlPointPositions.Length - 1];
        Vector3 curvePosition = Vector3.zero;
        for(int i = 0; i < controlPointPositions.Length - 1; i++){
            lerpedPositions[i] = Vector3.Lerp(controlPointPositions[i], controlPointPositions[i+1], t);
        }
        for(int j = 0; j < lerpedPositions.Length - 1; j++){
            curvePosition = Vector3.Lerp(lerpedPositions[j], lerpedPositions[j+1], t);
        }

        return curvePosition;
    }
    
}
