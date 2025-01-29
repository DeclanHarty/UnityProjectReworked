using System;
using System.Collections.Generic;

public class UnweighetedAdjacencyList<T> {
    private List<UnweighetedAdjacencyListNode<T>> list;

    public UnweighetedAdjacencyList(){
        list = new List<UnweighetedAdjacencyListNode<T>>();
    }

    public void AddNode(UnweighetedAdjacencyListNode<T> node){
        list.Add(node);
    }

    public void AddNode(T node){
        list.Add(new UnweighetedAdjacencyListNode<T>(node, new List<T>()));
    }

    public void RemoveNode(T nodeValue){
        foreach(UnweighetedAdjacencyListNode<T> node in list){
            if(node.GetNode().Equals(nodeValue)){
                list.Remove(node);
                return;
            }
        }
    }

    public List<T> GetNeighbors(T nodeValue){
        foreach(UnweighetedAdjacencyListNode<T> node in list){
            if(node.GetNode().Equals(nodeValue)){
                return node.GetNeighbors();
            }
        }

        throw new ArgumentException("Node was not found in the graph");
    }
}