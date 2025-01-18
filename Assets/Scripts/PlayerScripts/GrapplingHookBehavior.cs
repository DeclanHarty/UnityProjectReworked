using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GrapplingHookBehavior : MonoBehaviour
{
    public float maxInitialSpeed;
    public float minInitialSpeed;
    public Vector2 initialDirection;

    public float gravity;
    public float spawnDistanceFromPlayer;

    public bool isStuck;

    public Rigidbody2D rb;


    public void UpdateHook(){
        if(isStuck){
            return;
        }
        rb.velocity -= new Vector2(0, gravity * Time.deltaTime);
    }

    void Start(){
        //ActivateAndDirect(initialDirection, initialSpeed);
    }

    public void ActivateAndDirect(Vector2 initialDirection, float percentageOfMaxSpeed){
        ResetHook();
        transform.position = transform.parent.position + (Vector3)(initialDirection * spawnDistanceFromPlayer);
        gameObject.SetActive(true);
        rb.velocity = initialDirection * Mathf.Lerp(minInitialSpeed, maxInitialSpeed, percentageOfMaxSpeed);
    }

    public void ResetHook(){
        transform.position = transform.parent.position;
        isStuck = false;
        rb.constraints = RigidbodyConstraints2D.None;
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col){
        //Debug.Log("Collision");
        if(col.gameObject.tag.Equals("Ground")){
            isStuck = true;
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public Vector2 GetHookPosition(){
        return transform.position;
    }


}
