using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public int dialogo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            //Dialogo
            Destroy(gameObject);
            FindObjectOfType<DialogManager>().StartDialogue(dialogo);
        }
    }
}