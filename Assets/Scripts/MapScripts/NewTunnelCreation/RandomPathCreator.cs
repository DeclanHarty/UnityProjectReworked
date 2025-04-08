using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class RandomPathCreator : MonoBehaviour
{
    private static Vector2[] DIRECTIONS = new Vector2[] {new Vector2(0,1), new Vector2(0,-1), new Vector2(1,0), new Vector2(-1,0)};
    private List<Vector2> vertices;

    public static Vector2 GetNextBestMove(Vector2 currentPos, Vector2 goal){
        Vector2 bestDirection = Vector2.zero;
        float closest = float.PositiveInfinity;

        foreach(Vector2 direction in DIRECTIONS){
            float distance = Vector2.Distance(currentPos + direction, goal);
            if(distance < closest){
                closest = distance;
                bestDirection = direction;
            }
        }

        return bestDirection;
    }

    public static Vector2 GetRandomNonBacktrackingNextMove(Vector2 currentPos, Vector2 lastMove){
        Vector2 chosenDirection;
        while(true){
            int directionIndex = Random.Range(0, DIRECTIONS.Count());
            chosenDirection = DIRECTIONS[directionIndex];
            Vector2 opposite = (new Vector2(-1 * lastMove.x, -1 * lastMove.y));
            if(chosenDirection != opposite){
                break;
            }
        }
        
        return chosenDirection;
    }

    public static bool IsBacktrack(Vector2 direction, Vector2 previousDirection){
        Vector2 opposite = (new Vector2(-1 * previousDirection.x, -1 * previousDirection.y));
        if(direction == opposite){
            return true;
        }else{
            return false;
        }
    }

    // Gets the next move in a path that is random but weighted towards the optimal solution
    public static Vector2 GetWeightedRandomMove(Vector2 currentPos, Vector2 goal, Vector2 previousDirection){
        float[] weights = new float[4];
        float total = 0f;
        for(int i = 0; i < 4; i++){
            float weight = CalculateMoveWeight(currentPos, goal, DIRECTIONS[i], previousDirection);
            Debug.Log(weight);
            total += weight;
            weights[i] = weight;
        }

        float rand_value = Random.Range(0f, total);
        float cursor = 0;
        for(int i = 0; i < 4; i++){
            cursor += weights[i];
            if(cursor >= rand_value){
                return DIRECTIONS[i];
            }
        }

        return GetNextBestMove(currentPos, goal);
    }

    public static float CalculateMoveWeight(Vector2 currentPos, Vector2 goal, Vector2 direction, Vector2 previousDirection){
        //float bestPossibleMoveDistance = 1f;
        float bestPossibleCloseness = Vector2.Distance(currentPos, goal) - 1f;
        if(IsBacktrack(direction, previousDirection)){
            return 0;
        }else{
            return 2.1f - (Vector2.Distance(currentPos + direction, goal) - bestPossibleCloseness);
        }

    }

    public static List<Vector2> GetBestLine(Vector2 start, Vector2 goal){
        List<Vector2> vertices = new List<Vector2>(new Vector2[]{start});
        int i = 0;
        while(vertices.Last() != goal && i < 100){
            vertices.Add(GetNextBestMove(vertices.Last(), goal) + vertices.Last());
            i++;
        }

        return vertices;
    }

    public static List<Vector2> GetRandomWalk(Vector2 start, int iterations){
        List<Vector2> vertices = new List<Vector2>(new Vector2[]{start});
        
        Vector2 previousDirection = Vector2.zero;

        for(int i = 0; i < iterations; i++){
            Vector2 newDirection = GetRandomNonBacktrackingNextMove(vertices.Last(), previousDirection);
            print(previousDirection);
            vertices.Add(newDirection + vertices.Last());
            previousDirection = newDirection;
        }

        return vertices;
    }

    public static List<Vector2> GetBiasedRandomWalk(Vector2 start, Vector2 goal){
        List<Vector2> vertices = new List<Vector2>(new Vector2[]{start});
        Vector2 lastDirection = Vector2.zero;
        int i = 0;
        while(vertices.Last() != goal && i < 100){
            Debug.Log("Move : " + (i + 1));
            Vector2 direction = GetWeightedRandomMove(vertices.Last(), goal, lastDirection);
            vertices.Add(direction + vertices.Last());
            lastDirection = direction;
            i++;
        }

        return vertices;
    }

    
}  
