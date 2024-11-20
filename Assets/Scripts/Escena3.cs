using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Escena3 : MonoBehaviour
{
    //FALTA PONER EL TEXTO DE LOS BOTONES
    [SerializeField] TextMeshProUGUI texto1;
    [SerializeField] Button boton1;
    [SerializeField] Button boton2;
    [SerializeField] AudioSource audioTeclado;
    [SerializeField] Animator crossFade;

    FirebaseManager fbm;
    AuthManager authM;

    List<string> textos;
    List<string> botones1 = new List<string>();
    List<string> botones2 = new List<string>();

    bool cambioEscena;

    string idCurrent;

    public float velText = 0.01f;
    int indice;
    int contadorExpulsion;

    // Start is called before the first frame update
    void Start()
    {
        cambioEscena = false;
        contadorExpulsion = 0;
        boton2.gameObject.SetActive(false);
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        fbm.leerDialogos("Escena3", result =>
        {
            textos = result;
            //debe estar dentro de la llamada a leerPrueba o se ejecuta antes de que se guarde la información en el array
            texto1.text = string.Empty;
            startDialogo();
        });

        botones1.Add("...hola??");
        botones1.Add("¿Dónde estoy?");
        botones1.Add("¿Qué?!");
        botones1.Add("Okis");
        botones1.Add("Ok, ok");
        botones1.Add("Vaaale, continuemos");

        botones2.Add("filler");
        botones2.Add("filler");
        botones2.Add("filler");
        botones2.Add("¡Explicamelo!");
        botones2.Add("¡Venga ya, explícalo!");
        botones2.Add("y tan en serio ¡Explica!");

        boton1.GetComponentInChildren<TMP_Text>().text = botones1[0];
        boton2.GetComponentInChildren<TMP_Text>().text = botones2[0];

        /*indice = 1;
        contadorExpulsion = 0;
        cambiarTextos();
        boton2.gameObject.SetActive(false);
        boton1.onClick.AddListener(() => cambiarTextos());
        boton2.onClick.AddListener(() => bucle());*/
    }

    private void Update()
    {
        boton1.onClick.RemoveAllListeners();
        boton1.onClick.AddListener(() =>
        {
            if (texto1.text == textos[indice])
            {
                if (indice >= 3)//Esto es para saltar ya a la siguiente escena
                {
                    boton1.enabled = false;
                    boton2.enabled = false;
                    cambioEscena = true;
                    fbm.actualizarCapitulo(idCurrent, "Escena4");
                    StartCoroutine(siguienteEscena());
                }
                siguienteLinea();
            }
            else
            {
                StopAllCoroutines();
                audioTeclado.Stop();
                texto1.text = textos[indice];
            }
        });
        boton2.onClick.RemoveAllListeners();
        boton2.onClick.AddListener(() =>
        {
            if (texto1.text == textos[indice])
            {
                if (contadorExpulsion == 2)//controla si se va a repetir el bucle una tercera vez
                {
                    boton1.enabled = false;
                    boton2.enabled = false;
                    cambioEscena = true;
                    fbm.actualizarCapitulo(idCurrent, "Escena3b");
                    StartCoroutine (finalAlternativo());
                }
                siguienteLinea();
            }
            else
            {
                StopAllCoroutines();
                audioTeclado.Stop();
                texto1.text = textos[indice];
            }
        });
    }

    private IEnumerator siguienteEscena()
    {
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Escena4");
    }

    private IEnumerator finalAlternativo()
    {
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Escena3b");
    }

    void startDialogo()
    {
        indice = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        if (!cambioEscena)
        {
            boton1.GetComponentInChildren<TMP_Text>().text = botones1[indice];
            boton2.GetComponentInChildren<TMP_Text>().text = botones2[indice];
            audioTeclado.Play();
            foreach (char c in textos[indice].ToCharArray())
            {
                texto1.text += c;
                yield return new WaitForSeconds(velText);
            }
            audioTeclado.Stop();
        }
    }

    void siguienteLinea()
    {
        //Esto controla el bucle del final
        if (indice == 5) 
        {
            contadorExpulsion++;//al sumar aquí dentro significa que ha dado una vuelta completa al bucle de las 3 frases
            indice = 2;
        }

        //Para que aparezca el segundo boton o no
        if (indice >= 2)
        {
            boton2.gameObject.SetActive(true);
            indice++;
            texto1.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if (indice < 2)
        {
            indice++;
            texto1.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    
}
