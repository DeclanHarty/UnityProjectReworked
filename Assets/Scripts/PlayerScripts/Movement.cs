using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float movementAcceleration;
    public float maxSpeed;
    public bool isGrounded = true;

    public float groundedFriction;
    public float frictionCutoff;

    [Header("Gravity")]
    public float gravity;
    public float maxFallingSpeed;

    [Header("Jump Fields")]
    public float jumpVelocity;
    private bool jumpHeld;
    public float earlyReleaseModifier;
    

    [Header("Coyote Time")]
    private float timeSinceLeftGround;
    [Min(0)]
    public float coyoteTime;

    [Header("JumpBuffer")]
    private float timeSinceJumpPressed;
    [Min(0)]
    public float jumpBuffer;
    public bool tryingToJump;

    [Header("Hook States")]
    private bool playerHasHooked;
    public Vector2 hookPosition;
    public float hookRopeLength;
    public Vector2 hookVelocity;

    public float maxHookingSpeed;
    public float hookingAcceleration;

    [Header("Current Velocities")]
    public float verticalVelocity;
    public float horizontalVelocity;

    [Header("Hook Energies")]
    public float startingE;
    public float PotentialE;
    public float KineticE;

    [Header("Object References")]
    public GroundCheck groundCheck;
    public BonkCheck bonkCheck;
    public Rigidbody2D rb;

    public void Move(Vector2 input, bool _jumpHeld){
        jumpHeld = _jumpHeld;
        // if(!playerHasHooked){
            horizontalVelocity += movementAcceleration * Time.deltaTime * input.x;
        // }
        

        bool groundCheckValue = IsGrounded();
        HandleGrounding(groundCheckValue);

        if(isGrounded && horizontalVelocity != 0 && (input.x == 0 || Mathf.Sign(input.x) != Mathf.Sign(horizontalVelocity))){
            horizontalVelocity += groundedFriction * -1 * Mathf.Sign(horizontalVelocity) * Time.deltaTime;
            if(Mathf.Abs(horizontalVelocity) < frictionCutoff){
                horizontalVelocity = 0;
            }
        }
        horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxSpeed, maxSpeed);

        if(tryingToJump){
            HandleJumpStorage();
        }

        ApplyGravity();
        verticalVelocity = Mathf.Max(verticalVelocity, -maxFallingSpeed);

        if(HasBonked()){
            Debug.Log("Bonked");
            HandleBonk();
        }
        

        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    public void Jump(){
        if(isGrounded || timeSinceLeftGround < coyoteTime){
            timeSinceLeftGround += coyoteTime;
            verticalVelocity = jumpVelocity;
            tryingToJump = false;
        }else{
            tryingToJump = true;
            timeSinceJumpPressed = 0;
        }
    }

    private bool HasBonked(){
        return bonkCheck.IsObjectInCast();
    }

    private bool IsGrounded(){
        return groundCheck.IsObjectInCast();
    }

    private void HandleGrounding(bool groundCheckValue){
        if(!isGrounded && groundCheckValue){
            verticalVelocity = 0;
        }
        isGrounded = groundCheckValue;
        if(isGrounded){
            timeSinceLeftGround = 0;
        }else{
            timeSinceLeftGround += Time.deltaTime;
        }
    }

    private void HandleBonk(){
        if(!isGrounded && verticalVelocity > 0){
            verticalVelocity = 0;
        }
    }

    private void HandleJumpStorage(){
        if(isGrounded && timeSinceJumpPressed < jumpBuffer){
            Jump();
        }else if(timeSinceJumpPressed < jumpBuffer){
            timeSinceJumpPressed += Time.deltaTime;
        }else{
            timeSinceJumpPressed = 0.0f;
            tryingToJump = false;
        }
    }

    private void ApplyGravity(){
        if (!isGrounded && !playerHasHooked) { 
            if(verticalVelocity > -maxFallingSpeed){
                if(!jumpHeld || verticalVelocity <= 0){
                    
                    verticalVelocity -= gravity * earlyReleaseModifier * Time.deltaTime;
                    return;
                }
                verticalVelocity -= gravity * Time.deltaTime;
            }
        }
    }




    public Vector2 GetPlayerPosition(){
        return transform.position;
    }

}