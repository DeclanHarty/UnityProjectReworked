using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class UnweighetedAdjacencyListNode<T> {
    private T node;
    private List<T> neighbors;
    
    public UnweighetedAdjacencyListNode(T node, List<T> neighbors){
        this.node = node;
        this.neighbors = neighbors;
    }

    public T GetNode(){
        return node;
    }

    public List<T> GetNeighbors(){
        return neighbors;
    }

    public void AddNeighbor(T newNeighbor){
        if(!neighbors.Contains(newNeighbor)){
            neighbors.Add(newNeighbor);
        }
    }

    public void RemoveNeighbor(T neighbor){
        neighbors.Remove(neighbor);
    }
}