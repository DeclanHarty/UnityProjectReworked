using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathTesting : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Vector2 start;
    public Vector2 goal;
    // Start is called before the first frame update
    void Start()
    {
        List<Vector2> verts = RandomPathCreator.GetBiasedRandomWalk(start, goal);
        lineRenderer.positionCount = verts.Count;
        
        Vector3[] verts3 = new Vector3[verts.Count];
        int i = 0;
        foreach(Vector2 vert in verts){
            verts3[i] = vert;
            i++;
        }

        lineRenderer.SetPositions(verts3);
    }
}
