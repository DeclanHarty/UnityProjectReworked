using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;

    public Rigidbody2D rb;


    public void Move(Vector2 input){
        rb.velocity = input.normalized * speed;
    }
}
