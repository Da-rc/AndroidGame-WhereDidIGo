using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPortrait : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("muteado") == 1)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

}
