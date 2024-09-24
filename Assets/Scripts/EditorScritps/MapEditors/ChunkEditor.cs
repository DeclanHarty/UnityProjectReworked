using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Chunk))]
public class ChunkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Chunk chunk = (Chunk)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generate Tiles")){
            chunk.InstantiateChunk();
        }

        if(GUILayout.Button("Remove Tiles")){
            chunk.DestroyChunk();
        }

    }
}

