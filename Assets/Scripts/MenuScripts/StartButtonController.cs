using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonController : MonoBehaviour
{
    private AsyncOperation coroutine;
    public void MoveToNextScene(){
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene(){
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Scenes/Testing/TilesetTesting");

        while(!asyncOperation.isDone){
            yield return null;
        }
    }
}
