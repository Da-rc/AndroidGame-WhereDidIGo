using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel2 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt1;
    [SerializeField] TextMeshProUGUI txt2;
    [SerializeField] TextMeshProUGUI txt3;
    [SerializeField] TextMeshProUGUI txt4;
    [SerializeField] TextMeshProUGUI txt5;
    [SerializeField] TextMeshProUGUI txt6;

    FirebaseManager fbm;
    AuthManager authM;
    string idCurrent;

    Estadisticas estadisticas;
    Jugador jugador;

    void Start()
    {
        fbm = new FirebaseManager();
        authM = new AuthManager();
        idCurrent = authM.obtenerID();

        fbm.leerEstadisticas(result =>
        {
            estadisticas = result;

            fbm.leerUsuario(idCurrent, result2 =>
            {
                jugador = result2;
                mostrarDatosIniciales();
            });
        });
    }

    private void mostrarDatosIniciales()
    {
        txt1.text = "EXPULSADOS \nExpulsados totales por MALAS PERSONAS: <color=red>" + estadisticas.expulsados;

        //Muertes de jugador individual
        if (jugador.muertes == 0)
        {
            txt2.text = "MUERTES INDIVIDUALES \nNo has muerto ni una sola vez ¡Increible! El resto de jugadores....bueno, ya lo ves";
        }
        else
        {
            txt2.text = "MUERTES INDIVIDUALES \nHas muerto un total de: <color=red>" + jugador.muertes + "</color> veces. Tienes que cuidar más tu vida, morch";
        }

        //Muertes globales
        if (estadisticas.muertesTotales == 0)
        {
            txt3.text = "MUERTES TOTALES \nNadie ha muerto. ¿Qué? ¿Cómo es posible?";
        }
        else
        {
            txt3.text = "MUERTES TOTALES \nEn total los jugadores han muerto: <color=red>" + estadisticas.muertesTotales + "</color> veces. Eso son muchas muertes tbh.";
        }

        //Flores
        if (estadisticas.floresSalvadas > estadisticas.floresAsesinadas)
        {
            txt4.text = "FLORES \n<color=red>" + estadisticas.floresSalvadas + "</color> flores han sido salvadas ¡Bien hecho! No como las <color=red>" + estadisticas.floresAsesinadas + "</color> personitas que han ASESINADO a flores inocentes";
        }
        else if (estadisticas.floresSalvadas == estadisticas.floresAsesinadas)
        {
            txt4.text = "FLORES \nOh wow, se han salvado <color=red>" + estadisticas.floresSalvadas + "</color> flores y se han acabado con otras <color=red>" + estadisticas.floresAsesinadas + "</color>Lo comido por lo servido supongo /shrug";
        }
        else
        {
            txt4.text = "FLORES \n<color=red>" + estadisticas.floresSalvadas + "</color> personas han logrado salvar a Flor. En cambio <color=red>" + estadisticas.floresAsesinadas + "</color> han sido ASESINADAS por autenticos MONSTRUOS";
        }

        //Insectos
        if (estadisticas.insectosSalvados > estadisticas.insectosAsesinados)
        {
            txt5.text = "INSECTOS \nSabemos que los insectos no son apreciados por todos pero <color=red>" + estadisticas.insectosSalvados + "</color> jugadores valoran todas las vidas supongo. Por otro lado <color=red>" + estadisticas.insectosAsesinados + "</color> jugadores han aplastado al insecto sin pensarlo. Normies";
        }
        else if (estadisticas.insectosSalvados == estadisticas.insectosAsesinados)
        {
            txt5.text = "INSECTOS \nOh wow, se han salvado <color=red>" + estadisticas.insectosSalvados + "</color> insectos y se han acabado con otros <color=red>" + estadisticas.insectosAsesinados + "</color>. Lo comido por lo servido supongo / shrug";
        }
        else
        {
            txt5.text = "INSECTOS \n<color=red>" + estadisticas.insectosSalvados + "</color> escasas y bondadosas almas han salvado al pobre insecto que solo quería vivir. Por otro lado, <color=red>" + estadisticas.insectosAsesinados + "</color> pobres insectos han sido masacrados. ¿POR QUÉ SOIS ASÍ?";
        }
        txt6.text = "MALOS PENSAMIENTOS \nTotal de malos pensamientos aplastados: <color=red>" + estadisticas.pensamientosAplastados + "</color>";
    }
}
