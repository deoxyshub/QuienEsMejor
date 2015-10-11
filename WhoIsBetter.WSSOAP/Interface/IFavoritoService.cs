using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSSOAP
{
    [ServiceContract]
    public interface IFavoritoService
    {
        [OperationContract]
        void CrearFavorito(Favorito favorito);
        
        [OperationContract]
        ICollection<Favorito> ListarFavoritos(int idTorneo, int idParticipante);
    }
}