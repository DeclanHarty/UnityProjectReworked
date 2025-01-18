using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineComponent : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject startControlLine;
    public GameObject endControlLine;

    [Min(1)]
    public int resolution;
    public GameObject curveLine;

    public bool usingVisibleControlPoints;
    public GameObject[] controlPoints = new GameObject[4];
    public Vector3[] controlPointPositions = new Vector3[4];

    public Vector3 startPosition {set; get;}
    public Vector3 endPosition {set; get;}
    // Start is called before the first frame update
    void Start()
    {
        if(usingVisibleControlPoints){
            controlPointPositions = GetControlPointPositions();

            startControlLine = Instantiate(linePrefab, transform);
            endControlLine = Instantiate(linePrefab, transform);

            startControlLine.GetComponent<LineRendererObject>().EnableLine(new Vector3[] {controlPointPositions[0], controlPointPositions[1]}, Color.gray);
            endControlLine.GetComponent<LineRendererObject>().EnableLine(new Vector3[] {controlPointPositions[3], controlPointPositions[2]}, Color.gray);
        }

        curveLine = Instantiate(linePrefab, transform);

        
        curveLine.GetComponent<LineRendererObject>().EnableLine(new Vector3[] { Vector3.one, Vector3.one}, Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        // Get Positions of the Control Points
        if(usingVisibleControlPoints){
            controlPointPositions = GetControlPointPositions();
        }
        

        // Determine the delta t depending on the resolution of the curve
        float tPerVert = 1f/(resolution+1);
        Vector3[] positions = new Vector3[resolution+2];

        // Get the positions on the curve with the given resolution plus 2 for the endPoints
        for(int i = 0; i < resolution + 2; i++){
            positions[i] = BezierCurve.GetBezierValues(tPerVert * i, controlPointPositions);
        }

        // Set curve
        curveLine.GetComponent<LineRendererObject>().SetPositions(positions);

        // Set Control Lines
        if(usingVisibleControlPoints){
            startControlLine.GetComponent<LineRendererObject>().SetPositions(new Vector3[] {controlPointPositions[0], controlPointPositions[1]});
            endControlLine.GetComponent<LineRendererObject>().SetPositions(new Vector3[] {controlPointPositions[3], controlPointPositions[2]});
        }
        

    }

    Vector3[] GetControlPointPositions(){
        Vector3[] positions = new Vector3[controlPoints.Length];
        for(int i = 0; i < controlPoints.Length; i++){
            positions[i] = controlPoints[i].transform.position;
        }

        return positions;
    }

    public void SetControlPointPositions(Vector3[] positions){
        controlPointPositions = positions;
        if(usingVisibleControlPoints){
            for(int i = 0; i < controlPoints.Length; i++){
                controlPoints[i].transform.position = positions[i];
            }
        }
    }

}
