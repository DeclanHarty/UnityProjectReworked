using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PlayerUI playerUI;

    public void UpdatePlayerHealthBar(float healthPercentage){
        playerUI.UpdatePlayerHealthBar(healthPercentage);
    }
}
