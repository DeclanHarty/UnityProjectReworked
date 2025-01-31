using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NavGraph", order = 2)]
public class NavGraph : ScriptableObject
{
    private UnweighetedAdjacencyList<Vector2Int> graph = new UnweighetedAdjacencyList<Vector2Int>();
    public int graphSize = 0;


    public UnweighetedAdjacencyList<Vector2Int> GetNavGraph(){
        return graph;
    }

    public void SetNavGraph(UnweighetedAdjacencyList<Vector2Int> graph){
        this.graph = graph;
        this.graphSize = graph.Count();
    }
}
