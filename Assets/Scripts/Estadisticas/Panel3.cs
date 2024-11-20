using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel3 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt1;
    [SerializeField] TextMeshProUGUI txt2;
    [SerializeField] TextMeshProUGUI txt3;
    [SerializeField] TextMeshProUGUI txt4;
    [SerializeField] TextMeshProUGUI txt5;
    [SerializeField] TextMeshProUGUI txt6;

    FirebaseManager fbm;

    Estadisticas estadisticas;

    void Start()
    {
        fbm = new FirebaseManager();

        fbm.leerEstadisticas(result =>
        {
            estadisticas = result;

            mostrarDatosSecundarios();
        });
    }
    private void mostrarDatosSecundarios()
    {
        //Colores
        if (estadisticas.verde > estadisticas.rojo)
        {
            txt1.text = "VERDE vs ROJO \n<color=green>" + estadisticas.verde + "</color> jugadores han preferido el verde frente a <color=red>" + estadisticas.rojo + "</color> personas que han elegido el rojo. El verde no es un color creativo.";
        }
        else
        {
            txt1.text = "VERDE vs ROJO \n<color=red>" + estadisticas.rojo + "</color> personitas con sentido común han elegido el color rojo frente a <color=green>" + estadisticas.verde + "</color> enfermos que han escogido el verde.";
        }

        //Perros y gatos
        txt2.text = "PERROS vs GATOS \n<color=red>" + estadisticas.gato + "</color> gatos y <color=red>" + estadisticas.perro + "</color> perros han sido salvados. Otros <color=red>" + (estadisticas.gato + estadisticas.perro) + "</color> animalitos perdieron su vida tragicamente. DEP";

        //Cama y ducha
        if (estadisticas.cama > estadisticas.ducha)
        {
            txt3.text = "CAMAS vs DUCHAS \n<color=red>" + estadisticas.cama + "</color> personas que prefieren llorar en la cama frente a <color=red>" + estadisticas.ducha + "</color> que prefieren la ducha. Ea ea, ya pasó";
        }
        else
        {
            txt3.text = "CAMAS vs DUCHAS \n<color=red>" + estadisticas.ducha + "</color> personas que prefieren llorar en la ducha frente a <color=red>" + estadisticas.cama + "</color> que prefieren la cama. Ea ea, ya pasó";
        }

        //alquiler
        txt4.text = "ALQUILER \nEn cuanto al alquiler...<color=red>NADIE</color> puede pagarlo, no hace falta mostrar ningun dato.";

        //Wifi o corazon
        if (estadisticas.wifi > estadisticas.corazon)
        {
            txt5.text = "WIFI vs CORAZON \n<color=red>" + estadisticas.wifi + "</color> personitas preferirían que se les estropee el wifi frente a las <color=red>" + estadisticas.corazon + "</color> que preferirían que fuese el corazón. Ahora resulta" +
                " que la mayoría está bien de la cabecita.";
        }
        else
        {
            txt5.text = "WIFI vs CORAZON \n<color=red>" + estadisticas.corazon + "</color> personitas preferirían que les dejase de funcionar el corazón frente a las <color=red>" + estadisticas.wifi + "</color> que preferirían que fuese el wifi. (Buscad ayuda)";
        }

        //VERDUNCH
        txt6.text = "VERdUNCH vs VERDUNCH \n<color=red>" + estadisticas.verdunchIzq + "</color> jugadores SON verdunch de la izquierda y<color=red> " + estadisticas.verdunchDrch + "</color> SON verdunch de la derecha. <color=red>" + (estadisticas.verdunchIzq + estadisticas.verdunchDrch) + "</color> verdunches han sido sacrificadas. DEPCH";
    }

}
