using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
//Script que controla el dialogo y la velocidad a la que se escribe el texto
public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;

    [SerializeField] GameObject jugador;
    public Animator animator;
    [SerializeField] AudioSource audioTeclado;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    private List<string> list;
    private List<string> sentences;

    public float velText = 0.1f;
    int indice;

    void Start()
    {
        sentences = new List<string>();
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        fbm.leerDialogos("Escena6", result =>
        {
            list = result;
        });
    }

    public void StartDialogue(int clave)
    {
        jugador.GetComponent<PlayerMovement>().releaseLeft();
        jugador.GetComponent<PlayerMovement>().releaseRight();
        animator.SetBool("isOpen", true);
        sentences.Clear();
        indice = 0;
        //guardamos el split en un queue
        string[] array = list[clave].Split(';');
        foreach (string linea in array)
        {
            sentences.Add(linea);
        }
        dialogText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    public void botonSiguiente()
    {
        if (dialogText.text == sentences[indice])
        {
            siguienteLinea();
        }
        else
        {
            audioTeclado.Stop();
            StopAllCoroutines();
            dialogText.text = sentences[indice];
        }
    }

    public void siguienteLinea()
    {
        indice++;
        if (indice <= sentences.Count-1) 
        {
            dialogText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
       
    }

    //para que el texto se escriba letra a letra
    IEnumerator TypeLine()
    {
        audioTeclado.Play();
        foreach (char c in sentences[indice].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(velText);
        }
        audioTeclado.Stop();
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }

}
