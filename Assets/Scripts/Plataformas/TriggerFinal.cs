using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerFinal : MonoBehaviour
{
    [SerializeField] Animator crossFade;
    [SerializeField] Animator giroPantalla;
    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    private void Start()
    {
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            fbm.actualizarCapitulo(idCurrent, "Escena7");
            //Dialogo
            StartCoroutine(siguienteEscena());
            //Destroy(gameObject);
        }
    }

    private IEnumerator siguienteEscena()
    {
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(3f);
        giroPantalla.SetTrigger("startGiro");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Escena7");
    }
}
