using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public float maxHookDistance;
    public float hookingSpeed;
    public bool playerReachedHook;
    
    public LayerMask layerMask;

    public bool hooked;
    public Vector2 hookPosition;

    public Rigidbody2D rb;


    public bool ThrowOutHook(Vector2 direction){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxHookDistance, layerMask);
        if(hit){
            hooked = true;
            hookPosition = hit.point;
            return true;
        }else{
            hooked = false;
            hookPosition = Vector2.zero;
            return false;
        }
    }

    public void MoveTowardsHook(){
        rb.velocity = GetHookDirection() * hookingSpeed;
    }

    public void ReleaseHook(){
        hooked = false;
    }

    public void OnDrawGizmos(){
        if(hooked) Gizmos.DrawWireSphere(hookPosition, .1f);
    }

    public Vector2 GetHookDirection(){
        return (hookPosition - (Vector2)transform.position).normalized;
    }
}



