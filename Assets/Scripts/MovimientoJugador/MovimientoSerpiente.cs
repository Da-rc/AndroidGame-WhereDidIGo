using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class Movimiento2 : MonoBehaviour
{
    [SerializeField] Transform endPoint;
    private Rigidbody2D player;
    private Animator anim;
    [SerializeField] private Animator crossFade;
    [SerializeField] private Animator animFinal;
    [SerializeField] private GameObject panel;
    [SerializeField] private AudioSource audioLatido;
    private float speedForce;

    private Touch touchInicial, touchActual;

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

        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player.drag = 2f; //controla el LineaDrag evitando que el personaje se deslize mucho cuanod se suelta el botón de movimiento
        player.gravityScale = 0f; //al modificar el linearDrag tmb afecta a la gravedad, por eso es necesario cambiarla también

        //Si es para el otro tipo de touch ponerla a 16f
        speedForce = 6f;

        StartCoroutine(moverJugador());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touchActual = Input.GetTouch(0);
        }

        switch (touchActual.phase)
        {
            case TouchPhase.Began:
                touchInicial.position = touchActual.position;
                break;

            case TouchPhase.Moved:
                if (touchInicial.position.x < touchActual.position.x)
                {
                    player.velocity = new Vector2(speedForce, 0f);
                }

                if (touchInicial.position.x > touchActual.position.x)
                {
                    player.velocity = new Vector2(-speedForce, 0f);
                }
                break;

            case TouchPhase.Ended:
                player.velocity = new Vector2(0f, 0f);
                break;
        }
        /* if (Input.touchCount > 0)
        {
            touchActual = Input.GetTouch(0);
        }

        switch (touchActual.phase)
        {
            case TouchPhase.Began:
                if (touchActual.position.x < Screen.width / 2)
                {
                    player.velocity = new Vector2(-speedForce, 0f);
                }

                if (touchInicial.position.x > Screen.width / 2)
                {
                    player.velocity = new Vector2(speedForce, 0f);
                }
                break;
               
            case TouchPhase.Ended:
                player.velocity = new Vector2(0f, 0f);
                break;
        }*/

    }

    IEnumerator moverJugador()
    {
        player.gravityScale = 0;
        while (true)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, endPoint.transform.position, Time.deltaTime * 2.0f);
            yield return null;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Meta"))
        {
            anim.SetTrigger("eat");
            StartCoroutine(animacionFinal());
        }
    }

    IEnumerator animacionFinal() 
    {
        yield return new WaitForSeconds(2f);
        panel.SetActive(true);
        animFinal.SetTrigger("start");
        yield return new WaitForSeconds(5f);
        fbm.actualizarCapitulo(idCurrent, "Escena3");
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        audioLatido.Play();
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("Escena3");
    }
}
