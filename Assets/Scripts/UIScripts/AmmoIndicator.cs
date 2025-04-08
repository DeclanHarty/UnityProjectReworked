using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoIndicator : MonoBehaviour
{
    public GameObject ammoFillBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAmmoFillIndicator(float pct){
        ammoFillBar.GetComponent<Image>().material.SetFloat("_ChargePercentage", pct);
    }
}
