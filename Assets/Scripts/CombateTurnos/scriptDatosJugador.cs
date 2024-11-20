using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scriptDatosJugador : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoNombre;
    [SerializeField] Slider hpSlider;

    public void setHUD(UnitJugador unit)
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
