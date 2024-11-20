using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//PARA QUE LA CAMARA SIGA AL JUGADOR PERO EL JUGADOR ESTÉ EN LA PARTE BAJA DE LA PANTALLA
public class CamControllerEscena2 : MonoBehaviour
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
        transform.position = new Vector3(player.position.x, player.position.y+4f, transform.position.z);
    }
}
