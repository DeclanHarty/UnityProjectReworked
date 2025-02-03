using System;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using Unity.VisualScripting;
using UnityEngine;

public class GreedyBestFirst<T> : IPathfinder<T>
{
    public static List<T> GBFSPath(T start, T goal, Func<T, T, float> heuristic, UnweighetedAdjacencyList<T> graph, int maxDepth = int.MaxValue){
        DateTime before = DateTime.Now;
        FastPriorityQueue<GenericFastPriorityQueueNode<T>> openSet = new FastPriorityQueue<GenericFastPriorityQueueNode<T>>(100000);


        Dictionary<T,T> cameFrom = new Dictionary<T, T>();



        int currentDepth = 0;
        T lastCameFrom = default(T);

        GenericFastPriorityQueueNode<T> startNode = new GenericFastPriorityQueueNode<T>();
        startNode.value = start;

        
        openSet.Enqueue(startNode, heuristic(start, goal));
        
        

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
                float hValue = heuristic(neighbor, goal);
                cameFrom[neighbor] = current;
                GenericFastPriorityQueueNode<T> neighborNode = new GenericFastPriorityQueueNode<T>();
                neighborNode.value = neighbor;
                openSet.Enqueue(neighborNode, hValue);
                }
            }
            if(!cameFrom.ContainsKey(current) || !cameFrom[current].Equals(lastCameFrom)){
                currentDepth++;
                if(cameFrom.ContainsKey(current)){
                    lastCameFrom = cameFrom[current];
                }
            }
            
        }
}

}
