using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;
using WhoIsBetter.DataAccess;

namespace WhoIsBetter.WSSOAP
{
    public class UsuarioService : IUsuarioService
    {
        UsuarioDA usuarioDA = null;

        public UsuarioService()
        {
            usuarioDA = new UsuarioDA();
        }

        public int CrearUsuario(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrEmpty(usuario.Nombre) || 
                string.IsNullOrEmpty(usuario.ApellidoPaterno) || string.IsNullOrEmpty(usuario.ApellidoMaterno))
                throw new FaultException("Información básica del usuario incompleta");

            if (!Validador.EsCorreo(usuario.Correo))
                throw new FaultException("Formato de correo incorrecto");

            if (usuarioDA.ObtenerUsuario(usuario.Correo) != null)
                throw new FaultException("Cuenta correo ya registrado");

            return usuarioDA.CrearUsuario(usuario);
        }

        public Usuario ObtenerUsuarioPorID(int id)
        {
            return usuarioDA.ObtenerUsuario(id);
        }
        
        public Usuario ObtenerUsuarioPorCorreo(string correo)
        {
            return usuarioDA.ObtenerUsuario(correo);
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrEmpty(usuario.Nombre) ||
                string.IsNullOrEmpty(usuario.ApellidoPaterno) || string.IsNullOrEmpty(usuario.ApellidoMaterno))
                throw new FaultException("Información básica del usuario incompleta");
            
            if ((usuario.NumeroCelular ?? 0).ToString().Length != 9)
                throw new FaultException("Número de celular incorrecto");

            if ((usuario.NumeroTelefono ?? 0).ToString().Length != 7)
                throw new FaultException("Número de teléfono incorrecto");
            
            usuarioDA.ActualizarUsuario(usuario);
        }

        public void EliminarUsuario(int id)
        {
            usuarioDA.EliminarUsuario(id);
        }
   
        public ICollection<Usuario> ListarUsuarios()
        {
            return usuarioDA.ListarUsuarios();
        }
    }
}