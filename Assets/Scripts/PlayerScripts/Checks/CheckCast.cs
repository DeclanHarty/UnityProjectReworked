using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCast : MonoBehaviour
{
    public float castHeight;
    public float castWidth;
    public float distanceFromCenter;
    public Vector3 castDirection;

    public LayerMask layerMask;

    public bool showGizmo;
    public bool IsObjectInCast()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(castWidth, castHeight), 0, castDirection, distanceFromCenter, layerMask);
        if(hit){
            return true;
        }
        return false;
    }

    void OnDrawGizmos(){
        if (showGizmo) Gizmos.DrawCube(transform.position + distanceFromCenter * castDirection, new Vector3(castWidth, castHeight));
    }
}
