using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererObject : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Vector3 startPos { set; get;}
    public Vector3 endPos { set; get;}

    public void EnableLine(Vector3[] positions, Color color){

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        startPos = positions[0];
        endPos = positions[positions.Length - 1];
        
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    public void SetPositions(Vector3[] positions){
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
}
