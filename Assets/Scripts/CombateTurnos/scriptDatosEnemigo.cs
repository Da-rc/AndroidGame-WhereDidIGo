using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scripDatosEnemigo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoNombre;
    [SerializeField] Slider hpSlider;

    public void setHUD(UnitEnemigo unit) 
    {
        textoNombre.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void setHUDbicho(BichoEnemigo unit)
    {
        textoNombre.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void setHP(int hp)
    {
        hpSlider.value = hp;
    }
}
