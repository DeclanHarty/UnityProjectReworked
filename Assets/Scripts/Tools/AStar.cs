using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public static class AStar<T>
{
    public static List<T> UWAStarPath(T start, T goal, Func<T, T, float> heuristic, UnweighetedAdjacencyList<T> graph){
        List<T> openSet = new List<T>(new T[] {start});

        int iteration = 0;

        Dictionary<T,T> cameFrom = new Dictionary<T, T>();
        
        Dictionary<T, float> gScore = new Dictionary<T, float>();
        foreach(T value in graph.GetNodeValues()){
            gScore[value] = float.PositiveInfinity;
        }

        gScore[start] = 0;

        Dictionary<T, float> fScore = new Dictionary<T, float>();
        foreach(T value in graph.GetNodeValues()){
            fScore[value] = float.PositiveInfinity;
        }

        fScore[start] = gScore[start] + heuristic(start, goal);

        while(openSet.Count != 0){
            iteration++;
            Debug.Log(iteration);
            T current = GetMinFScoreNode(openSet, fScore);

            if(current.Equals(goal)){
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            List<T> neighbors = graph.GetNeighbors(current);
            foreach(T neighbor in neighbors){
                float tentativeGScore = gScore[current] + 1;
                if(tentativeGScore < gScore[neighbor]){
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + heuristic(neighbor, goal);
                    if(!openSet.Contains(neighbor)){
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        throw new Exception("Goal not found in graph.");
    }

    private static List<T> ReconstructPath(Dictionary<T, T> cameFrom, T current){
        List<T> totalPath = new List<T>();
        totalPath.Add(current);
        while(cameFrom.ContainsKey(current)){
            current = cameFrom[current];
            totalPath.Add(current);
        }

        totalPath.Reverse();
        return totalPath;
    }

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
}
