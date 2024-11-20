using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum estadoFlor { BROTE, CRECIENDO, MADURA }
public class Escena7 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI texto1;
    [SerializeField] Button flor1;
    [SerializeField] Button flor2;
    [SerializeField] Button flor3;
    [SerializeField] Button flor4;
    [SerializeField] Button btnContinuar;
    [SerializeField] Button btnSalvar;
    [SerializeField] Button btnAplastar;

    [SerializeField] Animator crossFade;

    [SerializeField] GameObject insectoRb;
    [SerializeField] GameObject posicionInsecto;

    [SerializeField] AudioSource audioteclado;
    [SerializeField] AudioSource audioInsecto;
    [SerializeField] AudioSource audioBtnFlor;
    [SerializeField] AudioSource audioFlorFinal;

    estadoFlor estadoFlor1;
    estadoFlor estadoFlor2;
    estadoFlor estadoFlor3;
    estadoFlor estadoFlor4;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    List<string> textos;
    int indice;
    public float velText = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //Al inicio los botones de las flores no se pueden pulsar
        flor1.enabled = false;
        flor2.enabled = false;
        flor3.enabled = false;
        flor4.enabled = false;
        btnSalvar.gameObject.SetActive(false);
        btnAplastar.gameObject.SetActive(false);

        //Establecemos el estado inicial de las flores
        estadoFlor1 = estadoFlor.BROTE;
        estadoFlor2 = estadoFlor.BROTE;
        estadoFlor3 = estadoFlor.BROTE;
        estadoFlor4 = estadoFlor.BROTE;

        textos = new List<string>();
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        Debug.Log(idCurrent);

        //leemos textos de la base de datos
        fbm.leerDialogos("Escena7", result =>
        {
            textos = result;
            //debe estar dentro de la llamada a leerPrueba o se ejecuta antes de que se guarde la información en el array
            texto1.text = string.Empty;
            startDialogo();
        });
    }

    // Update is called once per frame
    void Update()
    {
        btnContinuar.onClick.RemoveAllListeners();
        btnContinuar.onClick.AddListener(() =>
        {
            if (texto1.text == textos[indice])
            {
                if (indice >= textos.Count - 1)
                {
                    btnContinuar.interactable = false;
                }
                siguienteLinea();
            }
            else
            {
                audioteclado.Stop();
                StopAllCoroutines();
                texto1.text = textos[indice];
            }
        });

        //BOTONES INSECTO
        btnSalvar.onClick.RemoveAllListeners();
        btnSalvar.onClick.AddListener(() =>
        {
            StartCoroutine(animacionSalvado());
        });

        btnAplastar.onClick.RemoveAllListeners();
        btnAplastar.onClick.AddListener(() =>
        {
            StartCoroutine(animacionAplastado());
        });


        //BOTONES CRECIMIENTO FLORES
        flor1.onClick.RemoveAllListeners();
        flor1.onClick.AddListener(() =>
        {
            if (estadoFlor1 == estadoFlor.BROTE)
            {
                //cambiamos animacion
                //flor1.GetComponentInChildren<TextMeshProUGUI>().text = "creciendo";
   
                estadoFlor1 = estadoFlor.CRECIENDO;
                audioBtnFlor.Play();
                flor1.GetComponent<Animator>().SetInteger("state", (int)estadoFlor1);
            }
            else if (estadoFlor1 == estadoFlor.CRECIENDO)
            {
                //flor1.GetComponentInChildren<TextMeshProUGUI>().text = "madura";

                estadoFlor1 = estadoFlor.MADURA;
                audioBtnFlor.Play();
                flor1.GetComponent<Animator>().SetInteger("state", (int)estadoFlor1);
                if (estadoFlor2 == estadoFlor.MADURA && estadoFlor3 == estadoFlor.MADURA && estadoFlor4 == estadoFlor.MADURA)
                {
                    //lanzamos bicho
                    Debug.Log("aparece insecto");
                    decisionInsecto();
                }
            }
        });

        flor2.onClick.RemoveAllListeners();
        flor2.onClick.AddListener(() =>
        {
            if (estadoFlor2 == estadoFlor.BROTE)
            {
                //cambiamos animacion
                //flor2.GetComponentInChildren<TextMeshProUGUI>().text = "creciendo";
                estadoFlor2 = estadoFlor.CRECIENDO;
                audioBtnFlor.Play();
                flor2.GetComponent<Animator>().SetInteger("state", (int)estadoFlor2);
            }
            else if (estadoFlor2 == estadoFlor.CRECIENDO)
            {
                //flor2.GetComponentInChildren<TextMeshProUGUI>().text = "madura";
                estadoFlor2 = estadoFlor.MADURA;
                audioBtnFlor.Play();
                flor2.GetComponent<Animator>().SetInteger("state", (int)estadoFlor2);
                if (estadoFlor1 == estadoFlor.MADURA && estadoFlor3 == estadoFlor.MADURA && estadoFlor4 == estadoFlor.MADURA)
                {
                    //lanzamos bicho
                    Debug.Log("aparece insecto");
                    decisionInsecto();
                }
            }
        });

        flor3.onClick.RemoveAllListeners();
        flor3.onClick.AddListener(() =>
        {
            if (estadoFlor3 == estadoFlor.BROTE)
            {
                //cambiamos animacion
                //flor3.GetComponentInChildren<TextMeshProUGUI>().text = "creciendo";
                estadoFlor3 = estadoFlor.CRECIENDO;
                audioBtnFlor.Play();
                flor3.GetComponent<Animator>().SetInteger("state", (int)estadoFlor3);
            }
            else if (estadoFlor3 == estadoFlor.CRECIENDO)
            {
                //flor3.GetComponentInChildren<TextMeshProUGUI>().text = "madura";
                estadoFlor3 = estadoFlor.MADURA;
                audioBtnFlor.Play();
                flor3.GetComponent<Animator>().SetInteger("state", (int)estadoFlor3);
                if (estadoFlor1 == estadoFlor.MADURA && estadoFlor2 == estadoFlor.MADURA && estadoFlor4 == estadoFlor.MADURA)
                {
                    //lanzamos bicho
                    Debug.Log("aparece insecto");
                    decisionInsecto();
                }
            }
        });

        flor4.onClick.RemoveAllListeners();
        flor4.onClick.AddListener(() =>
        {
            if (estadoFlor4 == estadoFlor.BROTE)
            {
                //cambiamos animacion
                //flor4.GetComponentInChildren<TextMeshProUGUI>().text = "creciendo";
                estadoFlor4 = estadoFlor.CRECIENDO;
                audioBtnFlor.Play();
                flor4.GetComponent<Animator>().SetInteger("state", (int)estadoFlor4);
            }
            else if (estadoFlor4 == estadoFlor.CRECIENDO)
            {
                //flor4.GetComponentInChildren<TextMeshProUGUI>().text = "madura";
                estadoFlor4 = estadoFlor.MADURA;
                audioBtnFlor.Play();
                flor4.GetComponent<Animator>().SetInteger("state", (int)estadoFlor4);
                if (estadoFlor1 == estadoFlor.MADURA && estadoFlor2 == estadoFlor.MADURA && estadoFlor3 == estadoFlor.MADURA)
                {
                    //lanzamos bicho
                    Debug.Log("aparece insecto");
                    decisionInsecto();
                }
            }
        });
    }

    void decisionInsecto() 
    {
        btnContinuar.gameObject.SetActive(false);
        //Revisar esto para que aparezca gradualmente
        //insectoRb.transform.position = GameObject.Find("posicionInsecto").transform.position;
        //insectoRb.transform.position = Vector2.MoveTowards(insectoRb.transform.position, posicionInsecto.transform.position, Time.deltaTime * 2.0f);
        //insectoRb.transform.position = GameObject.Find("posicionInsecto").transform.position;
        StartCoroutine(MoverInsecto());
       /* while (insectoRb.transform.position != posicionInsecto.transform.position) {
            insectoRb.transform.position = Vector2.MoveTowards(insectoRb.transform.position, posicionInsecto.transform.position, Time.deltaTime * 2.0f);
        }*/


       //lanzamos la frase del insecto
        indice = 2;
        texto1.text = string.Empty;
        StartCoroutine(TypeLine());

    }

    IEnumerator MoverInsecto()
    {
        insectoRb.GetComponent<Animator>().SetTrigger("walk");
        audioInsecto.Play();
        while (true)
        {
            insectoRb.transform.position = Vector2.MoveTowards(insectoRb.transform.position, posicionInsecto.transform.position, Time.deltaTime * 2.0f);
            // If the object has arrived, stop the coroutine
            if (insectoRb.transform.position == posicionInsecto.transform.position)
            {
                Debug.Log("acabar");
                audioInsecto.Stop();
                insectoRb.GetComponent<Animator>().SetTrigger("wait");
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
    }

    //METODOS TEXTO
    void startDialogo()
    {
        indice = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        audioteclado.Play();
        foreach (char c in textos[indice].ToCharArray())
        {
            texto1.text += c;
            yield return new WaitForSeconds(velText);
        }
        audioteclado.Stop();
        if (indice == 2)
        {
            btnSalvar.gameObject.SetActive(true);
            btnAplastar.gameObject.SetActive(true);
        }
    }

    void siguienteLinea()
    {
        Debug.Log(indice);
        //Para que aparezca el segundo boton o no
        if (indice == 0)
        {
            indice = 1;
            texto1.text = string.Empty;
            StartCoroutine(TypeLine());
            flor1.enabled = true;
            flor2.enabled = true;
            flor3.enabled = true;
            flor4.enabled = true;
        }
    }

    //Animaciones

    IEnumerator animacionSalvado() {
        btnSalvar.gameObject.SetActive(false);
        btnAplastar.gameObject.SetActive(false);
        //GUARDAMOS EN LA BASE DE DATO EL BICHO SALVADO
        Debug.Log(idCurrent);
        fbm.actualizarHito(idCurrent, "listaHitos.insectoSalvado", true);
        fbm.actualizarEstadistica("insectosSalvados");
        fbm.actualizarCapitulo(idCurrent, "Escena8");

        //LANZAMOS ANIMACION SALVADO
        insectoRb.GetComponent<Animator>().SetTrigger("grow");
        yield return new WaitForSeconds(3.1F);
        audioFlorFinal.Play();
        //esperamos lo que dure la animacion
        //Quizas añadimos un texto del narrador
        yield return new WaitForSeconds(2f);


        //CAMBIAMOS DE SCENE
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Escena8");
    }


    IEnumerator animacionAplastado() {
        btnSalvar.gameObject.SetActive(false);
        btnAplastar.gameObject.SetActive(false);
        //GUARDAMOS EN LA BASE DE DATO EL BICHO Aplastado
        Debug.Log(idCurrent);
        fbm.actualizarHito(idCurrent, "listaHitos.insectoSalvado", false);
        fbm.actualizarCapitulo(idCurrent, "Escena7b");

        yield return new WaitForSeconds(1f);


        //CAMBIAMOS DE SCENE
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Escena7b");
    }
}
