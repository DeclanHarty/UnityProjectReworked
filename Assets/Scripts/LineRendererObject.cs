using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererObject : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Vector3 startPos { set; get;}
    public Vector3 endPos { set; get;}

    public void EnableLine(Vector3[] vertices, Color color){

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        
        lineRenderer.positionCount = vertices.Length;
        lineRenderer.SetPositions(vertices);
    }
}
