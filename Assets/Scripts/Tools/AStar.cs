using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using Unity.Jobs;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class AStar<T> : IPathfinder<T>
{
    public static List<T> UWAStarPath(T start, T goal, Func<T, T, float> heuristic, UnweighetedAdjacencyList<T> graph, int maxDepth = int.MaxValue){
        DateTime before = DateTime.Now;
        FastPriorityQueue<GenericFastPriorityQueueNode<T>> openSet = new FastPriorityQueue<GenericFastPriorityQueueNode<T>>(100000);


        Dictionary<T,T> cameFrom = new Dictionary<T, T>();
        
        Dictionary<T, float> gScore = new Dictionary<T, float>();
        foreach(T value in graph.GetNodeValues()){
            gScore[value] = float.PositiveInfinity;
        }

        gScore[start] = 0;
        float fScore = gScore[start] + heuristic(start, goal);

        int currentDepth = 0;
        T lastCameFrom = default(T);

        GenericFastPriorityQueueNode<T> startNode = new GenericFastPriorityQueueNode<T>();
        startNode.value = start;

        
        openSet.Enqueue(startNode, fScore);
        
        

        while(openSet.Count != 0){

            T current = openSet.Dequeue().value;

            if(current.Equals(goal) || currentDepth >= maxDepth){
                DateTime after = DateTime.Now;
                TimeSpan duration = after.Subtract(before);
                Debug.Log("NavGraph Duration in milliseconds: " + duration.Milliseconds);
                return IPathfinder<T>.ReconstructPath(cameFrom, current);
            }

            List<T> neighbors = graph.GetNeighbors(current);
            foreach(T neighbor in neighbors){
                float tentativeGScore = gScore[current] + 1;
                if(tentativeGScore < gScore[neighbor]){
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore = tentativeGScore + heuristic(neighbor, goal);
                    GenericFastPriorityQueueNode<T> neighborNode = new GenericFastPriorityQueueNode<T>();
                    neighborNode.value = neighbor;
                    if(openSet.Contains(neighborNode)){
                        openSet.UpdatePriority(neighborNode, fScore);
                    }else{
                        openSet.Enqueue(neighborNode, fScore);
                    }
                }
            }
            if(!cameFrom.ContainsKey(current) || !cameFrom[current].Equals(lastCameFrom)){
                currentDepth++;
                if(cameFrom.ContainsKey(current)){
                    lastCameFrom = cameFrom[current];
                }
            }
            
        }

        throw new Exception("Goal not found in graph.");
    }

    // private static List<T> ReconstructPath(Dictionary<T, T> cameFrom, T current){
    //     List<T> totalPath = new List<T>();
    //     totalPath.Add(current);
    //     while(cameFrom.ContainsKey(current)){
    //         current = cameFrom[current];
    //         totalPath.Insert(0, current);
    //     }
    //     return totalPath;
    // }

    private static T GetMinFScoreNode(List<T> queue, Dictionary<T, float> fScoreMap){
        float minFScore = float.MaxValue;
        T valueWithLowestFScore = default(T);

        foreach(T value in queue){
            float currentFScore = fScoreMap[value];
            if(currentFScore < minFScore){
                minFScore = currentFScore;
                valueWithLowestFScore = value;
            }
        }
        return valueWithLowestFScore;
    }

    public static float DirectDistanceHeuristic(Vector2Int pos, Vector2Int goal){
        return (goal - pos).magnitude;
    }

    public static float DistanceSquaredHeuristic(Vector2Int pos, Vector2Int goal){
        return Mathf.Pow((goal - pos).magnitude, 2);
    }

    public static float ManhattanDistance(Vector2Int pos, Vector2Int goal){
        return Mathf.Abs(pos.x - goal.x) + Mathf.Abs(pos.y - goal.y);
    }
}

public class GenericFastPriorityQueueNode<T> : FastPriorityQueueNode{
    public T value {get; set;}
}

public class AStarJob<T> : IJob
{
    public T start; 
    public T goal;
    public Func<T, T, float> heuristic; 
    public UnweighetedAdjacencyList<T> graph;
    public int maxDepth = int.MaxValue;

    public List<T> path;
    public void Execute()
    {
        path = AStar<T>.UWAStarPath(start, goal, heuristic, graph, maxDepth);
    }

      
}
