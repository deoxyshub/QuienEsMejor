using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WSSOAP
{
    [ServiceContract]
    public interface IUsuarioService
    {
        [OperationContract]
        int CrearUsuario(Usuario usuario);
        
        [OperationContract]
        Usuario ObtenerUsuarioPorID(int id);

        [OperationContract]
        Usuario ObtenerUsuarioPorCorreo(string correo);

        [OperationContract]
        void ActualizarUsuario(Usuario usuario);

        [OperationContract]
        void EliminarUsuario(int id);

        [OperationContract]
        ICollection<Usuario> ListarUsuarios();
    }
}