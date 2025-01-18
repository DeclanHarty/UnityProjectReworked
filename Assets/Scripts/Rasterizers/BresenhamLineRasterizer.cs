using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BresenhamLineRasterizer : MonoBehaviour
{
    static List<Vector2Int> PlotLineLow(int x0, int y0, int x1, int y1){
        List<Vector2Int> plottedLine = new List<Vector2Int>();
        int dx = x1 - x0;
        int dy = y1 - y0;
        int yi = 1;

        if(dy < 0){
            yi = -1;
            dy = -dy;
        }
        int D = (2 * dy) - dx;
        int y = y0;

        for(int x = x0; x <= x1; x++){
            plottedLine.Add(new Vector2Int(x,y));
            if(D > 0){
                y += yi;
                D += 2 * (dy - dx);
            }else{
                D += 2*dy;
            }
        }

        return plottedLine;
    }

    static List<Vector2Int> PlotLineHigh(int x0, int y0, int x1, int y1){
        List<Vector2Int> plottedLine = new List<Vector2Int>();
        int dx = x1 - x0;
        int dy = y1 - y0;
        int xi = 1;

        if(dx < 0){
            xi = -1;
            dx = -dx;
        }
        int D = (2 * dx) - dy;
        int x = x0;

        for(int y = y0; y <= y1; y++){
            plottedLine.Add(new Vector2Int(x,y));
            if(D > 0){
                x += xi;
                D += 2 * (dx - dy);
            }else{
                D += 2*dx;
            }
        }

        return plottedLine;
    }

    public static List<Vector2Int> PlotLine(int x0, int y0, int x1, int y1){
        if(Math.Abs(y1 - y0) < Math.Abs(x1 - x0)){
            if(x0 > x1){
                return PlotLineLow(x1, y1, x0, y0);
            }else{
                return PlotLineLow(x0, y0, x1, y1);
            }
        }else{
            if(y0 > y1){
                return PlotLineHigh(x1, y1, x0, y0);
            }else{
                return PlotLineHigh(x0, y0, x1, y1);
            }
        }
    }

    public static List<Vector2Int> PlotMultiSegmentLine(Vector2Int[] vertices){
        List<Vector2Int> raster = new List<Vector2Int>();
        for(int i = 0; i < vertices.Length - 1; i++){
            raster.AddRange(PlotLine(vertices[i].x, vertices[i].y, vertices[i + 1].x, vertices[i + 1].y));
        }

        return raster;
    }
}
