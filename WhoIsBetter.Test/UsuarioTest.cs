using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoIsBetter.Entity;

namespace WhoIsBetter.Test
{
    [TestClass]
    public class UsuarioTest
    {
        [TestMethod]
        public void CrearUsuario()
        {
            using (var proxy = new WSUsuario.UsuarioServiceClient())
            {
                var idUsuario = proxy.CrearUsuario(new Usuario {
                    Correo = "jmorales0786@gmail.com",
                    Clave = "123456",
                    Nombre = "Jhonny Gianfranco",
                    ApellidoPaterno = "Morales",
                    ApellidoMaterno = "Olivares",
                    IDRol = Rol.Usuario
                });

                var usuario = proxy.ObtenerUsuarioPorID(idUsuario);

                Assert.AreEqual("jmorales0786@gmail.com", usuario.Correo);
                Assert.AreEqual("Jhonny Gianfranco", usuario.Nombre);
                Assert.AreEqual(Rol.Usuario, usuario.IDRol);
            }
        }

        [TestMethod]
        public void ObtenerUsuario()
        {
            using (var proxy = new WSUsuario.UsuarioServiceClient())
            {
                var usuario = proxy.ObtenerUsuarioPorCorreo("jmorales0786@gmail.com");

                Assert.AreEqual("jmorales0786@gmail.com", usuario.Correo);
                Assert.AreEqual("Jhonny Gianfranco", usuario.Nombre);
                Assert.AreEqual(Rol.Usuario, usuario.IDRol);
            }
        }

        [TestMethod]
        public void ActualizarUsuario()
        {
            using (var proxy = new WSUsuario.UsuarioServiceClient())
            {
                var usuario = proxy.ObtenerUsuarioPorCorreo("jmorales0786@gmail.com");

                usuario.Sexo = true;
                usuario.NumeroCelular = 991690546;
                usuario.NumeroTelefono = 5522606;
                usuario.FechaNacimiento = new DateTime(1986, 07, 23);

                proxy.ActualizarUsuario(usuario);

                usuario = proxy.ObtenerUsuarioPorID(usuario.ID);

                Assert.AreEqual(true, usuario.Sexo);
                Assert.AreEqual(991690546, usuario.NumeroCelular);
                Assert.AreEqual(5522606, usuario.NumeroTelefono);
            }
        }

        [TestMethod]
        public void EliminarUsuario()
        {
            using (var proxy = new WSUsuario.UsuarioServiceClient())
            {
                var usuario = proxy.ObtenerUsuarioPorCorreo("jmorales0786@gmail.com");

                proxy.EliminarUsuario(usuario.ID);

                usuario = proxy.ObtenerUsuarioPorID(usuario.ID);

                Assert.AreEqual(null, usuario);
            }
        }
    }
}
