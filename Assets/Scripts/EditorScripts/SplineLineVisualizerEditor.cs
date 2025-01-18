using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineLineVisualizer))]
public class SplineLineVisualizerEditor : Editor
{
    
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
    }

    public void OnSceneGUI(){
        SplineLineVisualizer splineLineVisualizer = (SplineLineVisualizer)target;

        Vector3[] controls = splineLineVisualizer.GetControlPointPositions();

        Handles.color = Color.yellow;
        foreach(Vector3 control in controls){
            Handles.DrawSolidDisc(control, Vector3.forward, .05f);
        }
    }
}
