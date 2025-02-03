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
}
