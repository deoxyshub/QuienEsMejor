using System;
using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;
using WhoIsBetter.DataAccess;

namespace WhoIsBetter.WSSOAP
{
    public class TorneoService : ITorneoService
    {
        TorneoDA torneoDA;

        public TorneoService()
        {
            torneoDA = new TorneoDA();
        }

        public int CrearTorneo(int idUsuario, Torneo torneo)
        {
            if (string.IsNullOrEmpty(torneo.Nombre) || 
                torneo.FechaInicio == null || 
                torneo.FechaFin == null || 
                torneo.NumeroParticipantes == 0 || 
                torneo.NumeroContendores == 0 ||
                string.IsNullOrEmpty(torneo.Enlace))
                throw new FaultException("Información básica del torneo incompleta");

            if (torneo.NumeroParticipantes % 2 == 0)
                throw new FaultException("Participantes debe ser impar");

            if (!Validador.EsPotenciaDe2(torneo.NumeroContendores))
                throw new FaultException("Contendores debe ser potencia de 2");

            if (torneo.FechaFin < DateTime.Today)
                throw new FaultException("Fecha fin debe ser futura");

            if (torneo.FechaInicio > torneo.FechaFin)
                throw new FaultException("Fecha inicio debe ser menor a fecha fin");

            if (torneoDA.ObtenerTorneo(torneo.Nombre) != null)
                throw new FaultException("El nombre del torneo ya esta registrado");

            return torneoDA.CrearTorneo(idUsuario, torneo);
        }
        
        public Torneo ObtenerTorneoPorID(int id)
        {
            return torneoDA.ObtenerTorneo(id);
        }

        public Torneo ObtenerTorneoPorEnlace(string enlace)
        {
            return torneoDA.ObtenerTorneo(enlace);
        }
        
        public void ActualizarTorneo(Torneo torneo)
        {
            var xtorneo = torneoDA.ObtenerTorneo(torneo.ID);
            
            if (xtorneo == null)
                throw new FaultException("Torneo no existe");

            if (string.IsNullOrEmpty(torneo.Nombre) ||
                torneo.FechaInicio == null ||
                torneo.FechaFin == null ||
                torneo.NumeroParticipantes == 0 ||
                torneo.NumeroContendores == 0 ||
                string.IsNullOrEmpty(torneo.Enlace))
                throw new FaultException("Información básica del torneo incompleta");

            if (torneo.NumeroParticipantes % 2 == 0)
                throw new FaultException("Participantes debe ser impar");

            if (!Validador.EsPotenciaDe2(torneo.NumeroContendores))
                throw new FaultException("Contendores debe ser potencia de 2");

            if (torneo.FechaFin < DateTime.Today)
                throw new FaultException("Fecha fin debe ser futura");

            if (torneo.FechaInicio > torneo.FechaFin)
                throw new FaultException("Fecha inicio debe ser menor a fecha fin");
            
            torneoDA.ActualizarTorneo(torneo);
        }

        public void EliminarTorneo(int id)
        {
            torneoDA.EliminarTorneo(id);
        }

        public ICollection<Torneo> ListarTorneos(int? idUsuario)
        {
            return torneoDA.ListarTorneos(idUsuario);
        }
    }
}