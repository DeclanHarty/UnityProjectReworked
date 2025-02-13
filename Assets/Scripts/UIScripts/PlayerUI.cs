using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;
    public void UpdatePlayerHealthBar(float healthPercentage){
        healthBar.SetHealthBar(healthPercentage);
    }
}
