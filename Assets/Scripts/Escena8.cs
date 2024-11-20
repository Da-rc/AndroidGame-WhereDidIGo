using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Personalidad {TRUEBUENO, TRUEMALO, BUENO, MALO }
public class Escena8 : MonoBehaviour
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
    Jugador j;
    Personalidad personalidad;

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
        fbm.leerDialogos("Escena8", result =>
        {
            textos = result;
            fbm.leerUsuario(idCurrent, resultado =>
            {
                j = resultado;
                if (j.listaHitos["florSalvada"] && j.listaHitos["insectoSalvado"])
                {
                    audioBueno.Play();
                    animCanvas.SetTrigger("Good");
                    animOjoIzq.SetTrigger("ojoCalma");
                    animOjoDrch.SetTrigger("ojoCalma");
                    personalidad = Personalidad.TRUEBUENO;
                    splitDialogos(0);
                    texto1.text = string.Empty;
                    startDialogo();
                }
                else if (!j.listaHitos["florSalvada"] && j.listaHitos["insectoSalvado"])
                {
                    audioBueno.Play();
                    animCanvas.SetTrigger("Good");
                    animOjoIzq.SetTrigger("ojoCalma");
                    animOjoDrch.SetTrigger("ojoCalma");
                    personalidad = Personalidad.BUENO;
                    splitDialogos(1);
                    texto1.text = string.Empty;
                    startDialogo();
                }
                else if (j.listaHitos["florSalvada"] && !j.listaHitos["insectoSalvado"])
                {
                    audioMalo.Play();
                    animCanvas.SetTrigger("Evil");
                    animOjoIzq.SetTrigger("ojoEnfado");
                    animOjoDrch.SetTrigger("ojoEnfado");
                    animLabios.SetTrigger("labioEnfado");
                    personalidad = Personalidad.MALO;
                    splitDialogos(2);
                    texto1.text = string.Empty;
                    startDialogo();
                }
                else if (!j.listaHitos["florSalvada"] && !j.listaHitos["insectoSalvado"])
                {
                    audioMalo.Play();
                    animCanvas.SetTrigger("Evil");
                    animOjoIzq.SetTrigger("ojoEnfado");
                    animOjoDrch.SetTrigger("ojoEnfado");
                    animLabios.SetTrigger("labioEnfado");
                    personalidad = Personalidad.TRUEMALO;
                    splitDialogos(3);
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
        Debug.Log(indice);
        indice++;
        texto1.text = string.Empty;
        if (indice == textoSplit.Count)
        {
            if (personalidad == Personalidad.TRUEBUENO || personalidad == Personalidad.BUENO)
            {
                fbm.actualizarCapitulo(idCurrent, "Escena9");
                StartCoroutine(siguienteEscena());
            }
            else
            {
                fbm.actualizarCapitulo(idCurrent, "Escena9");
                fbm.actualizarEstadistica("expulsados");
                Invoke("expulsado", 3f);
            }
            return;
        }
        StartCoroutine(TypeLine());
    }

    private IEnumerator siguienteEscena()
    {
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Escena9");
    }

    private void expulsado()
    {
        fbm.guardarHoraExpulsion(idCurrent);
        Application.Quit();
    }
}
