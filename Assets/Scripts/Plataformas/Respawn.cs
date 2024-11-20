using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] List<Transform> respawnPoints;
    private Rigidbody2D player;
    private Animator anim;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    int respawnActivo;


    // Start is called before the first frame update
    void Start()
    {
        respawnActivo = 0;
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vacio") || collision.gameObject.CompareTag("enemigo"))
        {
            player.bodyType = RigidbodyType2D.Static;
            fbm.actualizarEstadistica("muertesTotales");
            fbm.sumarMuerte(idCurrent);
            anim.SetTrigger("Muerte");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NuevoRespawn"))
        {
            respawnActivo = 1;
            Destroy(collision.gameObject);
        }
    }


    public void respawnear()
    {
        player.bodyType = RigidbodyType2D.Dynamic;
        player.transform.position = respawnPoints[respawnActivo].position;
        anim.SetTrigger("Respawn");
    }
}
