using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimacionMeta : MonoBehaviour
{
    [SerializeField] Button btnIzq;
    [SerializeField] Button btnDrch;
    [SerializeField] Button btnSalto;
    [SerializeField] Transform endPoint;
    private Rigidbody2D player;
    private Animator anim;

    public GameObject panelAnimacion;
    [SerializeField] Animator animFinal;
    [SerializeField] Animator crossFade;
    [SerializeField] Animator giroPantalla;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;


    // Start is called before the first frame update
    void Start()
    {
        panelAnimacion.SetActive(false);
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Meta"))
        {
            btnIzq.gameObject.SetActive(false);
            btnDrch.gameObject.SetActive(false);
            btnSalto.gameObject.SetActive(false);
            player.GetComponent<PlayerMovement>().releaseLeft();
            player.GetComponent<PlayerMovement>().releaseRight();
            StartCoroutine(moverJugador());
        }
    }

    IEnumerator moverJugador()
    {
        //Physics2D.IgnoreLayerCollision(0, 6, true);
        player.gravityScale = 0;
        while (true)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, endPoint.transform.position, Time.deltaTime * 2.0f);
            // If the object has arrived, stop the coroutine
            if (player.transform.position == endPoint.transform.position)
            {
                StartCoroutine(animacion());
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
    }

    IEnumerator animacion()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        panelAnimacion.SetActive(true);
        yield return new WaitForSeconds(1f);
        animFinal.SetTrigger("startAnim");
        yield return new WaitForSeconds(9.6f);
        crossFade.SetTrigger("startCrossFade");
        fbm.actualizarCapitulo(idCurrent, "Escena2");
        yield return new WaitForSeconds(1f);
        giroPantalla.SetTrigger("startGiro");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Escena2");

    }
}
