using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WhoIsBetter.Entity;

namespace WhoIsBetter.DataAccess
{
    public class ReporteDA : DataAccessBase
    {
        public ReporteDA() : base("DB") { }

        public ICollection<ReporteTorneo> GenerarReporteTorneo(int idTorneo)
        {
            Func<bool, SqlDataReader, string, object> obtenerGanador = (flg, dr, id) => 
            {
                return dr[id + (flg ? "1" : "2")];
            };

            return this.ExecuteEntityCollection<ReporteTorneo>("usp_ReporteTorneo",
                dr => new ReporteTorneo
                {
                    Torneo = new Torneo
                    {
                        ID = dr.GetValue<int>("IDTorneo"),
                        Nombre = dr.GetString("NombreTorneo"),
                        NumeroContendores = dr.GetValue<int>("NumeroContendores"),
                        FechaInicio = dr.GetValue<DateTime>("FechaInicio"),
                        FechaFin = dr.GetValue<DateTime>("FechaFin"),
                    },
                    Participante = new Participante
                    {
                        ID = dr.GetValue<int>("IDParticipante"),
                        Nombre = dr.GetString("NombreParticipante"),
                        Correo = dr.GetString("CorreoParticipante")
                    },
                    Contendor1 = new Contendor
                    {
                        ID = dr.GetValue<int>("IDContendor1"),
                        Nombre = dr.GetString("NombreContendor1"),
                        Texto = dr.GetString("TextoContendor1"),
                        RutaImagen = dr.GetString("RutaImagenContendor1"),
                        AgrupadorInicial = dr.GetValue<long>("AgrupadorInicial")
                    },
                    Contendor2 = new Contendor
                    {
                        ID = dr.GetValue<int>("IDContendor2"),
                        Nombre = dr.GetString("NombreContendor2"),
                        Texto = dr.GetString("TextoContendor2"),
                        RutaImagen = dr.GetString("RutaImagenContendor2"),
                        AgrupadorInicial = dr.GetValue<long>("AgrupadorInicial")
                    },
                    Ganador = new Contendor
                    {
                        ID = (int)obtenerGanador(dr.GetValue<int>("IDGanador") == dr.GetValue<int>("IDContendor1"), dr, "IDContendor"),
                        Nombre = (string)obtenerGanador(dr.GetValue<int>("IDGanador") == dr.GetValue<int>("IDContendor1"), dr, "NombreContendor"),
                        Texto = (string)obtenerGanador(dr.GetValue<int>("IDGanador") == dr.GetValue<int>("IDContendor1"), dr, "TextoContendor"),
                        RutaImagen = (string)obtenerGanador(dr.GetValue<int>("IDGanador") == dr.GetValue<int>("IDContendor1"), dr, "RutaImagenContendor")
                    },
                    PosFixture = dr.GetValue<long>("PosFixture")
                },
                new DataParameter("@pIDTorneo", idTorneo)
            );
        }
    }
}