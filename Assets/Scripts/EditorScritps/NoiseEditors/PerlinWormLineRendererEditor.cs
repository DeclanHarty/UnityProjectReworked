using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(PerlinWormLineRenderer))]
public class PerlinWormLineRendererEditor : Editor
{
    public override void OnInspectorGUI(){
        PerlinWormLineRenderer perlinWormLineRenderer = (PerlinWormLineRenderer)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Create Worm")){
            perlinWormLineRenderer.createWorm();
        }
    }
}
