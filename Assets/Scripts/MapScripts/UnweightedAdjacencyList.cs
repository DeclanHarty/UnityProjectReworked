using System;
using System.Collections.Generic;
using System.Linq;

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
            if(node.GetValue().Equals(nodeValue)){
                list.Remove(node);
                return;
            }
        }
    }

    public List<T> GetNeighbors(T nodeValue){
        foreach(UnweighetedAdjacencyListNode<T> node in list){
            if(node.GetValue().Equals(nodeValue)){
                return node.GetNeighbors();
            }
        }

        throw new ArgumentException("Node was not found in the graph");
    }

    public List<T> GetNodeValues(){
        List<T> nodeValues = new List<T>();
        foreach(UnweighetedAdjacencyListNode<T> node in list){
            nodeValues.Add(node.GetValue());
        }

        return nodeValues;
    }

    public override string ToString()
    {
        string totalString = "[";
        for(int nodeIndex = 0; nodeIndex < list.Count; nodeIndex++){
            UnweighetedAdjacencyListNode<T> node = list.ElementAt(nodeIndex);
            string nodeString = node.GetValue().ToString() + " : {";
            List<T> neighbors = node.GetNeighbors();
            for(int neighborIndex = 0; neighborIndex < neighbors.Count; neighborIndex++){
                T neighbor = neighbors[neighborIndex];
                if(neighborIndex == 0){
                    nodeString += neighbor;
                }else{
                    nodeString += ", " + neighbor;
                }
            }

            nodeString += "}";

            if(nodeIndex == 0){
                totalString += nodeString;
            }else{
                totalString += '\n' + nodeString;
            }
        }

        totalString += "]";
        return totalString;
    }

    public int Count(){
        return list.Count;
    }
}