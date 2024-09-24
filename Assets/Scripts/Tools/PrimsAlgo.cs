using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using Unity.Collections;
using UnityEngine;

public class PrimsAlgo
{
    public static double[,] CreateWeightedAdjacencyMatrix(IPoint[] points, IEdge[] edges){
        double[,] adjacencyMatrix = new double[points.Length, points.Length];

        Dictionary<IPoint, int> pointIndexDictionary = new Dictionary<IPoint, int>();

        for(int pointIndex = 0; pointIndex < points.Length; pointIndex++){
            pointIndexDictionary.Add(points[pointIndex], pointIndex);
        }

        for(int pointIndex = 0; pointIndex < points.Length; pointIndex++){
            for(int edgeIndex = 0; edgeIndex < edges.Length; edgeIndex++){
                if(edges[edgeIndex].P == points[pointIndex]){
                    IPoint startPoint = points[pointIndex];
                    IPoint endPoint = edges[edgeIndex].Q;

                    double distance = Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2));

                    adjacencyMatrix[pointIndex, pointIndexDictionary[endPoint]] = distance;
                    adjacencyMatrix[pointIndexDictionary[endPoint], pointIndex] = distance;
                }
            }
        }

        return adjacencyMatrix;
    }

    public static int[,] PrimMST(double[,] adjacencyGraph){
        int[,] treeAdjacencyGraph = new int[adjacencyGraph.GetLength(0), adjacencyGraph.GetLength(1)];
        HashSet<int> visitedPoints = new HashSet<int>();
        int algorithmStartingPoint = 0; // UnityEngine.Random.Range(0, adjacencyGraph.GetLength(0));
        visitedPoints.Add(algorithmStartingPoint);

        while(true){
            
            int shortestEdgeStartPoint = 0;
            int shortestEdgeEndPoint = 0;
            double currentShortestDistance = double.MaxValue;
            foreach(int point in visitedPoints){
                for(int endPointIndex = 0; endPointIndex < adjacencyGraph.GetLength(1); endPointIndex++){
                    if(adjacencyGraph[point, endPointIndex] == 0){
                        continue;
                    }

                    if(adjacencyGraph[point, endPointIndex] < currentShortestDistance && !visitedPoints.Contains(endPointIndex)){
                        shortestEdgeStartPoint = point;
                        shortestEdgeEndPoint = endPointIndex;
                        currentShortestDistance = adjacencyGraph[point, endPointIndex];
                    }
                }
            }

            if(shortestEdgeStartPoint == 0 && shortestEdgeEndPoint == 0){
                break;
            }

            treeAdjacencyGraph[shortestEdgeStartPoint, shortestEdgeEndPoint] = 1;
            treeAdjacencyGraph[shortestEdgeEndPoint, shortestEdgeStartPoint] = 1;
            visitedPoints.Add(shortestEdgeEndPoint);
        }
    
        return treeAdjacencyGraph;
    }
}
