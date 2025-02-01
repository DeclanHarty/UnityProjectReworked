using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(TilemapManagerLegacy))]
public class TilemapManagerLegacyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TilemapManagerLegacy tilemapManager = (TilemapManagerLegacy)target;

        DrawDefaultInspector();
        if(tilemapManager.generationType == GenerationType.CELLULAR_AUTOMATA){
            if(GUILayout.Button("Generate CA Map")){
                tilemapManager.GenerateRandomCellularAutomataMap();
            }
        }

        if(tilemapManager.generationType == GenerationType.FRACTAL_NOISE){
            if(GUILayout.Button("Generate Fractal Map")){
                tilemapManager.GenerateLayeredNoiseMap();
            }
        }

        if(tilemapManager.generationType == GenerationType.GENERATE_RASTERIZED_LINE){
            if(GUILayout.Button("Generate Rasterized Line")){
                tilemapManager.GenerateRasterizedLine();
            }
        }

        if(tilemapManager.generationType == GenerationType.ERASE_RASTERIZED_LINE){
            if(GUILayout.Button("Erase Rasterized Line")){
                tilemapManager.EraseRasterizedLine();
            }
        }
        
        if(GUILayout.Button("Generate Filled Map")){
            tilemapManager.GenerateFullMap();
        }

        if(GUILayout.Button("Clear Map")){
            tilemapManager.ClearMap();
        }
    }
}
