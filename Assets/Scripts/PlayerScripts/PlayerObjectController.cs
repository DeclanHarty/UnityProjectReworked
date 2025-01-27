using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour
{
    public float horizontalScale;

    [ContextMenu("Flip Player")]
    public void FlipPlayer(){
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
    }

    public void SetDirectionRight(){
        transform.localScale = new Vector3(horizontalScale, transform.localScale.y);
    }

    public void SetDirectionLeft(){
        transform.localScale = new Vector3(-horizontalScale, transform.localScale.y);
    }
}
