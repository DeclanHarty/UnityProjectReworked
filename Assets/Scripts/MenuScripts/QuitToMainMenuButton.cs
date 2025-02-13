using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenuButton : MonoBehaviour
{
    public static void QuitToMainMenu(){
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
