using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundCreatorController : MonoBehaviour
{
    public int width;
    public int height;
    public float cutoff;
    public int CAIterations;

    
    public GameObject groundParent;

    private bool[] map;

    public float tileSize;
    public GameObject tile;

    public GameObject[] tiles; 

    public GameObject worldPlaceholder;

    public bool[] CreateMap(){
        map = TilemapManager.Convert2DArrayMapToArrayMap(StaticNoiseGenerator.GenerateStaticBWNoise(width, height, cutoff), width, height);

        for(int i = 0; i < CAIterations; i++){
            map = CaveCA.RunTurn(map, width);
        }

        return map;

    }

    void Start(){
        // createMap();
    }

    

    public void InstantiateMap(){ 
        Renderer mapPlaceholderRenderer = worldPlaceholder.GetComponent<Renderer>();
        transform.localScale = new Vector3(width * tileSize / 10, height * tileSize / 10, 1);

        Texture2D texture2D = new Texture2D(width, height);
        texture2D.filterMode = FilterMode.Point;

        Color[] pixels = new Color[width * height];

        Color green = new Color(0.03529412f, 0.1215686f, 0.01176471f);

        for(int i = 0; i < map.Length; i++){
            if(map[i]){
                pixels[i] = green;
            }else{
                pixels[i] = Color.white;
            }
        }

        texture2D.SetPixels(pixels);
        texture2D.Apply();

        mapPlaceholderRenderer.sharedMaterial.mainTexture = texture2D;  
        
    }

    void DestroyTiles(GameObject[] gameObjects){
        for(int i = 0; i < gameObjects.Length; i++){
            DestroyImmediate(gameObjects[i]);
            gameObjects[i] = null;
        }
    }

    public void Reset(){
        DestroyTiles(tiles);
    }
}
