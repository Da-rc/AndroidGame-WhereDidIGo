using System.Collections;
using TMPro;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class GraficaMuertes : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI MuerteJug;
        [SerializeField] TextMeshProUGUI MuertesResto;
        [SerializeField] TextMeshProUGUI MuertesTotal;
        private PieChart chart;
        private Serie serie;

        FirebaseManager fbm;
        AuthManager authM;
        string idCurrent;
        Estadisticas estadisticas;
        Jugador jugador;

        private void Start()
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
                    StartCoroutine(PieAdd());
                });
            });
        }

        IEnumerator PieAdd()
        {
            chart = gameObject.GetComponent<PieChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<PieChart>();
                chart.Init();
            }
            chart.EnsureChartComponent<Title>().text = "Muertes";
            chart.RemoveData();
            serie = chart.AddSerie<Pie>();
            serie.radius[0] = 0;
            serie.radius[1] = 110;
            serie.center[0] = 0.5f;
            serie.center[1] = 0.4f;
            chart.AddData(0, jugador.muertes);
            chart.AddData(0, (estadisticas.muertesTotales - jugador.muertes));
            MuerteJug.text = "Veces que has muerto: <color=red>" + jugador.muertes+"</color>";
            MuertesResto.text = "Veces que han muerto el resto de jugadores: <color=red>" + (estadisticas.muertesTotales - jugador.muertes) + "</color>";
            MuertesTotal.text = "Muertes totales: <color=red>" + estadisticas.muertesTotales + "</color>";

            yield return new WaitForSeconds(1);
        }

    }
}

