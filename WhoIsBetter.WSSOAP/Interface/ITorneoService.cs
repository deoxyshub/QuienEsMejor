using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSSOAP
{
    [ServiceContract]
    public interface ITorneoService
    {
        [OperationContract]
        int CrearTorneo(int idUsuario, Torneo torneo);
        
        [OperationContract]
        Torneo ObtenerTorneoPorID(int id);

        [OperationContract]
        Torneo ObtenerTorneoPorEnlace(string enlace);

        [OperationContract]
        void ActualizarTorneo(Torneo torneo);

        [OperationContract]
        void EliminarTorneo(int id);

        [OperationContract]
        ICollection<Torneo> ListarTorneos(int? idUsuario);
    }
}