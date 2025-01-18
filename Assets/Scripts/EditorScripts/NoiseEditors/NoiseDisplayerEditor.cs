using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (NoiseDisplayer))]
public class NoiseDisplayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseDisplayer noiseDisplay = (NoiseDisplayer)target;

        if(DrawDefaultInspector()){
            if(noiseDisplay.autoUpdate){
                noiseDisplay.GenerateNoiseTexture();
            }
        }

        if(GUILayout.Button("Generate Perlin Noise Texture")){
            noiseDisplay.GenerateNoiseTexture();
        }

        if(GUILayout.Button("Generate Worley Noise")){
            noiseDisplay.GenerateWorleyNoiseTexutre();
        }
    }
}
