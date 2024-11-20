using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Escena4 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI texto1;
    [SerializeField] Button boton1;
    [SerializeField] Animator animOjoIzq;
    [SerializeField] Animator animOjoDrch;
    [SerializeField] Animator animLabios;
    [SerializeField] Animator crossFade;
    [SerializeField] AudioSource audioTeclado;
    [SerializeField] GameObject parteInicial;
    [SerializeField] GameObject caraNarrador;
    [SerializeField] GameObject scriptCombate;
    [SerializeField] GameObject muerteCombate;


    FirebaseManager fbm;
    AuthManager authM;

    List<string> textos;

    string idCurrent;

    public float velText = 0.1f;
    int indice;

    private void Awake()
    {
        scriptCombate.SetActive(false);
        muerteCombate.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        Debug.Log(idCurrent);
        fbm.leerDialogos("Escena4", result =>
        {
            textos = result;
            texto1.text = string.Empty;
            startDialogo();
        });
    }

    // Update is called once per frame
    void Update()
    {
        boton1.onClick.RemoveAllListeners();
        boton1.onClick.AddListener(() =>
        {
            if (texto1.text == textos[indice])
            {
                if (indice == 1 || indice == 2) {
                    Vibration.Vibrate(300);
                    animOjoIzq.SetTrigger("ojoVibracion");
                    animOjoDrch.SetTrigger("ojoVibracion");
                    animLabios.SetTrigger("labiosVibracion");
                }
                if (indice >= textos.Count-1)//Esto es para saltar ya a la siguiente escena
                {
                    Vibration.Vibrate(300);
                    animOjoIzq.SetTrigger("ojoVibracion");
                    animOjoDrch.SetTrigger("ojoVibracion");
                    animLabios.SetTrigger("labiosVibracion");
                    StartCoroutine(siguienteEscena());
                    boton1.enabled = false;
                    return;
                }
                siguienteLinea();
            }
            else
            {
                StopAllCoroutines();
                texto1.text = textos[indice];
                audioTeclado.Stop();
            }
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
    }

    void siguienteLinea()
    {
        Debug.Log(indice);
        indice++;
        texto1.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator siguienteEscena()
    {
        yield return new WaitForSeconds(1f);
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        parteInicial.SetActive(false);
        caraNarrador.SetActive(false);
        scriptCombate.SetActive(true);
    }
}
