using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SilenciarAudio : MonoBehaviour
{
    [SerializeField] Sprite soundOn;
    [SerializeField] Sprite soundOff;
    [SerializeField] Button boton;
    private bool muteado;

    private void Start()
    {
        muteado = PlayerPrefs.GetInt("muteado") == 1 ? true : false;
        if (muteado) 
        {
            boton.image.sprite = soundOff;
        }
        else 
        {
            boton.image.sprite = soundOn;
        }
    }


    public void toggleSilencio()
    {
        if (muteado)
        {
            muteado = false;
            PlayerPrefs.SetInt("muteado", muteado ? 1 : 0);
            AudioListener.volume = 1;
            boton.image.sprite = soundOn;
        }
        else
        {
            muteado = true;
            PlayerPrefs.SetInt("muteado", muteado ? 1 : 0);
            AudioListener.volume = 0;
            boton.image.sprite = soundOff;
        }
    }
}
