using System.Collections.Generic;
using System.Linq;
using WhoIsBetter.Entity;
using WhoIsBetter.DataAccess;

namespace WhoIsBetter.WSSOAP
{
    public class ReporteService : IReporteService
    {
        ReporteDA reporteDA;
        Dictionary<int, Dictionary<int, int>> scoress;
        Dictionary<int, Dictionary<int, Contendor>> ganadoress;
        List<ReporteTorneo> lst;
        long position;

        public ReporteService()
        {
            reporteDA = new ReporteDA();
        }

        public List<ReporteTorneo> GenerarReporteTorneo(int idTorneo)
        {
            lst = new List<ReporteTorneo>();
            position = 1;
            scoress = new Dictionary<int, Dictionary<int, int>>();
            ganadoress = new Dictionary<int, Dictionary<int, Contendor>>();

            List<ReporteTorneo> source = reporteDA.GenerarReporteTorneo(idTorneo).ToList();

            if (!source.Any())
            {
                return null;
            }

            scoress.Add(0, new Dictionary<int, int>());
            scoress.Add(1, new Dictionary<int, int>());
            scoress.Add(2, new Dictionary<int, int>());
            ganadoress.Add(0, new Dictionary<int, Contendor>());
            ganadoress.Add(1, new Dictionary<int, Contendor>());
            ganadoress.Add(2, new Dictionary<int, Contendor>());

            var lstCount = 0;

            for (int i = 0; i < source.Count; i++)
            {
                ReporteTorneo row = source[i];

                Contendor cx1 = row.Contendor1;

                if (row.PosFixture == 1)
                {

                    GenerarPuntaje(0, row);

                    if (cx1.AgrupadorInicial != source[i + 1].Contendor1.AgrupadorInicial)
                    {
                        ReporteTorneo rpt = new ReporteTorneo();
                        rpt.Torneo = row.Torneo;
                        rpt.Contendor1 = row.Contendor1;
                        rpt.Contendor2 = row.Contendor2;
                        CalcularGanador(0, rpt, source, i);
                    }
                }
                else
                {
                    if (position == row.PosFixture)
                    {
                        continue;
                    }
                    position = row.PosFixture;
                    var skippedList = lst.Skip(lstCount).ToList();
                    lstCount = skippedList.Count;
                    //se busca los ganadores de la ronda anterior como competidores de la siguiente ronda
                    for (int j = 0; j < lstCount; j += 2)
                    {
                        if ((j + 1) == lstCount) { break; }

                        Contendor g1 = skippedList[j].Ganador;
                        Contendor g2 = skippedList[j + 1].Ganador;

                        bool _break = false;
                        bool cal = false;

                        for (int h = i; h < source.Count; h++)
                        {
                            row = source[h];

                            if (position != row.PosFixture) { _break = true; break; }

                            cx1 = row.Contendor1;
                            Contendor cx2 = row.Contendor2;

                            bool criterio = (cx1.ID == g1.ID && cx2.ID == g2.ID);
                            if (criterio) { GenerarPuntaje(1, row); cal = true; }
                        }

                        if (_break == true && cal == false) { break; }

                        if (cal)
                        {
                            /*Para que lo USO !*/
                            ReporteTorneo rpt = new ReporteTorneo();
                            rpt.Torneo = skippedList[j].Torneo;
                            rpt.Contendor1 = g1;
                            rpt.Contendor2 = g2;
                            CalcularGanador(1, rpt, source, i);
                        }
                    }
                }
            }

            return lst;
        }

        private void GenerarPuntaje(int scoressKey, ReporteTorneo row)
        {
            Dictionary<int, int> scores = scoress[scoressKey];
            Dictionary<int, Contendor> ganadores = ganadoress[scoressKey];
            int val = 0;

            Contendor ganador = row.Ganador;
            int id = ganador.ID;

            //validar si existe ganador
            if (scores.ContainsKey(id))
            {
                val = scores[id];
            }
            else
            {
                if (ganadores.ContainsKey(id))
                {
                    ganadores[id] = ganador;
                }
                else
                {
                    ganadores.Add(id, ganador);
                }
            }

            //incrementar el puntaje del ganador
            if (scores.ContainsKey(id))
            {
                scores[id] = val + 1;
            }
            else
            {
                scores.Add(id, val + 1);
            }
            
        }

        private void CalcularGanador(int key, ReporteTorneo rpt, List<ReporteTorneo> source, int i)
        {

            Dictionary<int, int> scores = scoress[key];
            Dictionary<int, Contendor> ganadores = ganadoress[key];
            int val = 0;
            bool addRpt = false;
            
            foreach (var k in scores.Keys)
            {
                int meKey = (int)k;
                int meValue = (int)scores[k];

                //ir almacenando al ganador si su puntaje es mayor al del anterior
                if (meValue > val)
                {
                    addRpt = true;
                    val = meValue;
                    rpt.Ganador = (Contendor)ganadores[meKey];
                }
            }

            if (addRpt) { lst.Add(rpt); }

            //cleaning Objects
            if (scoress.ContainsKey(key))
            {
                scoress[key] = new Dictionary<int, int>();
            }
            else
            {
                scoress.Add(key, new Dictionary<int, int>());
            }

            if (ganadoress.ContainsKey(key))
            {
                ganadoress[key] = new Dictionary<int, Contendor>();
            }
            else
            {
                ganadoress.Add(key, new Dictionary<int, Contendor>());
            }
        }
    }
}