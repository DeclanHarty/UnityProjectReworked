using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    [SerializeField]
    private UIManager uIManager;

    public void Resume(){
        uIManager.ExitPauseFromUI();
    }
}
