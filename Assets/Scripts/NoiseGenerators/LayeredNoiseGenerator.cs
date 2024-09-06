using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LayeredNoiseGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct Noise{
        public float XOrg;
        public float YOrg;
        public float Roughness;

        [Range(0,1)]
        public float Strength;
    }

    public int width;
    public int height;

    public float persistence;

    public bool cutoff;
    [Range(0,1)]
    public float tolerence;

    public Noise[] noises = new Noise[1]; 

    public int iterations;
    void Start()
    {
        Texture2D noiseTex = CalcNoise(noises);
        GetComponent<Renderer>().material.mainTexture = noiseTex;

        // for(int i = 0; i < iterations; i ++){
        //     RunTurn();
        // }
    }

    // Update is called once per frame
    void Update()
    {
        RunTurn();
    }

    Texture2D CalcNoise(Noise[] noises){
        Texture2D texture = new Texture2D(width, height);
        Color[] pix = new Color[width * height];
         // For each pixel in the texture...
        for (float y = 0.0F; y < height; y++)
        {
            for (float x = 0.0F; x < width; x++)
            {
                float sample = 0;
                float persistencePower = 0;
                foreach(Noise noise in noises){
                    float xCoord = noise.XOrg + x / width;
                    float yCoord = noise.YOrg + y / height;
                    sample += Mathf.PerlinNoise(xCoord * noise.Roughness, yCoord * noise.Roughness) * noise.Strength * Mathf.Pow(persistence, persistencePower);
                    persistencePower++;
                }

                if(cutoff){
                    sample = sample >= tolerence ? sample = 1 : sample = 0;
                }
                pix[(int)y * width + (int)x] = new Color(sample, sample, sample);

            }
        }

        texture.SetPixels(pix);
        texture.Apply();

        return texture;
    }

    
    Texture2D SimulateGameOfLifeTurn(Texture2D sampleTex){
        Texture2D newTexture = new Texture2D(width, height);

        Color[] pix = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color[] neighbors = new Color[8];
                int counter = 0;
                for(int neighborY = y - 1; neighborY <= y + 1; neighborY++){
                    for(int neighborX = x - 1; neighborX <= x + 1; neighborX++){

                        if(neighborX == x && neighborY == y){ continue;}

                        if(neighborX < 0 || neighborY < 0 || neighborX > width || neighborY > height){
                            neighbors[counter] = new Color(0,0,0);
                        }else{
                            neighbors[counter] = sampleTex.GetPixel(neighborX, neighborY);
                        }

                        counter++;
                    }
                }

                int numOfWallNeighbors = 0;
                foreach(Color neighbor in neighbors){
                    if(neighbor.Equals(new Color(0,0,0))){
                        numOfWallNeighbors++;
                    }
                }

                Color newPix;
                if(numOfWallNeighbors > 4){
                    newPix = new Color(0,0,0);
                }else{
                    newPix = new Color(1,1,1);
                }

                pix[(int)y * width + (int)x] = newPix;
            }
        }

        newTexture.SetPixels(pix);
        newTexture.Apply();

        return newTexture;
    }

    [ContextMenu("Run Turn")]
    void RunTurn(){
        Texture2D newTexture = SimulateGameOfLifeTurn((Texture2D)GetComponent<Renderer>().material.mainTexture);

        GetComponent<Renderer>().material.mainTexture = newTexture;
    }

}

