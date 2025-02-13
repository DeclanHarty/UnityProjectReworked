using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarFill;
    public void SetHealthBar(float healthPercentage){
        healthBarFill.transform.localScale = new Vector3(healthPercentage, healthBarFill.transform.localScale.y, healthBarFill.transform.localScale.z);
    }
}
