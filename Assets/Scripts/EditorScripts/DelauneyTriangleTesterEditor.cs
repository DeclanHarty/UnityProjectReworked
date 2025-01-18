using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DelauneyTriangleTesting))]
public class DelauneyTriangleTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        DelauneyTriangleTesting delauneyTriangleTesting = (DelauneyTriangleTesting)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generate Delauney Mesh")){
            delauneyTriangleTesting.GenerateMesh();
        }

        if(GUILayout.Button("Clear Mesh and MST")){
            delauneyTriangleTesting.ClearMeshAndEdges();
        }
    }
}
