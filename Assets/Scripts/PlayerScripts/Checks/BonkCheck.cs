using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkCheck : CheckCast
{
    
    public Movement movement;

    public float castHeight;
    public float castWidth;

    public float distanceFromCenter;

    public LayerMask layerMask;

    public override bool IsObjectInCast()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(castWidth, castHeight), 0, Vector2.up, distanceFromCenter, layerMask);
        if(hit){
            return true;
        }
        return false;
    }

    void OnDrawGizmos(){
        Gizmos.DrawCube(transform.position + distanceFromCenter * Vector3.up, new Vector3(castWidth, castHeight));
    }
}
