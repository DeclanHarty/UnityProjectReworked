using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;

    public void MovePlayer(Vector2 input){
        player.GetComponent<Movement>().Move(input);
    }

    public Vector2 GetPlayerPosition(){
        return player.transform.position;
    }
}
