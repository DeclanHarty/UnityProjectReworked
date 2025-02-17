using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TunnelCreator : MonoBehaviour
{
    public static GameObject dot;

    public static List<Vector2> TunnelRaster(Vector2 startPos, Vector2 endPos, int thickness){
        GameObject dot = Resources.Load<GameObject>("Assets/Prefabs/DotSpritePrefab.prefab");
        List<Vector2> raster = new List<Vector2>(); // Initialize the tunnel raster

        float tunnelLength = (endPos - startPos).magnitude;
        int numOfSegments = (int)Mathf.Floor(tunnelLength/10);
        if(numOfSegments <= 0){
            return raster;
        }

        Vector3[] controlPointVertices = NoiseySpline.CreateNoiseySplineControlPositionsWith1D_Displacement(startPos, endPos, numOfSegments, .1f, 10);
        Vector3[] baseLine = SplineCurve.CreateSpline(controlPointVertices, 2); // gets the baseline for the curve

        Vector3[] baseLineNormals = new Vector3[baseLine.Length - 1]; // initializes the normals array with a length of baseLine - 1
        Vector3[] midpointPositions = new Vector3[baseLine.Length - 1]; // initializes midpointPositions for the baseline

        //Calculates normals of the base line
        for(int i = 0; i < baseLine.Length - 1; i++){
            baseLineNormals[i] = Quaternion.AngleAxis(90, Vector3.forward) * (baseLine[i+1] - baseLine[i]).normalized; // Rotates the slope by 90 to get the normal
            midpointPositions[i] = (baseLine[i+1] - baseLine[i]) / 2 + baseLine[i]; // Calculates the midpoint to be adjusted acccording to the normal
        }

        Vector3[] lineVerticesStartToEnd = new Vector3[baseLineNormals.Length];
        Vector3[] lineVerticesEndToStart = new Vector3[baseLineNormals.Length];

        // Set all values in both vertice arrays
        for(int i = 0; i < baseLineNormals.Length; i++){
            lineVerticesStartToEnd[i] = midpointPositions[i] - Mathf.Floor(thickness / 2f) * baseLineNormals[i]; // Start to End is subtracted by the floor of thickness / 2 applied to each base
            lineVerticesEndToStart[i] = midpointPositions[i] + Mathf.Ceil(thickness / 2f) * baseLineNormals[i];  // Start to End is subtracted by the ceiling of thickness / 2 applied to each base
        }

        // Reserse End to Start
        Array.Reverse(lineVerticesEndToStart);

        // Copy over both halves of the line
        Vector3[] linePolygonV3 = new Vector3[midpointPositions.Length * 2];
        Array.Copy(lineVerticesStartToEnd, linePolygonV3, lineVerticesStartToEnd.Length);
        Array.Copy(lineVerticesEndToStart, 0, linePolygonV3, lineVerticesStartToEnd.Length, lineVerticesEndToStart.Length);
        linePolygonV3 = IntersectionRemover(linePolygonV3);

        Vector2Int[] linePolygon = Scanline.ConvertFloatPolygonToIntPolygon(Scanline.ConvertVector3ArrayToVector2Array(linePolygonV3));

        raster = Scanline.PolygonFill(linePolygon);

        // foreach(Vector2Int vec in linePolygon){
        //     raster.Add(new Vector2(vec.x, vec.y));
        // }

        return raster;
    }

    public static List<Vector2> TunnelRaster(Vector2 startPos, Vector2 endPos, int thickness, Vector3[] controlPointVertices){
        GameObject dot = Resources.Load<GameObject>("Assets/Prefabs/DotSpritePrefab.prefab");
        List<Vector2> raster = new List<Vector2>(); // Initialize the tunnel raster

        float tunnelLength = (endPos - startPos).magnitude;
        int numOfSegments = (int)Mathf.Floor(tunnelLength/10);

        Vector3[] baseLine = SplineCurve.CreateSpline(controlPointVertices, 2); // gets the baseline for the curve

        Vector3[] baseLineNormals = new Vector3[baseLine.Length - 1]; // initializes the normals array with a length of baseLine - 1
        Vector3[] midpointPositions = new Vector3[baseLine.Length - 1]; // initializes midpointPositions for the baseline

        //Calculates normals of the base line
        for(int i = 0; i < baseLine.Length - 1; i++){
            baseLineNormals[i] = Quaternion.AngleAxis(90, Vector3.forward) * (baseLine[i+1] - baseLine[i]).normalized; // Rotates the slope by 90 to get the normal
            midpointPositions[i] = (baseLine[i+1] - baseLine[i]) / 2 + baseLine[i]; // Calculates the midpoint to be adjusted acccording to the normal
        }

        Vector3[] lineVerticesStartToEnd = new Vector3[baseLineNormals.Length];
        Vector3[] lineVerticesEndToStart = new Vector3[baseLineNormals.Length];

        // Set all values in both vertice arrays
        for(int i = 0; i < baseLineNormals.Length; i++){
            lineVerticesStartToEnd[i] = midpointPositions[i] - Mathf.Floor(thickness / 2f) * baseLineNormals[i]; // Start to End is subtracted by the floor of thickness / 2 applied to each base
            lineVerticesEndToStart[i] = midpointPositions[i] + Mathf.Ceil(thickness / 2f) * baseLineNormals[i];  // Start to End is subtracted by the ceiling of thickness / 2 applied to each base
        }

        // Reserse End to Start
        Array.Reverse(lineVerticesEndToStart);

        // Copy over both halves of the line
        Vector3[] linePolygonV3 = new Vector3[midpointPositions.Length * 2];
        Array.Copy(lineVerticesStartToEnd, linePolygonV3, lineVerticesStartToEnd.Length);
        Array.Copy(lineVerticesEndToStart, 0, linePolygonV3, lineVerticesStartToEnd.Length, lineVerticesEndToStart.Length);
        linePolygonV3 = IntersectionRemover(linePolygonV3);

        Vector2Int[] linePolygon = Scanline.ConvertFloatPolygonToIntPolygon(Scanline.ConvertVector3ArrayToVector2Array(linePolygonV3));

        raster = Scanline.PolygonFill(linePolygon);

        // foreach(Vector2Int vec in linePolygon){
        //     raster.Add(new Vector2(vec.x, vec.y));
        // }

        return raster;
    }

    public static Vector3[] IntersectionRemover(Vector3[] line){
        List<int> intersectIndex = new List<int>();
        intersectIndex.Add(0);

        List<Vector3> fixedLine = new List<Vector3>();

        for(int i = 0; i < line.Length - 1; i++){
            for(int j = i + 1; j < line.Length - 1; j++){
                // Get the first edge
                Vector3 p1 = line[i];
                Vector3 p2 = line[i + 1];

                // Get second edge
                Vector3 p3 = line[j];
                Vector3 p4 = line[j + 1];

                // Test interscetion
                bool isIntersecting = IntersectionTest(p1,p2,p3,p4);

                // If intersecting add the start and end indices to the intersect list 
                if(isIntersecting){
                    intersectIndex.Add(i);
                    intersectIndex.Add(j+1);
                }  
            }
        }

        intersectIndex.Add(line.Length - 1);


        for(int i = 0; i + 1 < intersectIndex.Count; i+=2){
            int startIndex = intersectIndex.ElementAt(i);
            int endIndex = intersectIndex.ElementAt(i + 1);
            for(int index = startIndex; index <= endIndex; index++){
                fixedLine.Add(line[index]);
            }
        }

        return fixedLine.ToArray();
    }

    public static bool IntersectionTest(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4){
        // Calculate the bezier parameter of the intersection 
        // Using t = (p1.x - p3.x) * (p3.y - p4.y) - (p1.y - p3.y) * (p3.x - p4.x) / (p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x)
        // t must be 0 <= t <= 1
        // This means division can be avoiding by by comparing the top and bottom portions of the equation
        float top_value_t = Mathf.Abs((p1.x - p3.x) * (p3.y - p4.y) - (p1.y - p3.y) * (p3.x - p4.x));
        float bottom_value_t = Mathf.Abs((p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x));

        // do the same for u = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x) / (p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x)
        float top_value_u = Mathf.Abs((p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x));
        float bottom_value_u = Mathf.Abs((p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x));

        bool valid_t;
        bool valid_u;

        if(bottom_value_t < 0){
            valid_t = top_value_t < 0 && top_value_t > bottom_value_t;
        }else{
            valid_t = top_value_t > 0 && top_value_t < bottom_value_t;
        }

        if(bottom_value_u < 0){
            valid_u = top_value_u < 0 && top_value_u > bottom_value_u;
        }else{
            valid_u = top_value_u > 0 && top_value_u < bottom_value_u;
        }

        return valid_t && valid_u;
    }
}