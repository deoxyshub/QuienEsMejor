using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;
using WhoIsBetter.DataAccess;

namespace WhoIsBetter.WSSOAP
{
    public class ParticipanteService : IParticipanteService
    {
        ParticipanteDA participanteDA = null;

        public ParticipanteService()
        {
            participanteDA = new ParticipanteDA();
        }

        public int InscribirParticipante(int idTorneo, Participante participante)
        {
            if (string.IsNullOrEmpty(participante.Correo) || string.IsNullOrEmpty(participante.Nombre))
                throw new FaultException("Información básica del participante incompleta");

            if (!Validador.EsCorreo(participante.Correo))
                throw new FaultException("Formato de correo incorrecto");
            
            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxy.ObtenerTorneoPorID(idTorneo);

                if (torneo.IDEstado == 3 || torneo.IDEstado == 4)
                    throw new FaultException("Torneo ya no se encuentra vigente");

                var entity = participanteDA.ObtenerParticipante(idTorneo, participante.Correo);

                if (entity != null) return entity.ID;

                torneo.NumeroRealParticipantes++;

                if (torneo.NumeroRealParticipantes >= torneo.NumeroParticipantes)
                    throw new FaultException("Torneo llegó al límite de participantes");
                
                var idParticipante = participanteDA.InscribirParticipante(idTorneo, participante);
                
                return idParticipante;
            }
        }
    }
}