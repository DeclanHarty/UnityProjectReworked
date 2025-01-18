using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineLineVisualizer : SplineObserver
{
    public SplineCurve splineCurve;
    public GameObject linePrefab;
    public GameObject lineObject;
    public Color lineColor;

    public void CreateLineObject(){
        if(lineObject == null){
            lineObject = Instantiate(linePrefab, transform);
            lineObject.GetComponent<LineRendererObject>().EnableLine(new Vector3[2], lineColor);
        }
    }

    public void SetPositionsOfLine(Vector3[] positions){
        lineObject.GetComponent<LineRendererObject>().SetPositions(positions);
    }

    public override void NotifyUpdate()
    {
        CreateLineObject();
        SetPositionsOfLine(splineCurve.GetCurvePositions());
    }

    public Vector3[] GetControlPointPositions(){
        return splineCurve.controlPointPositions;
    }
}
