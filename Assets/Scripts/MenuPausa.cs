using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public GameObject menuPausa;
    public GameObject popup;
    [SerializeField] Button btnMenu;

    // Start is called before the first frame update
    void Start()
    {
        menuPausa.SetActive(false);
        popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        btnMenu.onClick.RemoveAllListeners();
        btnMenu.onClick.AddListener(() => pausarJuego());
    }

    public void pausarJuego()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
    }

    public void continuarJuego() 
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    public void abrirPopup()
    {
        popup.SetActive(true);
    }

    public void cerrarPopup()
    {
        popup.SetActive(false);
    }

    public void volverInicio()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}
