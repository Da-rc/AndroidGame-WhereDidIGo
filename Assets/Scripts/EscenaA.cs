using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaA : MonoBehaviour
{

    [SerializeField] Animator crossFade;
    [SerializeField] Animator animEspejo;
    [SerializeField] AudioSource espejoRoto;
    [SerializeField] GameObject panelAnimacionInicial;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    // Start is called before the first frame update
    void Start()
    {
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        StartCoroutine(animacion());
    }

    // Update is called once per frame

    private IEnumerator animacion()
    {
        yield return new WaitForSeconds(1f);
        animEspejo.SetTrigger("startAnim");
        yield return new WaitForSeconds(5.1f);
        espejoRoto.Play();
        yield return new WaitForSeconds(1f);
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1.4f);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return new WaitForSeconds(1f);
        Debug.Log("pantalla cambiada");
        crossFade.SetTrigger("crossFadeIn");
        Debug.Log("crossfade in");
        panelAnimacionInicial.SetActive(false);
    }
}
