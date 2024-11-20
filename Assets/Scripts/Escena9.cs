using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escena9 : MonoBehaviour
{
    [SerializeField] AudioSource audioTeclado;
    [SerializeField] TextMeshProUGUI texto1;
    [SerializeField] Animator crossFade;
    [SerializeField] GameObject sello;
    [SerializeField] GameObject txtCredito;
    [SerializeField] GameObject endPoint;

    FirebaseManager fbm;
    AuthManager authM;

    List<string> textos;
    List<string> textoSplit;

    string idCurrent;
    Jugador j;

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
        fbm.leerDialogos("Escena9", result =>
        {
            textos = result;
            fbm.leerUsuario(idCurrent, resultado =>
            {
                j = resultado;
                if (j.listaHitos["florSalvada"] && j.listaHitos["insectoSalvado"])
                {
                    splitDialogos(0);
                    texto1.text = string.Empty;
                    fbm.actualizarEstadistica("finalTrueBueno");
                    startDialogo();
                }
                else if (!j.listaHitos["florSalvada"] && j.listaHitos["insectoSalvado"])
                {
                    splitDialogos(1);
                    texto1.text = string.Empty;
                    fbm.actualizarEstadistica("finalBueno");
                    startDialogo();
                }
                else if (j.listaHitos["florSalvada"] && !j.listaHitos["insectoSalvado"])
                {
                    splitDialogos(2);
                    texto1.text = string.Empty;
                    fbm.actualizarEstadistica("finalMalo");
                    startDialogo();
                }
                else if (!j.listaHitos["florSalvada"] && !j.listaHitos["insectoSalvado"])
                {
                    splitDialogos(3);
                    texto1.text = string.Empty;
                    fbm.actualizarEstadistica("finalTrueMalo");
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
        yield return new WaitForSeconds(2f);
        siguienteLinea();
    }

    void siguienteLinea()
    {
        //Para que aparezca el segundo boton o no
        indice++;
        if (indice == textoSplit.Count)
        {
            StartCoroutine(creditos());
            return;
        }
        texto1.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator creditos() 
    {
        yield return new WaitForSeconds(3f);
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        StartCoroutine(moverCreditos());
    }

    IEnumerator moverCreditos()
    {
        while (true)
        {
            sello.transform.position = Vector2.MoveTowards(sello.transform.position, endPoint.transform.position, Time.deltaTime * 1.0f);
            txtCredito.transform.position = Vector2.MoveTowards(txtCredito.transform.position, endPoint.transform.position, Time.deltaTime * 1.0f);
            // If the object has arrived, stop the coroutine
            if (txtCredito.transform.position.y == endPoint.transform.position.y)
            {
                SceneManager.LoadScene("Estadisticas");
            }
            // Otherwise, continue next frame
            yield return null;
        }
    }
}
