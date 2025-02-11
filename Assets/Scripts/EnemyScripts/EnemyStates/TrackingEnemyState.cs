using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TrackingEnemyState : EnemyState
{
    public bool HasLineOfSight(Vector2 playerPosition){
        // Checks to see if enemy has line of sight with player
        RaycastHit2D enemyLosCheck = Physics2D.Raycast(enemy.rb.position, (playerPosition - enemy.rb.position).normalized, enemy.visionDistance, enemy.playerMask); 

        // If enemy does have LOS then the enemy moves directly to the player and clears last seen positions
        if(enemyLosCheck && enemyLosCheck.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
            enemy.lastKnownPlayerPosition = playerPosition;
            enemy.lastSeenPositions.Clear();
            return true;
        }else{
            return false;
        }
    }
    
    public void TrackPath(Vector2 playerPosition){
        //Check if enemy has just lost LOS if so add last known position to the last seen queue
        if(enemy.lastSeenPositions.Count == 0){
            enemy.lastSeenPositions.Add(enemy.lastKnownPlayerPosition);
        }

        // Does a raycast to determine if the enemy's last LOS Position has a direct path to the player
        RaycastHit2D lastSeenPositionCheck = Physics2D.Raycast(enemy.lastSeenPositions.Last(), (playerPosition - enemy.lastSeenPositions.Last()).normalized, enemy.visionDistance, enemy.playerMask);

        // If true updates last known position
        if(lastSeenPositionCheck && lastSeenPositionCheck.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
            enemy.lastKnownPlayerPosition = playerPosition;
        // If the last seen pos does not have a direct LOS and player has not yet escaped the enemy, 
        // the last known position is appended to lastSeenPositions queue
        }else if(!enemy.playerEscaped){
            enemy.lastSeenPositions.Add(enemy.lastKnownPlayerPosition);
            // If the distance between the first LSP in the LSP queue and the player is greater than maxPsycheDistance then 
            // the player has escaped the enemy and positions will no longer be appended to the LSP queue
            if(Vector2.Distance(enemy.lastSeenPositions[0], playerPosition) >enemy.maxPsycheDistance){
                enemy.playerEscaped = true;
            }
        }
    }
}
