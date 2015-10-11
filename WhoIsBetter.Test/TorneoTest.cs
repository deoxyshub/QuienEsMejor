using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoIsBetter.Entity;

namespace WhoIsBetter.Test
{
    [TestClass]
    public class TorneoTest
    {
        [TestMethod]
        public void CrearTorneo()
        {
            using (var proxyUsuario = new WSUsuario.UsuarioServiceClient())
            {
                var usuario = proxyUsuario.ObtenerUsuarioPorCorreo("jmorales0786@gmail.com");

                using (var proxy = new WSTorneo.TorneoServiceClient())
                {
                    var idTorneo = proxy.CrearTorneo(usuario.ID, new Torneo
                    {
                        Nombre = "Mejor alumno del curso DSD",
                        Enlace = "http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd",
                        IDEstado = 1,
                        FechaInicio = DateTime.Today,
                        FechaFin = DateTime.Today.AddDays(2),
                        NumeroContendores = 16,
                        NumeroParticipantes = 3
                    });

                    var torneo = proxy.ObtenerTorneoPorID(idTorneo);

                    Assert.AreEqual("Mejor alumno del curso DSD", torneo.Nombre);
                    Assert.AreEqual(16, torneo.NumeroContendores);
                    Assert.AreEqual(3, torneo.NumeroParticipantes);
                }
            }
        }

        [TestMethod]
        public void ObtenerTorneo()
        {
            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxy.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                torneo = proxy.ObtenerTorneoPorID(torneo.ID);

                Assert.AreEqual("Mejor alumno del curso DSD", torneo.Nombre);
                Assert.AreEqual(16, torneo.NumeroContendores);
                Assert.AreEqual(3, torneo.NumeroParticipantes);
            }
        }

        [TestMethod]
        public void ActualizarTorneo()
        {
            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxy.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");
                torneo.NumeroParticipantes = 5;

                proxy.ActualizarTorneo(torneo);

                torneo = proxy.ObtenerTorneoPorID(torneo.ID);

                Assert.AreEqual(5, torneo.NumeroParticipantes);
            }
        }

        [TestMethod]
        public void EliminarTorneo()
        {
            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxy.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                proxy.EliminarTorneo(torneo.ID);

                torneo = proxy.ObtenerTorneoPorID(torneo.ID);

                Assert.AreEqual(null, torneo);
            }
        }
    }
}
