using System;
using System.Collections.Generic;
using System.Web.Http;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSREST.Controllers
{
    [Authorize]
    [RoutePrefix("api/torneo")]
    public class TorneoController : ApiController
    {
        [Route("~/api/usuario/{idUsuario:int}/torneo")]
        public IHttpActionResult PostTorneo(int idUsuario, Torneo torneo)
        {
            try
            {
                using (var proxy = new WSTorneo.TorneoServiceClient())
                {
                    proxy.CrearTorneo(idUsuario, torneo);

                    return Ok(new
                    {
                        success = true
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

        [Route("{id:int}")]
        public Torneo GetTorneo(int id)
        {
            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                return proxy.ObtenerTorneoPorID(id);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult PutTorneo(int id, Torneo torneo)
        {
            try
            {
                torneo.ID = id;

                using (var proxy = new WSTorneo.TorneoServiceClient())
                {
                    proxy.ActualizarTorneo(torneo);

                    return Ok(new
                    {
                        success = true
                    });
                }
            }
            catch (Exception)
            {
                return Ok(new
                {
                    success = false,
                    message = "No se ha podido grabar el torneo, vuelva a intentarlo más tarde."
                });
            }
        }

        [Route("{id:int}")]
        public void DeleteTorneo(int id)
        {
            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                proxy.EliminarTorneo(id);
            }
        }

        [Route("~/api/usuario/{idUsuario:int}/torneo")]
        public ICollection<Torneo> GetAllTorneosPorUsuario(int idUsuario)
        {
            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                return proxy.ListarTorneos(idUsuario) ?? new List<Torneo>();
            }
        }
    }

    public class TorneoPublicoController : ApiController
    {
        [Route("~/api/torneo")]
        public ICollection<Torneo> GetAllTorneos()
        {
            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                return proxy.ListarTorneos(null);
            }
        }
    }
}
