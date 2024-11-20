using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum estadoBatallaBicho { START, TURNOENEMIGO, TURNOJUGADOR, BICHOASESINADO, LOST }
public class SistemaCombateBicho : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogo;
    [SerializeField] scripDatosEnemigo enemigoHUD;
    [SerializeField] scriptDatosJugador jugadorHUD;
    [SerializeField] Animator animBicho;
    [SerializeField] Animator animJugador;
    [SerializeField] Animator crossFade;
    [SerializeField] AudioSource audioHit;
    [SerializeField] AudioSource audioHeal;
    [SerializeField] AudioSource audioAttack;
    [SerializeField] AudioSource audioBackground;
    [SerializeField] GameObject muerteCombate;


    BichoEnemigo BichoEnemigo;
    UnitJugador unitJugador;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;


    public estadoBatallaBicho estado;

    private void Awake()
    {
        muerteCombate.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        estado = estadoBatallaBicho.START;
        BichoEnemigo = new BichoEnemigo();
        unitJugador = new UnitJugador();
        StartCoroutine(setupBattle());
    }

    IEnumerator setupBattle()
    {
        dialogo.text = "Has decidido atacar al insecto...qué decepción";
        //le pasamos a los scripts del HUD de cada personaje el Unit script con toda la información
        jugadorHUD.setHUD(unitJugador);
        enemigoHUD.setHUDbicho(BichoEnemigo);


        yield return new WaitForSeconds(2f);

        //cambiamos el estado al turno del jugador
        estado = estadoBatallaBicho.TURNOJUGADOR;
        turnoJugador();
    }

    private void turnoJugador()
    {
        dialogo.text = "Elige una acción:";
    }

    IEnumerator ataqueJugador()
    {
        //cambiamos hp del enemigo
        animJugador.SetTrigger("jugAttack");
        animBicho.SetTrigger("bichoIsHit");
        audioHit.Play();
        bool isDead = BichoEnemigo.recibirGolpe(unitJugador.damage);

        //actualizamos HUD
        enemigoHUD.setHP(BichoEnemigo.currentHP);
        dialogo.text = "¡Vaya golpe!";

        if (isDead)
        {
            estado = estadoBatallaBicho.BICHOASESINADO;
            yield return new WaitForSeconds(1f);
            finalBatalla();
        }
        else
        {
            estado = estadoBatallaBicho.TURNOENEMIGO;
            yield return new WaitForSeconds(1f);
            StartCoroutine(turnoEnemigo());
        }
    }

    IEnumerator curaJugador()
    {
        animJugador.SetTrigger("jugHeal");
        audioHeal.Play();
        unitJugador.curar(10);

        jugadorHUD.setHP(unitJugador.currentHP);
        dialogo.text = "¡Te sientes mucho mejor!";

        estado = estadoBatallaBicho.TURNOENEMIGO;
        yield return new WaitForSeconds(2f);
        StartCoroutine(turnoEnemigo());
    }


    IEnumerator turnoEnemigo()
    {
        dialogo.text = BichoEnemigo.unitName + " va a atacar";

        yield return new WaitForSeconds(1f);

        animBicho.SetTrigger("bichoAttack");
        audioAttack.Play();
        yield return new WaitForSeconds(0.2f);
        animJugador.SetTrigger("jugIsHit");
        audioHit.Play();
        bool isDead = unitJugador.recibirGolpe(BichoEnemigo.damage);
        jugadorHUD.setHP(unitJugador.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            estado = estadoBatallaBicho.LOST;
            finalBatalla();
        }
        else
        {
            estado = estadoBatallaBicho.TURNOJUGADOR;
            turnoJugador();
        }
    }

    private void finalBatalla()
    {
        if (estado == estadoBatallaBicho.BICHOASESINADO)
        {
            dialogo.text = "está muerto, ya no florecerá...";
            fbm.actualizarEstadistica("insectosAsesinados");
            //No guardamos en la base de dato que está muerta porque ya se crea en false por defecto
            fbm.actualizarCapitulo(idCurrent, "Escena8");
            StartCoroutine(siguienteEscena());
        }
        else if (estado == estadoBatallaBicho.LOST)
        {
            dialogo.text = "Has muerto";
            //Se ha perdido
            //guardar en base de datos que se ha muerto 1 vez
            fbm.actualizarEstadistica("muertesTotales");
            fbm.sumarMuerte(idCurrent);
            //pantalla de muerte y vuelta a cargar escena
            StartCoroutine(escenaMuerte());
        }
    }

    //*********************************
    //***********BOTONES***************
    //*********************************

    public void botonAtaque()
    {
        if (estado != estadoBatallaBicho.TURNOJUGADOR) return;

        StartCoroutine(ataqueJugador());
    }

    public void botonCurar()
    {
        if (estado != estadoBatallaBicho.TURNOJUGADOR) return;

        StartCoroutine(curaJugador());
    }

    private IEnumerator siguienteEscena()
    {
        yield return new WaitForSeconds(3f);
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Escena8");
    }

    private IEnumerator escenaMuerte()
    {
        yield return new WaitForSeconds(1f);
        crossFade.SetTrigger("startCrossFade");
        audioBackground.Stop();
        yield return new WaitForSeconds(1f);
        muerteCombate.SetActive(true);
    }
}
