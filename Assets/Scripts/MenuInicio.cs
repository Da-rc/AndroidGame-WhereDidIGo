using Firebase.Auth;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicio : MonoBehaviour
{
    [SerializeField] Button botonComenzar;
    [SerializeField] GameObject panelExpulsion;
    [SerializeField] TextMeshProUGUI textExpulsion;

    public Animator animator;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    string capitulo;
    DateTime timestamp;
    bool cuentaAtras = false;

    // Start is called before the first frame update
    void Start()
    {
        panelExpulsion.SetActive(false);
        PlayerPrefs.SetInt("muteado", 0);
        botonComenzar.interactable = false;
        fbm=new FirebaseManager();
        authM=new AuthManager();

        //Obtenemos el id del usuario conectado actualmente desde el AuthManager
        idCurrent = authM.obtenerID();
        Debug.Log(idCurrent);
        //llamamos a este metodo de FirebaseManager para que nos devuelva los datos del usuario logueado
        fbm.leerUsuario(idCurrent, result =>
        {
            capitulo = result.capitulo;
            timestamp = result.horaExpulsion.ToDateTime();
            if (!capitulo.Equals("Nuevo")) 
            {
                botonComenzar.interactable = true;
            }
        });
    }

    private void Update()
    {
        //Esto necesito hacerlo en el update y no en el cargarGuardado para que la cuenta atrás se vaya actualizando
        if (cuentaAtras)
        {
            TimeSpan minExpulsion = TimeSpan.FromMinutes(5);
            TimeSpan minPasados = DateTime.UtcNow.Subtract(timestamp);
            TimeSpan minRestantes = minExpulsion.Subtract(minPasados);

            if (minRestantes.TotalMinutes >= 0.01)
            {
                textExpulsion.text = "Amorch, fuiste expulsado, no puedes volver a entrar hasta dentro de <color=red>" +
                    $"{(int)minRestantes.TotalMinutes}:{minRestantes.Seconds:00}" +
                    "</color> minutos.\n \nA ver si encima de ser mala persona, no sabes escuchar.";
            }
            else
            {
                SceneManager.LoadScene(capitulo);
            }
        }
    }


    public void cargarGuardado()
    {
        if ((DateTime.UtcNow.Subtract(timestamp)).TotalMinutes < 5)
        {
            panelExpulsion.SetActive(true);
            cuentaAtras = true;
           
        }
        else
        {
            SceneManager.LoadScene(capitulo);
        }
    }

    public void activarPopup() 
    {
        if (capitulo == "Nuevo")
        {
            nuevaPartida();
        }
        else 
        {
            animator.SetBool("isOpen", true);
        }
    }

    public void cerrarPopup()
    {
        animator.SetBool("isOpen", false);
    }

    public void nuevaPartida()
    {
        fbm.actualizarCapitulo(idCurrent, "Escena1");
        fbm.resetearMuertes(idCurrent);
        SceneManager.LoadScene("Escena1");
    }

    public void salirJuego()
    {
        Application.Quit();
    }

    public void cerrarPopupExpulsion() 
    {
        cuentaAtras = false;
        panelExpulsion.SetActive(false);
    }

    public void logout() 
    {
        authM.SignOut();
        SceneManager.LoadScene("MenuPreInicio");
    }
}
