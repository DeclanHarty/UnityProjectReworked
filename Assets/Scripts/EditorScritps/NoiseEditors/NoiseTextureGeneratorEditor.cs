using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (NoiseTextureGenerator))]
public class NoiseTextureGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseTextureGenerator noiseGen = (NoiseTextureGenerator)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generate Static Noise Texture")){
            noiseGen.SetTextureToNoise();
        }

        if(GUILayout.Button("Generate BW Noise Texture")){
            noiseGen.SetTextureToBWNoise();
        }
    }
}
