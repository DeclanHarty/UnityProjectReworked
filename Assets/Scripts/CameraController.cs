using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void SetPostion(Vector2 position){
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
