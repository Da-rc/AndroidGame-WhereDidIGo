using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escena5 : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI texto1;
    [SerializeField] Animator animCanvas;
    [SerializeField] Animator animOjoIzq;
    [SerializeField] Animator animOjoDrch;
    [SerializeField] Animator animLabios;
    [SerializeField] Animator crossFade;
    [SerializeField] AudioSource audioBueno;
    [SerializeField] AudioSource audioMalo;
    [SerializeField] AudioSource audioTeclado;

    FirebaseManager fbm;
    AuthManager authM;

    List<string> textos;
    List<string> textoSplit;

    string idCurrent;

    public float velText = 0.1f;
    int indice;
    // Start is called before the first frame update
    void Start()
    {
        textos = new List<string>();
        textoSplit = new List<string>();
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        Debug.Log(idCurrent);
        fbm.leerDialogos("Escena5", result => 
        {
            textos = result;
            fbm.leerUsuario(idCurrent, resultado => 
            {
                Jugador j = resultado;
                if (j.listaHitos["florSalvada"])
                {
                    audioBueno.Play();
                    animCanvas.SetTrigger("Good");
                    animOjoIzq.SetTrigger("ojoCalma");
                    animOjoDrch.SetTrigger("ojoCalma");
                    splitDialogos(0);
                    texto1.text = string.Empty;
                    startDialogo();
                }
                else 
                {
                    audioMalo.Play();
                    animCanvas.SetTrigger("Evil");
                    animOjoIzq.SetTrigger("ojoEnfado");
                    animOjoDrch.SetTrigger("ojoEnfado");
                    animLabios.SetTrigger("labioEnfado");
                    splitDialogos(1);
                    texto1.text = string.Empty;
                    startDialogo();
                }
            });
        });
    }

    private void splitDialogos(int index) 
    {
        string[] textosArr = textos[index].Split(";");
        for (int i = 0; i < textosArr.Length; i++)
        {
            textoSplit.Add(textosArr[i]);
        }
    }
    void startDialogo()
    {
        indice = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        audioTeclado.Play();
        foreach (char c in textoSplit[indice].ToCharArray())
        {
            texto1.text += c;
            yield return new WaitForSeconds(velText);
        }
        audioTeclado.Stop();
        yield return new WaitForSeconds(1f);
        siguienteLinea();
    }

    void siguienteLinea()
    {
        indice++;
        texto1.text = string.Empty;
        if (indice == textoSplit.Count) 
        {
            StopAllCoroutines();
            fbm.actualizarCapitulo(idCurrent, "Escena6");
            //Cargar siguiente escena.
            StartCoroutine(siguienteEscena());
            return;
        }
        StartCoroutine(TypeLine());
    }

    private IEnumerator siguienteEscena()
    {
        yield return new WaitForSeconds(1f);
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Escena6");
    }
}
