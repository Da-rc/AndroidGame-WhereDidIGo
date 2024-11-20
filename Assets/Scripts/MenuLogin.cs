using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogin : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TextMeshProUGUI indicacion;
    [SerializeField] Button botonLogin;
    [SerializeField] Button botonVolver;

    AuthManager authManager;
    public bool estado;


    private void Start()
    {
       authManager = new AuthManager();
       botonLogin.onClick.AddListener(() => Login());
       botonVolver.onClick.AddListener(() => volverAtras());
    }


    private void comprobarCampos()
    {
        if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            estado = false;
        }
        //esto seria para cuando añada campo de verificacion
        /*else if (passwordField.text != passwordVerifyField.text)
        {
            estado = false;
        }*/
        else
        {
            estado = true;
        }
    }

    private void Login()
    {
        comprobarCampos();
        if (estado == false)
        {
            indicacion.text = "No puede haber un campo vacio";
            Invoke("limpiarCampo", 2f);
        }
        else
        {
            authManager.comprobarLogin(emailField.text, passwordField.text, logueado => 
             {
                 if (logueado)
                 {
                     Debug.Log("logueado");
                     SceneManager.LoadScene("MenuInicio");
                 }
                 else
                 {
                     indicacion.text = "Error al loguear";
                     Invoke("limpiarCampo", 2f);
                     //Debug.Log("Error al loguear");
                 }
             });

        }
    }

    private void volverAtras()
    {
        SceneManager.LoadScene("MenuPreInicio");
    }
    private void limpiarCampo() 
    {
        indicacion.text = string.Empty;
    }
}
