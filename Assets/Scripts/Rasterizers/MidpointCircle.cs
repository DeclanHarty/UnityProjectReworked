using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MidpointCircle
{
    public static List<Vector2Int> FindPerimeter(Vector2Int centerPoint, int radius){
        List<Vector2Int> points = new List<Vector2Int>();
        int x = radius;
        int y = 0;

        int t1 = radius / 16;
        int t2;

        while(x >= y){
            foreach(Vector2Int point in GetSymmetricPoints(x,y)){
                points.Add(point + centerPoint);
            }

            y++;
            t1 += y;
            t2 = t1 - x;
            if(t2 >= 0){
                t1 = t2;
                x = x - 1;
            }
        }

        return points;
    }

    public static List<Vector2Int> GetSymmetricPoints(int x, int y){
        List<Vector2Int> points = new List<Vector2Int>(new Vector2Int[] {new Vector2Int(x,y), new Vector2Int(-x,y), new Vector2Int(x,-y), new Vector2Int(-x,-y), new Vector2Int(y,x), new Vector2Int(-y,x), new Vector2Int(y,-x), new Vector2Int(-y,-x)});

        return points;
    }
}
