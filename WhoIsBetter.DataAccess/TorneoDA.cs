using System;
using System.Collections.Generic;
using WhoIsBetter.Entity;

namespace WhoIsBetter.DataAccess
{
    public class TorneoDA : DataAccessBase
    {
        public TorneoDA() : base("DB") { }

        public int CrearTorneo(int idUsuario, Torneo torneo)
        {
            return this.ExecuteScalar<int>("usp_CrearTorneo",
                new DataParameter("@pNombre", torneo.Nombre),
                new DataParameter("@pNumeroParticipantes", torneo.NumeroParticipantes),
                new DataParameter("@pNumeroContendores", torneo.NumeroContendores),
                new DataParameter("@pFechaInicio", torneo.FechaInicio),
                new DataParameter("@pFechaFin", torneo.FechaFin),
                new DataParameter("@pEnlace", torneo.Enlace),
                new DataParameter("@pIDEstado", torneo.IDEstado),
                new DataParameter("@pIDUsuario", idUsuario)
            );
        }

        public Torneo ObtenerTorneo(int id)
        {
            return this.ExecuteEntity<Torneo>("usp_ObtenerTorneo",
                dr => new Torneo
                {
                    ID = dr.GetValue<int>("ID"),
                    Nombre = dr.GetString("Nombre"),
                    NumeroParticipantes = dr.GetValue<int>("NumeroParticipantes"),
                    NumeroRealParticipantes = dr.GetValue<int>("NumeroRealParticipantes"),
                    NumeroContendores = dr.GetValue<int>("NumeroContendores"),
                    FechaInicio = dr.GetValue<DateTime>("FechaInicio"),
                    FechaFin = dr.GetValue<DateTime>("FechaFin"),
                    Enlace = dr.GetString("Enlace"),
                    IDEstado = dr.GetValue<int>("IDEstado")
                },
                new DataParameter("@pID", id)
            );
        }

        public Torneo ObtenerTorneo(string enlace)
        {
            return this.ExecuteEntity<Torneo>("usp_ObtenerTorneo",
                dr => new Torneo
                {
                    ID = dr.GetValue<int>("ID"),
                    Nombre = dr.GetString("Nombre"),
                    NumeroParticipantes = dr.GetValue<int>("NumeroParticipantes"),
                    NumeroRealParticipantes = dr.GetValue<int>("NumeroRealParticipantes"),
                    NumeroContendores = dr.GetValue<int>("NumeroContendores"),
                    FechaInicio = dr.GetValue<DateTime>("FechaInicio"),
                    FechaFin = dr.GetValue<DateTime>("FechaFin"),
                    Enlace = dr.GetString("Enlace"),
                    IDEstado = dr.GetValue<int>("IDEstado")
                },
                new DataParameter("@pEnlace", enlace)
            );
        }

        public void ActualizarTorneo(Torneo torneo)
        {
            this.ExecuteNonQuery("usp_ActualizarTorneo",
                new DataParameter("@pID", torneo.ID),
                new DataParameter("@pNombre", torneo.Nombre),
                new DataParameter("@pNumeroParticipantes", torneo.NumeroParticipantes),
                new DataParameter("@pNumeroContendores", torneo.NumeroContendores),
                new DataParameter("@pFechaInicio", torneo.FechaInicio),
                new DataParameter("@pFechaFin", torneo.FechaFin),
                new DataParameter("@pEnlace", torneo.Enlace),
                new DataParameter("@pIDEstado", torneo.IDEstado)
            );
        }

        public void EliminarTorneo(int id)
        {
            this.ExecuteNonQuery("usp_EliminarTorneo",
                new DataParameter("@pID", id)
            );
        }

        public ICollection<Torneo> ListarTorneos(int? idUsuario = null)
        {
            return this.ExecuteEntityCollection<Torneo>("usp_ListarTorneos",
                dr => new Torneo
                {
                    ID = dr.GetValue<int>("ID"),
                    Nombre = dr.GetString("Nombre"),
                    NumeroParticipantes = dr.GetValue<int>("NumeroParticipantes"),
                    NumeroContendores = dr.GetValue<int>("NumeroContendores"),
                    FechaInicio = dr.GetValue<DateTime>("FechaInicio"),
                    FechaFin = dr.GetValue<DateTime>("FechaFin"),
                    Enlace = dr.GetString("Enlace"),
                    IDEstado = dr.GetValue<int>("IDEstado")
                },
                new DataParameter("@pIDUsuario", idUsuario)
            );
        }
    }
}