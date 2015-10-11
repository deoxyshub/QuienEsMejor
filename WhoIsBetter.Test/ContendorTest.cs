using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoIsBetter.Entity;
using System.Collections.Generic;
using System.Linq;

namespace WhoIsBetter.Test
{
    [TestClass]
    public class ContendorTest
    {
        [TestMethod]
        public void CrearContendores()
        {
            using (var proxyTorneo = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxyTorneo.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                var contendores = new List<Contendor>();

                contendores.Add(new Contendor { Nombre = "ARANGUENA PROAÑO, ROCIO KARIN", Texto = "ROCIO", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\rocio.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "BRAVO ANGULO, FREDDY ALBERTO", Texto = "FREDDY", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\freddy.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "CONCEPCION TIZA, MIGUEL ANGEL", Texto = "MIGUEL", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\miguel.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "ESCOBEDO SAAVEDRA, CHRISTIAN AUGUSTO",Texto =  "CHRISTIAN", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\christian.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "FUENTES VELASQUEZ, ORESTES ISIDRO",Texto =  "ORESTES", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\orestes.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "GUTIERREZ CRUZ, (PATRICIO)JAIME",Texto =  "PATRICIO", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\patricio.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "JIMENEZ TORERO, VICTOR HUGO", Texto = "VICTOR", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\victor.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "LAOS BARRANTES, JORGE AUGUSTO", Texto = "JORGE", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\jorge.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "MARTINEZ IGLESIAS, RONALD CHRISTIAN", Texto = "RONALD", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\ronald.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "MORALES OLIVARES, JHONNY GIANFRANCO", Texto = "GIAN", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\jhonny.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "ORELLANA PEÑAFIEL, MIGUEL ANGEL", Texto = "MIGUELO", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\miguelo.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "SANCHEZ RAFAEL, DAVID GREGORI", Texto = "DAVID", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\david.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "SANDOVAL ALVARADO, ENRIQUE RAPHAEL", Texto = "ENRIQUE",RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\enrique.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "SUMIRI MAMANI, GIANCARLO", Texto = "GIANCARLO", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\giancarlo.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "TERUKINA IJU, PAUL BENJAMIN", Texto = "PAUL", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\paul.png", IDTorneo = torneo.ID });
                contendores.Add(new Contendor { Nombre = "SAIRA ALVAREZ, HECTOR", Texto = "HECTOR", RutaImagen = @"D:\WorkSpace\VS\UPC\DSD\WhoIsBetter\uploads\hector.png", IDTorneo = torneo.ID });

                using (var proxy = new WSContendor.ContendorServiceClient())
                {
                    contendores.ForEach(c =>
                    {
                        var idContendor = proxy.CrearContendor(c);
                        var contendor = proxy.ObtenerContendor(idContendor);

                        Assert.AreEqual(c.Nombre, contendor.Nombre);
                        Assert.AreEqual(c.Texto, contendor.Texto);
                    });
                }
            }
        }

        [TestMethod]
        public void ObtenerContendor()
        {
            using (var proxyTorneo = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxyTorneo.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                using (var proxy = new WSContendor.ContendorServiceClient())
                {
                    var contendor = proxy.ListarContendores(torneo.ID).FirstOrDefault(s=> s.Texto.Equals("GIAN"));

                    Assert.AreEqual("MORALES OLIVARES, JHONNY GIANFRANCO", contendor.Nombre);
                }
            }
        }

        [TestMethod]
        public void ActualizarContendor()
        {
            using (var proxyTorneo = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxyTorneo.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                using (var proxy = new WSContendor.ContendorServiceClient())
                {
                    var contendor = proxy.ListarContendores(torneo.ID).FirstOrDefault(s => s.Texto.Equals("GIAN"));
                    contendor.Texto = "JHONNY";
                    proxy.ActualizarContendor(contendor);

                    contendor = proxy.ObtenerContendor(contendor.ID);

                    Assert.AreEqual("JHONNY", contendor.Texto);
                }
            }
        }

        [TestMethod]
        public void EliminarContendor()
        {
            using (var proxyTorneo = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxyTorneo.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                using (var proxy = new WSContendor.ContendorServiceClient())
                {
                    var contendor = proxy.ListarContendores(torneo.ID).FirstOrDefault(s => s.Texto.Equals("GIAN"));
                    
                    proxy.EliminarContendor(contendor.ID);

                    contendor = proxy.ObtenerContendor(contendor.ID);

                    Assert.AreEqual(null, contendor);
                }
            }
        }
    }
}
