using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfinder<T>
{
    static List<T> ReconstructPath(Dictionary<T, T> cameFrom, T current){
        List<T> totalPath = new List<T>();
        totalPath.Add(current);
        while(cameFrom.ContainsKey(current)){
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    static T GetMinFScoreNode(List<T> queue, Dictionary<T, float> fScoreMap){
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
}
