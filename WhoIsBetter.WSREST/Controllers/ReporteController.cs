using System.Collections.Generic;
using System.Web.Http;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSREST.Controllers
{
    [RoutePrefix("api/reporte")]
    public class ReporteController : ApiController
    {
        [Route("~/api/torneo/{idTorneo:int}/fixture")]
        public ICollection<ReporteTorneo> GetReporteTorneo(int idTorneo)
        {
            using (var proxy = new WSReporte.ReporteServiceClient())
            {
                return proxy.GenerarReporteTorneo(idTorneo) ?? new List<ReporteTorneo>();
            }
        }
    }
}
