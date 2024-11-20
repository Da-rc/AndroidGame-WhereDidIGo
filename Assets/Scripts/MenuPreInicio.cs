using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicial:MonoBehaviour
{

    public void menuLogin()
    {
        SceneManager.LoadScene("MenuLogin");
    }

    public void menuRegistro() 
    {
        SceneManager.LoadScene("MenuRegistro");
    }


}
