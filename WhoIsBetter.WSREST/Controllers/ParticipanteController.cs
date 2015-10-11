using System;
using System.Web.Http;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSREST.Controllers
{
    [RoutePrefix("api/participante")]
    public class ParticipanteController : ApiController
    {
        [Route("~/api/torneo/{idTorneo:int}/participante")]
        public IHttpActionResult PostParticipante(int idTorneo, Participante participante)
        {
            try
            {
                using (var proxy = new WSParticipante.ParticipanteServiceClient())
                {
                    var id = proxy.InscribirParticipante(idTorneo, participante);

                    return Ok(new
                    {
                        success = true,
                        idParticipante = id
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
