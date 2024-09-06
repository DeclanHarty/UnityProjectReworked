using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;
    public float tolerence;

    private Texture2D noiseTex;
    private Color[] pix;

    private Material material;


    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;

        Debug.Log(material);

        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];

        xOrg = Random.Range(0, 99999f); 
        yOrg = Random.Range(0, 99999f); 
        

        AssetDatabase.SaveAssets();
    }

    // Update is called once per frame
    void Update()
    {
        CalcNoise();

        material.mainTexture = noiseTex;
    }

    void CalcNoise(){
        for (float y = 0.0F; y < noiseTex.height; y++)
        {
            for (float x = 0.0F; x < noiseTex.width; x++)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                if(sample < tolerence){
                    sample = 1;
                }else{
                    sample = 0;
                }
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
}
