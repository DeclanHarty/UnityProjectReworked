using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDisplayer : MonoBehaviour
{
    public Renderer textureRenderer;

    public int width;
    public int height;
    public bool blackAndWhite;
    [Range(0,1)]
    public float cutoff;

    [Header("Fractal Noise Params")]
    public float scale;

    public int octaves;
    public float persistence;
    public float lacunarity;

    [Header("Worley Noise Params")]
    public int numOfCells;
    public int[] degrees;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateNoiseTexture(){
        float[,] noiseMap = LayeredPerlinNoiseGenerator.GenerateFractalNoise(width, height, Vector2.zero, scale, octaves, persistence, lacunarity);

        Texture2D texture2D = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        int pixel_index = 0;
        foreach(float value in noiseMap){
            Color color = Color.black;
            for (int i = 0; i < regions.Length; i++){
                if(value <= regions[i].height){
                    color = regions[i].color;
                    break;
                }
            }

            if(regions.Length == 0){
                color = new Color(value, value, value);
            }
            pixels[pixel_index] = color;
            pixel_index++;
        }

        texture2D.SetPixels(pixels);
        texture2D.filterMode = FilterMode.Point;
        texture2D.wrapMode = TextureWrapMode.Clamp;
        texture2D.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture2D;
    }

    void Update(){
        //GenerateNoiseTexture();
    }

    public void GenerateWorleyNoiseTexutre(){
        float[,] worleyNoise = blackAndWhite ? WorleyNoise.GenerateBWWorleyNoise(width, height, numOfCells, degrees, cutoff) : WorleyNoise.GenerateWorleyNoise(width, height, numOfCells, degrees);

        Texture2D texture2D = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                pixels[y * width + x] = new Color(worleyNoise[x,y], worleyNoise[x,y], worleyNoise[x,y]);
            }
        }

        texture2D.SetPixels(pixels);
        texture2D.filterMode = FilterMode.Point;
        texture2D.wrapMode = TextureWrapMode.Clamp;
        texture2D.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture2D;
    }


}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}
