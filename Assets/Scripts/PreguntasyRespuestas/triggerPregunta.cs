using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerPregunta : MonoBehaviour
{
    public int dialogo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            Destroy(gameObject);
            FindObjectOfType<MuestraDialogos>().StartDialogue(dialogo);
        }
    }
}
