using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;
using WhoIsBetter.DataAccess;
using System.Messaging;
using System;
using System.Linq;

namespace WhoIsBetter.WSSOAP
{
    public class FavoritoService : IFavoritoService
    {
        FavoritoDA favoritoDA = null;

        public FavoritoService()
        {
            favoritoDA = new FavoritoDA();
        }

        public void CrearFavorito(Favorito favorito)
        {
            if (favorito.IDContendor1 == favorito.IDContendor2)
                throw new FaultException("Contendor 1 y 2 no pueden ser el mismo");

            if (favorito.IDGanador != favorito.IDContendor1 &&
                favorito.IDGanador != favorito.IDContendor2)
                throw new FaultException("El ganador no corresponde a los contendores");

            favoritoDA.CrearFavorito(favorito);
            
            var rutaCola = @".\private$\DSWarrior2";

            if (!MessageQueue.Exists(rutaCola))
            {
                MessageQueue.Create(rutaCola);
            }

            var cola = new MessageQueue(rutaCola);
            var mensaje = new Message();
            mensaje.Label = "Registrar Favorito";
            mensaje.Body = favorito;
            cola.Send(mensaje);
        }
        
        public ICollection<Favorito> ListarFavoritos(int idTorneo, int idParticipante)
        {
            string rutaCola = @".\private$\DSWarrior2";

            if (!MessageQueue.Exists(rutaCola))
            {
                MessageQueue.Create(rutaCola);
            }

            var cola = new MessageQueue(rutaCola);
            cola.Formatter = new XmlMessageFormatter(new Type[] { typeof(Favorito) });

            var lista = new List<Favorito>();
            var messages = cola.GetAllMessages();
            var totalMessages = messages.Count();

            for (int i = 0; i < totalMessages; i++)
            {
                var mensaje = messages[i];
                var favorito = (Favorito)mensaje.Body;

                if (favorito.IDTorneo == idTorneo && favorito.IDParticipante == idParticipante) {
                    lista.Add(favorito);
                }
            }

            return lista;
            //return favoritoDA.ListarFavoritos(idTorneo, idParticipante);
        }
    }
}