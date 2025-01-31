using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class UnweighetedAdjacencyListNode<T> {
    private T value;
    private List<T> neighbors;
    
    public UnweighetedAdjacencyListNode(T value, List<T> neighbors){
        this.value = value;
        this.neighbors = neighbors;
    }

    public T GetValue(){
        return value;
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