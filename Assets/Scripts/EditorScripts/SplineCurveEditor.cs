using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplineCurve))]
public class SplineCurveEditor : Editor
{
    public override void OnInspectorGUI(){
        SplineCurve splineCurve = (SplineCurve)target;

        if(DrawDefaultInspector()){
            if(splineCurve.autoUpdate) splineCurve.UpdateSpline();
        }

        if(GUILayout.Button("Create Line")){
            splineCurve.CreateSpline();
        }

        if(GUILayout.Button("Update Line")){
            splineCurve.UpdateSpline();
        }


    }
}
