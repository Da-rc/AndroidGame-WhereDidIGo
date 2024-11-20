using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerRespuesta : MonoBehaviour
{
    public string opcion;
    public string tagContrario;
    [SerializeField] private AudioSource audioElegir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            audioElegir.Play();
            Destroy(gameObject);
            GameObject.Find(tagContrario).GetComponent<BoxCollider2D>().enabled = false;
            FindObjectOfType<MuestraDialogos>().guardarEstadisticas(opcion);
        }
    }
}
