using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHp : MonoBehaviour
{
     public PlayerMovement PlayerScript;
     public Slider healthbar, fuelbar, heatbar;

    private void Start()
    {

        healthbar.value = 1;
        heatbar.value = 1;
    }
    private void Update()
    {
        healthbar.value = CalculateHealthBar();
        heatbar.value = CalculateHeatBar();
    }
    float CalculateHealthBar()
    {
        return healthbar.value = PlayerScript.currentHp / PlayerScript.maxHp;

    }
    float CalculateHeatBar()
    {

        return heatbar.value = PlayerScript.heat / PlayerScript.MaxHeat;
    }
}
