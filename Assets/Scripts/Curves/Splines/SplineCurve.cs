using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SplineCurve : MonoBehaviour {
    [Min(1)]
    public int length;

    [Min(1)]
    public int resolution;

    [Min(0)]
    public float splineComponentLength;

    public GameObject splineComponentPrefab;

    public Vector3[] controlPointPositions;
    public Vector3[] curvePositions = new Vector3[0];

    [SerializeField]
    public List<SplineObserver> observers;
    public bool autoUpdate;

    [ContextMenu("CreateSpline")]
    public Vector3[] CreateSpline(){
        controlPointPositions = new Vector3[length * 3 + 1];
        for(int i = 0; i < controlPointPositions.Length; i++){
            controlPointPositions[i] = new Vector3(transform.position.x + i * (splineComponentLength / 3), transform.position.y, transform.position.z);
        }

        float tPerVert = 1f/(resolution+1);
        Vector3[] vertexPositions = new Vector3[length * (resolution + 1) + 1];

        Vector3[] bezierPoints;
        for(int component = 0; component < length; component++){
            bezierPoints = new Vector3[4];
            Array.Copy(controlPointPositions, component * 3, bezierPoints, 0, 4);

            for(int vertexIndex = 0; vertexIndex < resolution + 1; vertexIndex++){
                vertexPositions[component * (resolution + 1) + vertexIndex] = BezierCurve.GetBezierValues(tPerVert * vertexIndex, bezierPoints);
            }
        }

        bezierPoints = new Vector3[4];
        Array.Copy(controlPointPositions, (length - 1) * 3, bezierPoints, 0, 4);

        vertexPositions[vertexPositions.Length - 1] = BezierCurve.GetBezierValues(1, bezierPoints);
        curvePositions = vertexPositions;

        Notify();
        return curvePositions;
    }

    [ContextMenu("UpdateSpline")]
    public Vector3[] UpdateSpline(){
        if(controlPointPositions.Length != length * 3 + 1){
           int lengthDiff = length - (controlPointPositions.Length - 1) / 3;
           int sign = Math.Sign(lengthDiff);

           for(int i = 0; i < Math.Abs(lengthDiff); i++){
                if(sign > 0) AddSegment(); else RemoveSegment();
           }
        }
        float tPerVert = 1f/(resolution+1);
        Vector3[] vertexPositions = new Vector3[length * (resolution + 1) + 1];

        Vector3[] bezierPoints;
        for(int component = 0; component < length; component++){
            bezierPoints = new Vector3[4];
            Array.Copy(controlPointPositions, component * 3, bezierPoints, 0, 4);

            for(int vertexIndex = 0; vertexIndex < resolution + 1; vertexIndex++){
                vertexPositions[component * (resolution + 1) + vertexIndex] = BezierCurve.GetBezierValues(tPerVert * vertexIndex, bezierPoints);
            }
        }

        bezierPoints = new Vector3[4];
        Array.Copy(controlPointPositions, (length - 1) * 3, bezierPoints, 0, 4);

        vertexPositions[vertexPositions.Length - 1] = BezierCurve.GetBezierValues(1, bezierPoints);
        curvePositions = vertexPositions;

        Notify();
        return curvePositions;
    }

    public void AddSegment(){
        Vector3[] newControlPoints = new Vector3[length * 3 + 1];
        Array.Copy(controlPointPositions, newControlPoints, controlPointPositions.Length);
        Vector3 tangentDirection = (controlPointPositions[controlPointPositions.Length - 1] - controlPointPositions[controlPointPositions.Length - 2]).normalized;
        Vector3 lastControlPointPosition = controlPointPositions[controlPointPositions.Length - 1];
        for(int i = 0; i < newControlPoints.Length - controlPointPositions.Length; i++){
            newControlPoints[i + controlPointPositions.Length] = lastControlPointPosition + tangentDirection * (splineComponentLength / 3) * i;
        }

        controlPointPositions = newControlPoints;
    }

    public void RemoveSegment(){
        Vector3[] newControlPoints = new Vector3[length * 3 + 1];
        Array.Copy(controlPointPositions, newControlPoints, newControlPoints.Length);
        controlPointPositions = newControlPoints;
    }

    public void SetControlPointPositions(Vector3[] controlPointPositions){
        if((controlPointPositions.Length - 1) % 3 == 0){
            this.controlPointPositions = controlPointPositions;
            length = (controlPointPositions.Length - 1)/3;
            UpdateSpline();
        }
    }

    public Vector3[] GetCurvePositions(){
        return curvePositions;
    }

    public void Register(SplineObserver observer){
        observers.Add(observer);
    }

    public void Deregister(SplineObserver observer){
        observers.Remove(observer);
    }

    public void Notify(){
        foreach(SplineObserver observer in observers){
            observer.NotifyUpdate();
        }
    }

    public static Vector3[] CreateSpline(Vector3[] controlPoints, int resolution){
        float tPerVert = 1f/(resolution+1);
        int numberOfComponents = (controlPoints.Length - 1) / 3;

        Vector3[] vertexPositions = new Vector3[numberOfComponents * (resolution + 1) + 1];

        Vector3[] bezierPoints;
        
        for(int component = 0; component < numberOfComponents; component++){
            bezierPoints = new Vector3[4];
            Array.Copy(controlPoints, component * 3, bezierPoints, 0, 4);

            for(int vertexIndex = 0; vertexIndex < resolution + 1; vertexIndex++){
                vertexPositions[component * (resolution + 1) + vertexIndex] = BezierCurve.GetBezierValues(tPerVert * vertexIndex, bezierPoints);
            }
        }

        bezierPoints = new Vector3[4];
        Array.Copy(controlPoints, (numberOfComponents - 1) * 3, bezierPoints, 0, 4);

        vertexPositions[vertexPositions.Length - 1] = BezierCurve.GetBezierValues(1, bezierPoints);
        return vertexPositions;
    }
}

enum ControlsConstraint {
    DISCONTINUOUS,
    FIRST_DEGREE_CONTINUOUS,
    SECOND_DEGREE_CONTINUOUS
}