using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : CheckCast
{
    public Movement movement;

    public float castHeight;
    public float castWidth;

    public float distanceFromCenter;

    public LayerMask layerMask;


    public override bool IsObjectInCast()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(castWidth, castHeight), 0, Vector2.down, distanceFromCenter, layerMask);
        if(hit){
            return true;
        }

        return false;
    }
}
