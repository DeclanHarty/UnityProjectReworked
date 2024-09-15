using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(ChunkManager))]
public class ChunkManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChunkManager chunkManager = (ChunkManager)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generate Chunks")){
            chunkManager.GenerateChunks();
        }

        if(GUILayout.Button("Remove Chunks")){
            chunkManager.RemoveChunks();
        }

        if(GUILayout.Button("Instantiate Tiles")){
            chunkManager.InstantiateAllTilesInChunks();
        }

        if(GUILayout.Button("Destroy Tiles")){
            chunkManager.DestroyAllTilesInChunks();
        }

    }
}
