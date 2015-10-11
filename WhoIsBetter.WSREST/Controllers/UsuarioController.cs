using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSREST.Controllers
{
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        [Route("")]
        public IHttpActionResult PostUsuario(Usuario usuario)
        {
            try
            {
                using (var proxy = new WSUsuario.UsuarioServiceClient())
                {
                    usuario.IDRol = Rol.Usuario;
                    proxy.CrearUsuario(usuario);

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

        [Authorize]
        [Route("{id:int}")]
        public Usuario GetUsuario(int id)
        {
            using (var proxy = new WSUsuario.UsuarioServiceClient())
            {
                return proxy.ObtenerUsuarioPorID(id);
            }
        }

        [Authorize]
        [Route("{id:int}")]
        public IHttpActionResult PutUsuario(int id, Usuario usuario)
        {
            try
            {
                using (var proxy = new WSUsuario.UsuarioServiceClient())
                {
                    usuario.ID = id;
                    usuario.IDRol = Rol.Usuario;

                    proxy.ActualizarUsuario(usuario);

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

        [Authorize]
        [Route("{id:int}")]
        public void DeleteUsuario(int id)
        {
            using (var proxy = new WSUsuario.UsuarioServiceClient())
            {
                proxy.EliminarUsuario(id);
            }
        }

        [Authorize]
        [Route("")]
        public ICollection<Usuario> GetUsuarios()
        {
            using (var proxy = new WSUsuario.UsuarioServiceClient())
            {
                return proxy.ListarUsuarios() ?? new List<Usuario>();
            }
        }
    }

    [RoutePrefix("api")]
    public class AutenticacionController : ApiController
    {
        [Route("autenticar")]
        public IHttpActionResult Autenticar(Usuario account)
        {
            using (var proxy = new WSUsuario.UsuarioServiceClient())
            {
                var usuario = proxy.ObtenerUsuarioPorCorreo(account.Correo);
                if (!(usuario != null && usuario.Clave == account.Clave))
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Usuario o clave inválido"
                    });
                }

                return Ok(new
                {
                    success = true,
                    idUsuario = usuario.ID,
                    token = new
                    {
                        key = ".HTMLAUTH:" + usuario.ID.ToString(),
                        value = Encode(account.Correo + ':' + account.Clave)
                    }
                });
            }
        }

        private string Encode(string str)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var bytes = encoding.GetBytes(str);
            var base64 = Convert.ToBase64String(bytes);

            return base64;
        }
    }
}