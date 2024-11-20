
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EstadisticasFinales : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] Button btnSiguiente;
    [SerializeField] Button btnAtras;
    [SerializeField] Button btnInicio;

    int indice;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    Estadisticas estadisticas;
    Jugador jugador;

    private void Awake()
    {
        btnInicio.gameObject.SetActive(false);
        btnAtras.gameObject.SetActive(false);
        foreach (GameObject panel in panels) 
        {
            panel.gameObject.SetActive(false);
        }
        panels[indice].gameObject.SetActive(true);
    }

    private void Start()
    {
        indice = 0;
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
    }

    private void Update()
    {
        if (indice > 0)
        {
            btnAtras.gameObject.SetActive(true);
        }
        else
        {
            btnAtras.gameObject.SetActive(false);
        }

        if (indice == panels.Length - 1)
        {
            btnInicio.gameObject.SetActive(true);
            btnSiguiente.gameObject.SetActive(false);
        }
        else {
            btnInicio.gameObject.SetActive(false);
            btnSiguiente.gameObject.SetActive(true);
        }
    }

    public void siguiente()
    {
        panels[indice].gameObject.SetActive(false);
        indice++;
        if (indice <= panels.Length - 1)
        {
            panels[indice].gameObject.SetActive(true);
        }
        else
        {
            indice = 0;
            panels[indice].gameObject.SetActive(true);
        }
    }

    public void atras()
    {
        panels[indice].gameObject.SetActive(false);
        indice--;
        if (indice >= 0)
        {
            panels[indice].gameObject.SetActive(true);
        }
    }


    public void volverInicio()
    {
        fbm.actualizarCapitulo(idCurrent, "Nuevo");
        SceneManager.LoadScene("MenuInicio");
    }

   
  
}
