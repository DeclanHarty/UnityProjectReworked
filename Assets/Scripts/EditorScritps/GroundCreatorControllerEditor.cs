
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(GroundCreatorController))]
public class GroundCreatorControllerEditor : Editor
{
    public override void OnInspectorGUI()
        {
            GroundCreatorController groundCreator = (GroundCreatorController)target;

            DrawDefaultInspector();

            if(GUILayout.Button("Generate Map")){
                groundCreator.CreateMap();
            }

            if(GUILayout.Button("Delete Map")){
                groundCreator.Reset();
            }
            
        }
}
