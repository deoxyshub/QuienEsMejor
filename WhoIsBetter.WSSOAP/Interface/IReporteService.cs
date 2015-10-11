using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSSOAP
{
    [ServiceContract]
    public interface IReporteService
    {
        [OperationContract]
        List<ReporteTorneo> GenerarReporteTorneo(int idTorneo);
    }
}