using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
//Controlador principal del sistema de combate por turnos
public enum estadoBatalla { START, TURNOENEMIGO, TURNOJUGADOR, FLORASESINADA, FLORSALVADA, LOST}

public class SistemaCombate : MonoBehaviour
{
    // [SerializeField] GameObject jugador;
    //[SerializeField] GameObject enemigo;

    //[SerializeField] Transform spawnEnemigo;
    //[SerializeField] Transform spawnJugador;

    [SerializeField] TextMeshProUGUI dialogo;
    [SerializeField] scripDatosEnemigo enemigoHUD;
    [SerializeField] scriptDatosJugador jugadorHUD;
    [SerializeField] Animator animFlor;
    [SerializeField] Animator animJugador;
    [SerializeField] Animator crossFade;
    [SerializeField] AudioSource audioHit;
    [SerializeField] AudioSource audioHeal;
    [SerializeField] AudioSource audioAttack;
    [SerializeField] AudioSource audioBackground;
    [SerializeField] GameObject muerteCombate;


    List<string> textos;
    UnitEnemigo unitEnemigo;
    UnitJugador unitJugador;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    //para la opcion de hablar y la salida pacifica
    int contadorFrases;

    public estadoBatalla estado;

    // Start is called before the first frame update
    void Start()
    {
        textos = new List<string>();
        textos.Add("Espera, no tenemos por qué pelear");
        textos.Add("¿Por qué estás haciendo esto?");
        textos.Add("¿Qué te ha pasado? Podemos solucionar esto");
        textos.Add("Por favor...ugh...para");
        textos.Add("...");
        textos.Add("YA ESTÁ BIEN. TIENES QUE PARAR");

        crossFade.SetTrigger("crossFadeIn");
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();
        contadorFrases = 0;
        estado = estadoBatalla.START;
        unitEnemigo = new UnitEnemigo();
        unitJugador = new UnitJugador();
        StartCoroutine(setupBattle());
    }

    IEnumerator setupBattle() 
    {
        dialogo.text = "Ha aparecido " +unitEnemigo.unitName+ " y parece muy enfadada";
        //le pasamos a los scripts del HUD de cada personaje el Unit script con toda la información
        jugadorHUD.setHUD(unitJugador);
        enemigoHUD.setHUD(unitEnemigo);


        yield return new WaitForSeconds(2f);

        //cambiamos el estado al turno del jugador
        estado = estadoBatalla.TURNOJUGADOR;
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
        animFlor.SetTrigger("florIsHit"); 
        audioHit.Play();
        bool isDead = unitEnemigo.recibirGolpe(unitJugador.damage);

        //actualizamos HUD
        enemigoHUD.setHP(unitEnemigo.currentHP);
        dialogo.text = "¡Vaya golpe!";

        if (isDead)
        {
            estado = estadoBatalla.FLORASESINADA;
            yield return new WaitForSeconds(1f);
            finalBatalla();
        }
        else
        {
            estado = estadoBatalla.TURNOENEMIGO;
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

        estado = estadoBatalla.TURNOENEMIGO;
        yield return new WaitForSeconds(2f);
        StartCoroutine(turnoEnemigo());
    }

    IEnumerator hablar()
    {
        dialogo.text = textos[contadorFrases];
        contadorFrases++;

        if (contadorFrases == textos.Count)
        {
            estado = estadoBatalla.FLORSALVADA;
            yield return new WaitForSeconds(2f);
            dialogo.text = string.Empty;
            yield return new WaitForSeconds(1.5f);
            dialogo.text = "Flor: 'Yo...lo-lo siento...'";
            yield return new WaitForSeconds(2f);
            finalBatalla();
        }
        else
        {
            estado = estadoBatalla.TURNOENEMIGO;
            yield return new WaitForSeconds(2f);
            StartCoroutine(turnoEnemigo());
        }
    }

    IEnumerator turnoEnemigo() 
    {
        dialogo.text = unitEnemigo.unitName+ " va a atacar";

        yield return new WaitForSeconds(1f);

        animFlor.SetTrigger("florAttack");
        audioAttack.Play();
        yield return new WaitForSeconds(0.2f);
        animJugador.SetTrigger("jugIsHit");
        audioHit.Play();
        bool isDead = unitJugador.recibirGolpe(unitEnemigo.damage);
        jugadorHUD.setHP(unitJugador.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead) 
        {
            estado = estadoBatalla.LOST;
            finalBatalla();
        }
        else 
        {
            estado = estadoBatalla.TURNOJUGADOR;
            turnoJugador();
        }
    }

    private void finalBatalla() 
    {
        if (estado == estadoBatalla.FLORASESINADA)
        {
            dialogo.text = "oh...está muerta...";
            fbm.actualizarHito(idCurrent, "listaHitos.florSalvada", false);
            fbm.actualizarEstadistica("floresAsesinadas");
            fbm.actualizarCapitulo(idCurrent, "Escena5");
            StartCoroutine(siguienteEscena());
        }
        else if (estado == estadoBatalla.FLORSALVADA) 
        {
            dialogo.text = "Flor: 'no...no sé qué ha pasados...y-yo te vi y sentí dolor y miedo...y-y lo siento...'";
            //Guardar en la base de dato que flor ha sido salvada
            fbm.actualizarHito(idCurrent, "listaHitos.florSalvada", true);
            fbm.actualizarEstadistica("floresSalvadas");
            fbm.actualizarCapitulo(idCurrent, "Escena5");
            StartCoroutine(siguienteEscena());
        }
        else if(estado == estadoBatalla.LOST)
        {
            dialogo.text = "Oye...has muerto";
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
        if (estado != estadoBatalla.TURNOJUGADOR) return;

        StartCoroutine(ataqueJugador());
    }

    public void botonCurar()
    {
        if (estado != estadoBatalla.TURNOJUGADOR) return;

        StartCoroutine(curaJugador());
    }

    public void botonHablar() 
    {
        if (estado != estadoBatalla.TURNOJUGADOR) return;

        StartCoroutine(hablar());
    }

    private IEnumerator siguienteEscena() 
    {
        yield return new WaitForSeconds(3f);
        crossFade.SetTrigger("startCrossFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Escena5");
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
