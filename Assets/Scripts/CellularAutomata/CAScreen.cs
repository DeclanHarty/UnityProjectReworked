using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class CAScreen : MonoBehaviour
{

    public int width;
    public int height;
    [Range(0,1)]
    public float cutoff;

    public Renderer TextureRenderer;

    private bool[] currentGrid;

    Texture2D GenerateBWNoiseTexture(bool[] BWNoise){
        Texture2D texture2D = new Texture2D(width,height);

        Color[] pixels = new Color[BWNoise.Length];

        for(int i = 0; i < pixels.Length; i++){
            pixels[i] = BWNoise[i] ? Color.black : Color.white;
        }

        texture2D.SetPixels(pixels);
        texture2D.filterMode = FilterMode.Point;
        texture2D.Apply();

        return texture2D;
    }

    [ContextMenu("SetNoiseTexture")]
    void SetNoiseTexture(){
        currentGrid = TilemapManagerLegacy.Convert2DArrayMapToArrayMap(StaticNoiseGenerator.GenerateStaticBWNoise(width, height, cutoff), width, height);


        Texture2D texture2D = GenerateBWNoiseTexture(currentGrid);

        TextureRenderer.sharedMaterial.mainTexture = texture2D;
    }

    [ContextMenu("Run Cellular Automata")]
    void RunCellularAutomata(){
        bool[] newGrid = CaveCA.RunTurn(currentGrid, width);

        Texture2D newTexture = GenerateBWNoiseTexture(newGrid);

        currentGrid = newGrid;

        TextureRenderer.sharedMaterial.mainTexture = newTexture;
    }

    [ContextMenu("Run Cellular Automata x10")]
    void RunCellularAutomata10Times(){
        for(int i = 0; i < 10; i++){
            RunCellularAutomata();
        }
    }

}

        