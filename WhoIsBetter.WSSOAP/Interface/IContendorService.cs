using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSSOAP
{
    [ServiceContract]
    public interface IContendorService
    {
        [OperationContract]
        int CrearContendor(Contendor contendor);

        [OperationContract]
        Contendor ObtenerContendor(int id);

        [OperationContract]
        void ActualizarContendor(Contendor contendor);

        [OperationContract]
        void EliminarContendor(int id);

        [OperationContract]
        ICollection<Contendor> ListarContendores(int idTorneo);
    }
}