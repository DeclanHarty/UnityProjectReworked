using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseyCircleLineRenderer))]
public class NoiseyCircleLineRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseyCircleLineRenderer noiseyCircle = (NoiseyCircleLineRenderer)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generate Perlin Noise Texture")){
            noiseyCircle.CreateCircle();
        }

    }
}
