using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    void Break(){
        Destroy(gameObject);
    }

    void OnMouseDown(){
        Break();
    }



}
