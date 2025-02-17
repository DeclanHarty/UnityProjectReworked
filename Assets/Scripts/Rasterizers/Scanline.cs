using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Scanline : MonoBehaviour
{
    public GameObject square;
    public GameObject dot;

    public LineRenderer lineRenderer;

    public Mesh meshToRaster;



    public static Vector2Int[] ConvertFloatPolygonToIntPolygon(Vector2[] vertices){
        Vector2Int[] copyArray = new Vector2Int[vertices.Length];
        for(int i = 0; i < vertices.Length; i++){
            copyArray[i] = new Vector2Int((int)Mathf.Round(vertices[i].x), (int)Mathf.Round(vertices[i].y));
        }

        return copyArray;
    }

    public static Vector2[] ConvertVector3ArrayToVector2Array(Vector3[] array){
        Vector2[] new_array = new Vector2[array.Length];

        for(int i = 0; i < array.Length; i++){
            new_array[i] = array[i];
        }

        return new_array;
    }

    public static float[][] ConvertPolygonToEdgeTable(Vector2Int[] vertices){
        int length = vertices.Length;
        List<float[]> edgeTable = new List<float[]>(); // min Y, max Y, X value with min Y, Slope of Edge

        float slope;
        int minYIndex;
        int maxYIndex;
        int minXIndex = int.MaxValue;
        int maxXIndex = int.MinValue;
        for(int i = 0; i < length - 1; i++){
            
            if(vertices[i].y < vertices[i + 1].y){
                minYIndex = i;
                maxYIndex = i + 1;
            }else{
                minYIndex = i + 1;
                maxYIndex = i;
            }

            if(vertices[i].x < vertices[i + 1].x){
                minXIndex = i;
                maxXIndex = i + 1;
            }else{
                minXIndex = i + 1;
                maxXIndex = i;
            }

            try
            {
                slope = (float)(vertices[maxXIndex].y - vertices[minXIndex].y) / (float)(vertices[maxXIndex].x - vertices[minXIndex].x);
            }
            catch (System.Exception)
            {
                slope = float.PositiveInfinity;
            }

            if(slope != 0) edgeTable.Add(new float[] {vertices[minYIndex].y, vertices[maxYIndex].y, vertices[minYIndex].x, 1/slope});
            // edgeTable[i,1] = vertices[maxYIndex].y;
            // edgeTable[i,2] = vertices[minYIndex].x;
            // edgeTable[i,3] =  (vertices[maxYIndex].y - vertices[minYIndex].y) / (vertices[maxXIndex].x - vertices[minXIndex].x) != 0 ? 1 / (vertices[maxYIndex].y - vertices[minYIndex].y) / (vertices[maxXIndex].x - vertices[minXIndex].x) : 0;
        }

        if(vertices[length - 1].y < vertices[0].y){
            minYIndex = length - 1;
            maxYIndex = 0;
        }else{
            minYIndex = 0;
            maxYIndex = length - 1;
        }

        if(vertices[length - 1].x < vertices[0].x){
                minXIndex = length - 1;
                maxXIndex = 0;
            }else{
                minXIndex = 0;
                maxXIndex = length - 1;
            }

        try
        {
            slope = (float)(vertices[maxXIndex].y - vertices[minXIndex].y) / (float)(vertices[maxXIndex].x - vertices[minXIndex].x);
        }
        catch (System.Exception)
        {
            slope = float.PositiveInfinity;
        }

        if(slope != 0) edgeTable.Add(new float[] {vertices[minYIndex].y, vertices[maxYIndex].y, vertices[minYIndex].x, 1/slope});
        // edgeTable[length - 1,0] = vertices[minYIndex].y;
        // edgeTable[length - 1,1] = vertices[maxYIndex].y;
        // edgeTable[length - 1,2] = vertices[minYIndex].x;
        // edgeTable[length - 1,3] = (vertices[maxYIndex].y - vertices[minYIndex].y) / (vertices[maxXIndex].x - vertices[minXIndex].x) != 0 ? 1 / (vertices[maxYIndex].y - vertices[minYIndex].y) / (vertices[maxXIndex].x - vertices[minXIndex].x) : 0;

        return edgeTable.ToArray();
    }

    


    public static List<float[]> CreateSortedGlobalTable(float[][] edgeTable){
        float[][] globalEdgeTable = new float[edgeTable.Length][];

        Array.Copy(edgeTable, globalEdgeTable, edgeTable.Length);

        Comparer<float> comparer = Comparer<float>.Default;
        Array.Sort<float[]>(globalEdgeTable, (x,y) => {
            int sort_value = comparer.Compare(x[0], y[0]);
            if(sort_value != 0){
                return sort_value;
            }else{
                return comparer.Compare(x[2], y[2]);
            }
        });

        return globalEdgeTable.ToList();
    }

    public static List<Vector2> PolygonFill(Vector2Int[] vertices){
        float[][] edgeTable = ConvertPolygonToEdgeTable(vertices);
        List<float[]> globalTable = CreateSortedGlobalTable(edgeTable);

        List<Vector2> polygonRaster = new List<Vector2>();
        List<float[]> activeTable = new List<float[]>();


        // foreach(float[] edge in globalTable){
        //     Debug.Log("{" + edge[0] +  ", " + edge[1] +  ", " + edge[2] + ", " + edge[3] + "}");
        // }

        int scanline_y = (int)Mathf.Round(globalTable.ElementAt(0)[0]);
        UpdateScanline(scanline_y, ref globalTable, ref activeTable);
        
        while(activeTable.Count() > 0){
            //Debug.Log("Active Table Size : " + activeTable.Count());
            for(int i = 0; i < activeTable.Count(); i+=2){
                if(i >= activeTable.Count() || i+1 >= activeTable.Count()){
                    activeTable.Clear();
                    break;
                }
                int endValue = (int)Mathf.Round(Mathf.Max(activeTable.ElementAt(i)[1], activeTable.ElementAt(i + 1)[1]));
                int startValue = (int)Mathf.Round(Mathf.Min(activeTable.ElementAt(i)[1], activeTable.ElementAt(i + 1)[1]));
                for(int x = startValue; x < endValue; x++){
                    polygonRaster.Add(new Vector2(x, scanline_y));
                }  
                
            }

            scanline_y++;
            UpdateScanline(scanline_y, ref globalTable, ref activeTable);
        }

        return polygonRaster;
    }

    public static void UpdateScanline(float y_value, ref List<float[]> globalTable, ref List<float[]> activeTable){
        int globalTableLength = globalTable.Count();
        int activeTableLength = activeTable.Count();
        List<float[]> activeCopy = new List<float[]>();
        List<float[]> globalCopy = new List<float[]>();

        foreach(float[] edge in activeTable){
            if((int)Mathf.Floor(edge[0]) != y_value){
                activeCopy.Add(edge);
                edge[1] += edge[2];
            }
        }

        activeTable = activeCopy;

        foreach(float[] edge in globalTable)
        {
            if((int)Mathf.Floor(edge[0]) <= y_value){
                activeTable.Add(new float[] {edge[1], edge[2], edge[3]});
            }else{
                globalCopy.Add(edge);
            }
        }

        activeTable.Sort( (x,y) => Comparer<float>.Default.Compare(x[1], y[1]));

        globalTable = globalCopy;
    }

    void Start(){

    }
}
