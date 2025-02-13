using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerUIObject;
    [SerializeField]
    private GameObject GameOverScreen;
    [SerializeField]
    private GameObject PauseScreen;

    [SerializeField]
    private GameManager gameManager;

    public void UpdatePlayerHealthBar(float healthPercentage){
        playerUIObject.GetComponent<PlayerUI>().UpdatePlayerHealthBar(healthPercentage);
    }

    public void ActivateGameOverScreen(){
        GameOverScreen.SetActive(true);
    }

    public void DeactivatePlayerUI(){
        playerUIObject.SetActive(false);
    }

    public void ActivatePauseScreen(){
        PauseScreen.SetActive(true);
    }

    public void DeactivatePauseScreen(){
        PauseScreen.SetActive(false);
    }

    public void ExitPauseFromUI(){
        DeactivatePauseScreen();
        gameManager.SwitchState(new Playing());
    }

}
