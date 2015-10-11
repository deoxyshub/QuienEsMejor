using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoIsBetter.Entity;
using System.Linq;
using System.Collections.Generic;

namespace WhoIsBetter.Test
{
    [TestClass]
    public class FavoritoTest
    {
        [TestMethod]
        public void CrearFavorito()
        {
            using (var proxyTorneo = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxyTorneo.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                using (var proxyContendor = new WSContendor.ContendorServiceClient())
                {
                    var contendores = proxyContendor.ListarContendores(torneo.ID);

                    var ROCIO = contendores.FirstOrDefault(s => s.Texto.Equals("ROCIO")).ID;
                    var FREDDY = contendores.FirstOrDefault(s => s.Texto.Equals("FREDDY")).ID;
                    var MIGUEL = contendores.FirstOrDefault(s => s.Texto.Equals("MIGUEL")).ID;
                    var CHRISTIAN = contendores.FirstOrDefault(s => s.Texto.Equals("CHRISTIAN")).ID;
                    var ORESTES = contendores.FirstOrDefault(s => s.Texto.Equals("ORESTES")).ID;
                    var PATRICIO = contendores.FirstOrDefault(s => s.Texto.Equals("PATRICIO")).ID;
                    var VICTOR = contendores.FirstOrDefault(s => s.Texto.Equals("VICTOR")).ID;
                    var JORGE = contendores.FirstOrDefault(s => s.Texto.Equals("JORGE")).ID;
                    var RONALD = contendores.FirstOrDefault(s => s.Texto.Equals("RONALD")).ID;
                    var JHONNY = contendores.FirstOrDefault(s => s.Texto.Equals("JHONNY")).ID;
                    var MIGUELO = contendores.FirstOrDefault(s => s.Texto.Equals("MIGUELO")).ID;
                    var DAVID = contendores.FirstOrDefault(s => s.Texto.Equals("DAVID")).ID;
                    var ENRIQUE = contendores.FirstOrDefault(s => s.Texto.Equals("ENRIQUE")).ID;
                    var GIANCARLO = contendores.FirstOrDefault(s => s.Texto.Equals("GIANCARLO")).ID;
                    var PAUL = contendores.FirstOrDefault(s => s.Texto.Equals("PAUL")).ID;
                    var HECTOR = contendores.FirstOrDefault(s => s.Texto.Equals("HECTOR")).ID;

                    using (var proxyParticipante = new WSParticipante.ParticipanteServiceClient())
                    {
                        var idParticipante = proxyParticipante.InscribirParticipante(torneo.ID, new Participante
                        {
                            Correo = "abc@test.com",
                            Nombre = "ABC"
                        });

                        var favoritos = new List<Favorito>();

                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = ROCIO, IDContendor2 = FREDDY, IDGanador = ROCIO, Etapa = 16 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = MIGUEL, IDContendor2 = CHRISTIAN, IDGanador = CHRISTIAN, Etapa = 16 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = ORESTES, IDContendor2 = PATRICIO, IDGanador = ORESTES, Etapa = 16 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = VICTOR, IDContendor2 = JORGE, IDGanador = JORGE, Etapa = 16 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = RONALD, IDContendor2 = JHONNY, IDGanador = JHONNY, Etapa = 16 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = MIGUELO, IDContendor2 = DAVID, IDGanador = DAVID, Etapa = 16 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = ENRIQUE, IDContendor2 = GIANCARLO, IDGanador = ENRIQUE, Etapa = 16 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = PAUL, IDContendor2 = HECTOR, IDGanador = HECTOR, Etapa = 16 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = ROCIO, IDContendor2 = CHRISTIAN, IDGanador = ROCIO, Etapa = 8 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = ORESTES, IDContendor2 = JORGE, IDGanador = JORGE, Etapa = 8 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = JHONNY, IDContendor2 = DAVID, IDGanador = JHONNY, Etapa = 8 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = ENRIQUE, IDContendor2 = HECTOR, IDGanador = HECTOR, Etapa = 8 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = ROCIO, IDContendor2 = JORGE, IDGanador = ROCIO, Etapa = 4 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = JHONNY, IDContendor2 = HECTOR, IDGanador = JHONNY, Etapa = 4 });
                        favoritos.Add(new Favorito { IDTorneo = torneo.ID, IDParticipante = idParticipante, IDContendor1 = ROCIO, IDContendor2 = JHONNY, IDGanador = JHONNY, Etapa = 2 });

                        using (var proxy = new WSFavorito.FavoritoServiceClient())
                        {
                            favoritos.ForEach(f =>
                            {
                                proxy.CrearFavorito(f);
                            });

                            var data = proxy.ListarFavoritos(torneo.ID, idParticipante);

                            Assert.AreEqual(favoritos.Count, data.Count);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void ObtenerFavorito()
        {
            using (var proxyTorneo = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxyTorneo.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                using (var proxyParticipante = new WSParticipante.ParticipanteServiceClient())
                {
                    var idParticipante = proxyParticipante.InscribirParticipante(torneo.ID, new Participante
                    {
                        Correo = "abc@test.com",
                        Nombre = "ABC"
                    });

                    using (var proxy = new WSFavorito.FavoritoServiceClient())
                    {
                        var data = proxy.ListarFavoritos(torneo.ID, idParticipante);

                        Assert.IsTrue(data.Count > 0);
                    }
                }
            }
        }
    }
}
