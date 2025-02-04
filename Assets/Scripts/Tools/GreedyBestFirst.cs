using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Priority_Queue;
using Unity.VisualScripting;
using UnityEngine;

public class GreedyBestFirst<T> : IPathfinder<T>
{
    // public static List<T> GBFSPath(T start, T goal, Func<T, T, float> heuristic, UnweighetedAdjacencyList<T> graph, int maxDepth = int.MaxValue){
    //     DateTime before = DateTime.Now;
    //     DateTime after;
    //     TimeSpan duration;
    //     FastPriorityQueue<GenericFastPriorityQueueNode<T>> openSet = new FastPriorityQueue<GenericFastPriorityQueueNode<T>>(100000);


    //     Dictionary<T,T> cameFrom = new Dictionary<T, T>();
    //     HashSet<T> visited = new HashSet<T>();


    //     int currentDepth = 0;
    //     T lastCameFrom = default(T);

    //     GenericFastPriorityQueueNode<T> startNode = new GenericFastPriorityQueueNode<T>();
    //     startNode.value = start;

        
    //     openSet.Enqueue(startNode, heuristic(start, goal));
        
        

    //     // while(openSet.Count != 0){
    //     for(int i = 0; i < 40; i++){
    //         T current = openSet.Dequeue().value;

    //         if(current.Equals(goal) || currentDepth >= maxDepth){
    //             after = DateTime.Now;
    //             duration = after.Subtract(before);
    //             Debug.Log("GBFS Duration in milliseconds: " + duration.Milliseconds);
    //             return IPathfinder<T>.ReconstructPath(cameFrom, current);
    //         }

    //         List<T> neighbors = graph.GetNeighbors(current);
    //         foreach(T neighbor in neighbors){
    //             float hValue = heuristic(neighbor, goal);
    //             cameFrom[neighbor] = current;

    //             if(!visited.Contains(neighbor)){
    //                 GenericFastPriorityQueueNode<T> neighborNode = new GenericFastPriorityQueueNode<T>();
    //                 neighborNode.value = neighbor;
    //                 openSet.Enqueue(neighborNode, hValue);
    //                 visited.Add(neighbor);
    //             }
    //         }
            
    //         if(!cameFrom.ContainsKey(current) || !cameFrom[current].Equals(lastCameFrom)){
    //             currentDepth++;
    //             if(cameFrom.ContainsKey(current)){
    //                 lastCameFrom = cameFrom[current];
    //             }
    //         }
            
    //     }

    //     after = DateTime.Now;
    //     duration = after.Subtract(before);
    //     Debug.Log("GBFS Duration in milliseconds: " + duration.Milliseconds);

    //     throw new Exception("Goal not found in graph.");
    // }

    public static List<Vector2Int> GBFSPath(Vector2Int start, Vector2Int goal, Func<Vector2Int, Vector2Int, float> heuristic, UnweighetedAdjacencyList<Vector2Int> graph, int maxDepth = int.MaxValue){
        DateTime before = DateTime.Now;
        DateTime after;
        TimeSpan duration;

        Dictionary<Vector2Int,Vector2Int> cameFrom = new Dictionary<Vector2Int,Vector2Int>();
        FastPriorityQueue<PathNode> queue = new FastPriorityQueue<PathNode>(1000000);

        PathNode startNode = new PathNode(start);

        queue.Enqueue(startNode, heuristic(start, goal));

        // List<T> queue = new List<T>();
        // queue.Add(start);

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        visited.Add(start);

        while(queue.Count != 0){
            Vector2Int current = queue.Dequeue().value;

            List<Vector2Int> neighbors = graph.GetNeighbors(current);

            foreach(Vector2Int neighbor in neighbors){
                if(!visited.Contains(neighbor)){
                    cameFrom[neighbor] = current;
                    visited.Add(neighbor);
                    if(neighbor.Equals(goal)){
                        after = DateTime.Now;
                        duration = after.Subtract(before);
                        Debug.Log("Best First Duration in milliseconds: " + duration.Milliseconds);
                        return ReconstructPath(cameFrom, current);
                    }
                    else{
                        PathNode neighborNode = new PathNode(neighbor);
                        queue.Enqueue(neighborNode, heuristic(neighbor, goal));
                    }
                }
            }
        }
        after = DateTime.Now;
        duration = after.Subtract(before);
        Debug.Log("Best First Duration in milliseconds: " + duration.Milliseconds);
        throw new Exception("Goal not found in graph.");
    }

    static T RemoveMinItem(List<T> queue, T goal, Func<T, T, float> heuristic){
        float minFScore = float.MaxValue;
        T valueWithLowestFScore = default(T);

        foreach(T value in queue){
            float currentFScore = heuristic(value, goal);
            if(currentFScore < minFScore){
                minFScore = currentFScore;
                valueWithLowestFScore = value;
            }
        }
        queue.Remove(valueWithLowestFScore);
        return valueWithLowestFScore;
    }

    static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current){
        List<Vector2Int> totalPath = new List<Vector2Int>();
        totalPath.Add(current);
        while(cameFrom.ContainsKey(current)){
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

}
