using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMoveSpeed;
    public float distanceCoefficent;
    public void SetPostion(Vector2 position){
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    public void MoveCamera(Vector2 toPosition){
        Vector3 toPositionV3 = new Vector3(toPosition.x, toPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, toPositionV3, cameraMoveSpeed * Time.deltaTime * Mathf.Pow(distanceCoefficent * Vector3.Distance(transform.position, toPosition), 2));
    }
}
