using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escena3b : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI texto1;
    [SerializeField] AudioSource audioTeclado;
    [SerializeField] Animator crossFade;

    FirebaseManager fbm;
    AuthManager authM;

    List<string> textos;

    string idCurrent;

    public float velText = 0.1f;
    int indice;

    // Start is called before the first frame update
    void Start()
    {
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        fbm.leerDialogos("Escena3b", result =>
        {
            textos = result;
            texto1.text = string.Empty;
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
            yield return new WaitForSeconds(velText);
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
            Vibration.Vibrate(300);
            fbm.actualizarCapitulo(idCurrent, "Nuevo");
            fbm.actualizarEstadistica("finalAlternativo");
            StartCoroutine(volverMenu());
            return;
        }
        StartCoroutine(TypeLine());
 
    }

    IEnumerator volverMenu()
    {
        yield return new WaitForSeconds(2f);
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MenuInicio");
    }
}
