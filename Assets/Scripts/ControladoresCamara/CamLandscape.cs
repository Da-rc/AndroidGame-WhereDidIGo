using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLandscape : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
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
