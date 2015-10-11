using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Http;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSREST.Controllers
{
    [RoutePrefix("api/votacion")]
    public class FavoritoController : ApiController
    {
        [Route("torneo/{idTorneo:int}/participante/{idParticipante:int}/favorito")]
        public IHttpActionResult PostFavorito(int idTorneo, int idParticipante, Favorito favorito)
        {
            try
            {
                favorito.IDTorneo = idTorneo;
                favorito.IDParticipante = idParticipante;

                using (var proxy = new WSFavorito.FavoritoServiceClient())
                {
                    proxy.CrearFavorito(favorito);
                
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

        [Route("torneo/{idTorneo:int}/participante/{idParticipante:int}/favorito")]
        public ICollection<Favorito> GetAllFavoritos(int idTorneo, int idParticipante)
        {
            using (var proxy = new WSFavorito.FavoritoServiceClient())
            {
                return proxy.ListarFavoritos(idTorneo, idParticipante) ?? new List<Favorito>();
            }
        }

        [Route("torneo/{enlace}")]
        public Torneo GetTorneo(string enlace)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            enlace = encoding.GetString(Convert.FromBase64String(enlace));
            enlace = HttpContext.Current.Server.UrlDecode(enlace);

            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                return proxy.ObtenerTorneoPorEnlace(enlace);
            }
        }

        [Route("torneo/{idTorneo:int}/contendor")]
        public ICollection<Contendor> GetAllContendores(int idTorneo)
        {
            using (var proxy = new WSContendor.ContendorServiceClient())
            {
                return proxy.ListarContendores(idTorneo) ?? new List<Contendor>();
            }
        }
    }
}
