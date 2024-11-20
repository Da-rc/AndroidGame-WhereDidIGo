using System.Collections;
using UnityEngine;
using XCharts.Runtime;
namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class GraficaFinales : MonoBehaviour
    {
        private BarChart chart;
        private Serie serie;
        FirebaseManager fbm;
        Estadisticas estadisticas;
        private void Start()
        {
            fbm = new FirebaseManager();

            fbm.leerEstadisticas(result =>
            {
                estadisticas = result;

                StartCoroutine(AddSimpleBar());
            });
        }

        IEnumerator AddSimpleBar()
        {
            chart = gameObject.GetComponent<BarChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<BarChart>();
                chart.Init();
            }
            chart.EnsureChartComponent<Title>().text = "Finales más jugados";
            var yAxis = chart.EnsureChartComponent<YAxis>();
            yAxis.minMaxType = Axis.AxisMinMaxType.Default;
            chart.RemoveData();
            serie = chart.AddSerie<Bar>("Bar1");
            chart.AddXAxisData("TrueGood");
            chart.AddData(0, estadisticas.finalTrueBueno);
            chart.AddXAxisData("Good");
            chart.AddData(0, estadisticas.finalBueno);
            chart.AddXAxisData("Evil");
            chart.AddData(0, estadisticas.finalMalo);
            chart.AddXAxisData("TrueEvil");
            chart.AddData(0, estadisticas.finalTrueMalo);
            chart.AddXAxisData("Alternativo");
            chart.AddData(0, estadisticas.finalAlternativo);
            yield return new WaitForSeconds(1);
        }
    }
}
