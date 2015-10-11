using System.ServiceModel;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSSOAP
{
    [ServiceContract]
    public interface IParticipanteService
    {
        [OperationContract]
        int InscribirParticipante(int idTorneo, Participante participante);
    }
}
