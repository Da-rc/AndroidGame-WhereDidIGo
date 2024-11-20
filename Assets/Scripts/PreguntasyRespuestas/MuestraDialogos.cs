using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MuestraDialogos : MonoBehaviour
{
    public TextMeshProUGUI dialogText;

    public GameObject panel;
    [SerializeField] AudioSource audioTeclado;

    private List<string> list;
    public float velText = 0.1f;
    int indice;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);

        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();

        list = new List<string>
        {
            "ELIGE",
            "¿A cuál salvarías?",
            "El otro ha muerto",
            "¿Dónde has llorado más?",
            "¿Puedes pagar el alquiler?",
            "¿Qué preferirías que te dejase de funcionar?",
            "¿Qué VERDUNCH eres?",
            "La otra ha muerto",
            "y punch"
        };
    }

    public void StartDialogue(int clave)
    {
        panel.SetActive(true);
        indice = clave;
        dialogText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        audioTeclado.Play();
        foreach (char c in list[indice].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(velText);
        }
        audioTeclado.Stop();
        yield return new WaitForSeconds(2f);
        panel.SetActive(false);
    }

    public void guardarEstadisticas(string campo) 
    {
        fbm.actualizarEstadistica(campo);
    }

}
