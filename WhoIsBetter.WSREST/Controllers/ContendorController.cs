using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WhoIsBetter.Entity;
using Newtonsoft.Json;
using System.IO;

namespace WhoIsBetter.WSREST.Controllers
{
    [Authorize]
    [RoutePrefix("api/contendor")]
    public class ContendorController : ApiController
    {
        [Route("~/api/torneo/{idTorneo:int}/contendor")]
        public IHttpActionResult PostContendor(int idTorneo)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var archivo = HttpContext.Current.Request.Files[0];
                var contendor = JsonConvert.DeserializeObject<Contendor>(HttpContext.Current.Request.Form["entity"]);
                var carpeta = Path.GetDirectoryName(Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/")));
                var path = Path.Combine(carpeta, "uploads", archivo.FileName);

                archivo.SaveAs(path);

                contendor.IDTorneo = idTorneo;
                contendor.RutaImagen = path;

                using (var proxy = new WSContendor.ContendorServiceClient())
                {
                    proxy.CrearContendor(contendor);

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
        public Contendor GetContendor(int id)
        {
            using (var proxy = new WSContendor.ContendorServiceClient())
            {
                return proxy.ObtenerContendor(id);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult PutContendor(int id)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var archivo = HttpContext.Current.Request.Files[0];
                var contendor = JsonConvert.DeserializeObject<Contendor>(HttpContext.Current.Request.Form["entity"]);
                var carpeta = Path.GetDirectoryName(Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/")));
                var path = Path.Combine(carpeta, "uploads", archivo.FileName);

                archivo.SaveAs(path);

                contendor.RutaImagen = path;

                using (var proxy = new WSContendor.ContendorServiceClient())
                {
                    proxy.ActualizarContendor(contendor);

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
        public void DeleteContendor(int id)
        {
            using (var proxy = new WSContendor.ContendorServiceClient())
            {
                proxy.EliminarContendor(id);
            }
        }

        [Route("~/api/torneo/{idTorneo:int}/contendor")]
        public ICollection<Contendor> GetAllContendores(int idTorneo)
        {
            using (var proxy = new WSContendor.ContendorServiceClient())
            {
                return proxy.ListarContendores(idTorneo) ?? new List<Contendor>();
            }
        }
    }
}