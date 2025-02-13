using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Moving")]
    public float movementAcceleration;
    public float maxSpeed;
    public float groundedFriction;
    public float airborneFrictionMultiplier;
    public float frictionCutoff;
    // 1 means facing right, -1 means facing left
    public int playerDirection;
    //
    public bool switchingDirection;

    [Header("Check Values")]
    public bool isGrounded = true;
    public bool touchingWall;
    public bool touchingLedge;

    public float ledgeHeight;

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

    [Header("Jump Buffer")]
    private float timeSinceJumpPressed;
    [Min(0)]
    public float jumpBuffer;
    public bool tryingToJump;

    [Header("Current Velocities")]
    public float verticalVelocity;
    public float horizontalVelocity;

    [Header("Object References")]
    public CheckCast groundCheck;
    public CheckCast bonkCheck;
    public CheckCast wallCheck;
    public CheckCast stepupCheck;
    public Rigidbody2D rb;

    public void Move(Vector2 input, bool _jumpHeld){
        jumpHeld = _jumpHeld;
        // if(!playerHasHooked){
            horizontalVelocity += movementAcceleration * Time.deltaTime * input.x;
        // }

        if(input.x != 0){
            int newDirection = Math.Sign(input.x);
            if(newDirection != playerDirection){
                playerDirection = newDirection;
                switchingDirection = true;
            }
            
        }
        

        bool groundCheckValue = IsGrounded();
        HandleGrounding(groundCheckValue);

        touchingWall = TouchingWall();
        touchingLedge = TouchingLedge();

        if(!touchingWall){
            if(horizontalVelocity != 0 && (input.x == 0 || Mathf.Sign(input.x) != Mathf.Sign(horizontalVelocity))){
                float frictionEffect = groundedFriction * -1 * Mathf.Sign(horizontalVelocity) * Mathf.Abs(horizontalVelocity) * Time.deltaTime;
                if(!isGrounded){
                    frictionEffect *= airborneFrictionMultiplier;   
                }
                horizontalVelocity += frictionEffect;
                if(Mathf.Abs(horizontalVelocity) < frictionCutoff){
                    horizontalVelocity = 0;
                }
            }
            horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxSpeed, maxSpeed);
        }

        if(touchingWall || (!isGrounded && touchingLedge)){
            horizontalVelocity = 0;
        }

        if(tryingToJump){
            HandleJumpStorage();
        }

        ApplyGravity();
        verticalVelocity = Mathf.Max(verticalVelocity, -maxFallingSpeed);

        if(HasBonked()){
            HandleBonk();
        }

        if(isGrounded && touchingLedge && !touchingWall && input.x != 0 && !switchingDirection){
            rb.MovePosition(new Vector2(rb.position.x + .1f * (float)playerDirection, rb.position.y + ledgeHeight));
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

    public void Jump(bool ignoreGround){
        if(ignoreGround || isGrounded || timeSinceLeftGround < coyoteTime){
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

    private bool TouchingWall(){
        return wallCheck.IsObjectInCast();
    }

    private bool TouchingLedge(){
        return stepupCheck.IsObjectInCast();
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
        if (!isGrounded) { 
            if(verticalVelocity > -maxFallingSpeed){
                if(!jumpHeld || verticalVelocity <= 0){
                    verticalVelocity -= gravity * earlyReleaseModifier * Time.deltaTime;
                    return;
                }
                verticalVelocity -= gravity * Time.deltaTime;
            }
        }
    }

    public   void SwitchedDirection(){
        switchingDirection = false;
    }

    public void SetPlayerPosition(Vector2 position){
        rb.position = position;
    }

    public Vector2 GetPlayerPosition(){
        return transform.position;
    }

    public int GetPlayerDirection(){
        return playerDirection;
    }

    public bool IsSwitchingDirection(){
        return switchingDirection;
    }

    public void ClearAllVelocity(){
        horizontalVelocity = 0;
        verticalVelocity = 0;
        rb.velocity = Vector2.zero;
    }

    public void SetVelocity(Vector2 velocity){
        horizontalVelocity = velocity.x;
        verticalVelocity = velocity.y;
        rb.velocity = velocity;
    }

}