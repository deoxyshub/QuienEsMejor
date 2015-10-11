using System;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace WhoIsBetter.WSREST.Modules
{
    public class BasicAuthHttpModule : IHttpModule
    {
        private const string Realm = "WhoIsBetter";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }
        
        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool AuthenticateUser(string credentials)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            credentials = encoding.GetString(Convert.FromBase64String(credentials));

            var credentialsArray = credentials.Split(':');
            var username = credentialsArray[0];
            var password = credentialsArray[1];

            try
            {   
                using (var proxy = new WSUsuario.UsuarioServiceClient())
                {
                    var usuario = proxy.ObtenerUsuarioPorCorreo(username);
                    if (!(usuario != null && usuario.Clave == password))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpException(401, "Unauthorized", ex);
            }

            var identity = new GenericIdentity(username);
            SetPrincipal(new GenericPrincipal(identity, null));

            return true;
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            var request = application.Context.Request;
            var response = application.Context.Response;
            var authHeader = request.Headers["Authorization"];

            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    //AuthenticateUser(authHeaderVal.Parameter);
                    if (!AuthenticateUser(authHeaderVal.Parameter))
                    {
                        response.StatusCode = 401;
                        application.CompleteRequest();
                    }
                }
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        public void Dispose()
        {
        }
    }
}