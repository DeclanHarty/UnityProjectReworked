using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(TilemapManager))]
public class TilemapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TilemapManager tilemapManager = (TilemapManager)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Create Map")){
            tilemapManager.CreateMap();
        }

        if(GUILayout.Button("Clear Map")){
            tilemapManager.ClearMap();
        }
    }
}
