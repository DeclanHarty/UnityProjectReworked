using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class NoiseTextureGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public Renderer textureRenderer;

    private float[] noiseArray;

    [Range(0,1)]
    public float cutoff;

    private Texture2D CreateTextureFromStaticNoise(){
        Texture2D texture2D = new Texture2D(width, height);

        noiseArray = StaticNoiseGenerator.GenerateStaticNoise(width, height);

        Color[] noiseColors = new Color[noiseArray.Length];

        for(int i = 0; i < noiseColors.Length; i++){
            float valueAtPointInNoise = noiseArray[i];
            noiseColors[i] = new Color(valueAtPointInNoise,valueAtPointInNoise,valueAtPointInNoise);
        }

        texture2D.SetPixels(noiseColors);
        texture2D.Apply();

        return texture2D;
    }

    public void SetTextureToNoise(){
        textureRenderer.sharedMaterial.mainTexture = CreateTextureFromStaticNoise();
    }

    private Texture2D CreateBWTextureFromNoise(){
        Texture2D texture2D = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        float[] noise;

        if(noiseArray.Length > 0){
            noise = noiseArray;
        }else{
            noise = StaticNoiseGenerator.GenerateStaticNoise(width, height);
        }

        for(int i = 0; i < noise.Length; i++){
            if(noise[i] >= cutoff){
                pixels[i] = Color.white;
            }else{
                pixels[i] = Color.black;
            }
        }

        texture2D.SetPixels(pixels);
        texture2D.Apply();

        return texture2D;
    }

    public void SetTextureToBWNoise(){
        textureRenderer.sharedMaterial.mainTexture = CreateBWTextureFromNoise();
    }
}
