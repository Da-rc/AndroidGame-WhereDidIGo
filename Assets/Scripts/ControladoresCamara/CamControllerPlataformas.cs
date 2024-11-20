using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para que la camara siga al personaje principal
public class CamControllerPlataformas : MonoBehaviour
{
    [SerializeField] private Transform player;

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

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y+0.7f, transform.position.z);
    }

}
