using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuerteCombate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI texto1;
    [SerializeField] AudioSource audioTeclado;
    [SerializeField] Animator crossFade;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;
    List<string> textos;
    int indice;

    Jugador datosJugador;

    // Start is called before the first frame update
    void Start()
    {
        crossFade.SetTrigger("crossFadeIn");
        indice = 0;
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        fbm.leerDialogos("EscenaMuerte", result =>
        {
            textos = result;
            //debe estar dentro de la llamada a leerPrueba o se ejecuta antes de que se guarde la información en el array
            texto1.text = string.Empty;
            fbm.leerUsuario(idCurrent, resultado => {
                datosJugador = resultado;
            });          
        startDialogo();
        });
    }

    void startDialogo()
    {
        indice = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        audioTeclado.Play();
        foreach (char c in textos[indice].ToCharArray())
        {
            texto1.text += c;
            yield return new WaitForSeconds(0.1f);
        }
        audioTeclado.Stop();
        yield return new WaitForSeconds(2f);
        siguienteLinea();
    }

    void siguienteLinea()
    {
        indice++;
        texto1.text = string.Empty;
        if (indice == textos.Count)
        {
            Vibration.Vibrate(200);
            StartCoroutine(volverCombate());
            return;
        }
        StartCoroutine(TypeLine());
    }

    IEnumerator volverCombate()
    {
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(datosJugador.capitulo);
    }
}
