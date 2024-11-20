using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiJumpCheck : MonoBehaviour
{
    Rigidbody2D player;
    [SerializeField] AudioSource muerteEnemi;
    private float jumpForce;
    FirebaseManager fbm;

    private void Start()
    {
        fbm = new FirebaseManager();
        player = transform.parent.GetComponent<Rigidbody2D>();
        jumpForce = 16f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemigo")) 
        {
            player.velocity = new Vector2(player.velocity.x, jumpForce);
            fbm.actualizarEstadistica("pensamientosAplastados");
            collision.GetComponent<followWayPoint>().parar = true;
            muerteEnemi.Play();
            collision.GetComponent<Animator>().SetTrigger("death");
            StartCoroutine(destroyObject(collision.gameObject));
        }
    }

    IEnumerator destroyObject(GameObject enemigo) 
    {
        yield return new WaitForSeconds(1f);
        Destroy(enemigo);
    }
}
