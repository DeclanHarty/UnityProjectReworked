using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DelauneyTriangleTesting : MonoBehaviour
{
    private Delaunator delaunator;

    public MeshFilter meshFilter;

    public Vector2 topRightCorner;
    public Vector2 bottomLeftCorner;

    [Min(2)]
    public int gridScale;

    public GameObject pointPrefab;
    public int numOfPoints;
    public int startIndex;
    public IPoint[] points = new IPoint[0];
    public GameObject[] pointObjects = new GameObject[0];

    public GameObject linePrefab;
    public GameObject[] lineObjects = new GameObject[0];
    public List<GameObject> highlightLineObjects = new List<GameObject>();

    public GameObject textPrefab;

    public double[,] adjacencyGraph;

    public int[,] MST = new int[0,0];

    public HashSet<int> visitedPoints = new HashSet<int>();

    public IPoint[] CreatePoints(){
        float cellScale = (topRightCorner.x - bottomLeftCorner.x) / gridScale;
        IPoint[] points = new IPoint[gridScale * gridScale];
        for(int y = 0; y < gridScale; y++){
            for(int x = 0; x < gridScale; x++){
                Vector2 randOffset = new Vector2(UnityEngine.Random.Range(0f, cellScale), UnityEngine.Random.Range(0f, 1f));

                Vector2 pointPos = new Vector2(x * cellScale + randOffset.x + bottomLeftCorner.x, y * cellScale + randOffset.y + bottomLeftCorner.y);
                points[y * gridScale + x] = new Point(pointPos.x, pointPos.y);
            }
        }

        return points;
    }

    public void GetDelaunation(IPoint[] points){
        delaunator = new Delaunator(points);
    }

    public IEdge[] GetEdges(){
        return (IEdge[])delaunator.GetEdges();
    }

    public void GenerateMesh(){
        GetDelaunation(CreatePoints());
        points = delaunator.Points;
        int[] triangles = delaunator.Triangles;

        Mesh mesh = new Mesh{
            vertices = points.ToVectors3(),
            triangles = triangles
        };

        meshFilter.mesh = mesh;
        adjacencyGraph = PrimsAlgo.CreateWeightedAdjacencyMatrix(points, Enumerable.ToArray(delaunator.GetEdges()));

        // for(int i = 0; i < points.Length; i++){
        //     string doublesString = "";
        //     for(int j = 0; j < points.Length; j++){
        //         doublesString += adjacencyGraph[i,j].ToString() + ", ";
        //     }
        //     Debug.Log(doublesString);
        // }

        MST = PrimsAlgo.PrimMST(adjacencyGraph);
        for(int i = 0; i < points.Length; i++){
            string doublesString = "";
            for(int j = 0; j < points.Length; j++){
                doublesString += MST[i,j].ToString() + ", ";
            }
            Debug.Log(doublesString);
        }

        
    }

    void OnDrawGizmos(){
        if(points.Length > 0){
            for(int i = 0; i < MST.GetLength(0); i++){
                for(int j = 0; j < MST.GetLength(1); j++){
                    if(MST[i,j] == 1){
                        Gizmos.DrawLine(new Vector3((float)points[i].X, (float)points[i].Y), new Vector3((float)points[j].X, (float)points[j].Y));
                    }
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3((float)points[0].X, (float)points[0].Y), .1f);
        }  
    }

    void InstantiatePoints(){
        pointObjects = new GameObject[points.Length];
        for(int i = 0; i < points.Length; i++){
            pointObjects[i] = Instantiate(pointPrefab, new Vector2((float)points[i].X, (float)points[i].Y), Quaternion.identity);
        }
        pointObjects[0].GetComponent<SpriteRenderer>().color = Color.green;
    }

    void ClearPoints(){
        for(int i = 0; i < pointObjects.Length; i++){
            Destroy(pointObjects[i]);
        }
    }

    void ClearEdges(){
        for(int i = 0; i < lineObjects.Length; i++){
            Destroy(lineObjects[i]);
        }
        foreach(GameObject line in highlightLineObjects){
            Destroy(line);
        }
        visitedPoints = new HashSet<int>(new int[] {0});
    }

    public void ClearMeshAndEdges(){
        meshFilter.mesh = null;
        MST = new int[0,0];
    }

    public Vector2Int MoveForwardMSTOneStep(){
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
            return Vector2Int.zero;
        }

        MST[shortestEdgeStartPoint, shortestEdgeEndPoint] = 1;
        MST[shortestEdgeEndPoint, shortestEdgeStartPoint] = 1;
        visitedPoints.Add(shortestEdgeEndPoint);
        Debug.Log("Current Shortest Distance : " + currentShortestDistance);
        Vector2Int pointInfo = new Vector2Int(shortestEdgeStartPoint, shortestEdgeEndPoint);

        return pointInfo;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            ClearEdges();
            ClearPoints();
            points = CreatePoints();
            InstantiatePoints();
        }

        if(Input.GetKeyDown(KeyCode.Return) && pointObjects.Length > 0){
            ClearEdges();
            GetDelaunation(points);

            IEdge[] edges = Enumerable.ToArray(delaunator.GetEdges());
            
            adjacencyGraph = PrimsAlgo.CreateWeightedAdjacencyMatrix(points, edges);
            lineObjects = new GameObject[adjacencyGraph.GetLength(0) * adjacencyGraph.GetLength(0)];

            // for(int i = 0; i < edges.Length;  i++){
            //     Vector2 startPointPostion = new Vector2((float)edges[i].P.X, (float)edges[i].P.Y);
            //     Vector2 endPointPosition = new Vector2((float)edges[i].Q.X, (float)edges[i].Q.Y);
            //     lineObjects[i] = Instantiate(linePrefab, startPointPostion, Quaternion.identity);
            //     LineRendererObject lineScript = lineObjects[i].GetComponent<LineRendererObject>();
            //     lineScript.EnableLine(startPointPostion, endPointPosition, Color.clear);
            // }

            for(int y = 0; y < adjacencyGraph.GetLength(1); y++){
                for(int x = 0; x < adjacencyGraph.GetLength(0); x++){
                    if(adjacencyGraph[x,y] > 0){
                        Vector2 startPointPostion = new Vector2((float)points[x].X, (float)points[x].Y);
                        Vector2 endPointPosition = new Vector2((float)points[y].X, (float)points[y].Y);
                        lineObjects[y * adjacencyGraph.GetLength(0) + x] = Instantiate(linePrefab, startPointPostion, Quaternion.identity);
                        LineRendererObject lineScript = lineObjects[y * adjacencyGraph.GetLength(0) + x].GetComponent<LineRendererObject>();
                        lineScript.EnableLine(new Vector3[] {startPointPostion, endPointPosition}, Color.clear);
                        GameObject text = Instantiate(textPrefab, (endPointPosition + startPointPostion) / 2, Quaternion.identity);

                        text.GetComponent<TMP_Text>().text = (Math.Truncate(adjacencyGraph[x,y] * 100) / 100).ToString();
                    }
                }
            }

            MST = new int[points.Length, points.Length];
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)){
            Vector2Int lineInfo = MoveForwardMSTOneStep();
            if(lineInfo.x == 0 && lineInfo.y == 0){
                return;
            }

            Vector3 startPointPostion = new Vector3((float)points[lineInfo.x].X, (float)points[lineInfo.x].Y, -1);
            Vector3 endPointPosition = new Vector3((float)points[lineInfo.y].X, (float)points[lineInfo.y].Y, -1);
            GameObject lineObject = Instantiate(linePrefab, startPointPostion, Quaternion.identity);
            highlightLineObjects.Add(lineObject);
            LineRendererObject lineScript = lineObject.GetComponent<LineRendererObject>();
            lineScript.EnableLine(new Vector3[] {startPointPostion, endPointPosition}, Color.green);
        }
    }

    void Start(){
        visitedPoints = new HashSet<int>(new int[] {0});
    }
}
