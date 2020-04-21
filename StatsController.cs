using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
    public PlayerMovement PlayerScript;
    public Slider healthbar, fuelbar, heatbar;

    private void Start()
    {

        healthbar.value = 1;
        heatbar.value = 1;
        fuelbar.value = 1;
    }
    private void Update()
    {
        healthbar.value = CalculateHealthBar();
        heatbar.value = CalculateHeatBar();
        fuelbar.value = CalcualteStamina();
    }
    float CalculateHealthBar()
    {
        return healthbar.value = PlayerScript.currentHp / PlayerScript.maxHp;

    }
    float CalculateHeatBar()
    {

        return heatbar.value = PlayerScript.heat / PlayerScript.MaxHeat;
    }
    float CalcualteStamina()
    {
        return PlayerScript.Stamina / PlayerScript.MaxStamina;
    }
}
