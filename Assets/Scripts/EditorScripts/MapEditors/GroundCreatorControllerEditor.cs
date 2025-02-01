
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(GroundCreatorController))]
public class GroundCreatorControllerEditor : Editor
{
    public override void OnInspectorGUI()
        {
            GroundCreatorController groundCreator = (GroundCreatorController)target;

            DrawDefaultInspector();

            // if(GUILayout.Button("Create Map")){
            //     groundCreator.CreateMap();
            // }

            // if(GUILayout.Button("Instantiate Map")){
            //     groundCreator.InstantiateMap();
            // }

            if(GUILayout.Button("Delete Map")){
                groundCreator.Reset();
            }
            
        }
}
