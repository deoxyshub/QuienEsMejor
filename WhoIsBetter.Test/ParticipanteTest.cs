using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoIsBetter.Entity;

namespace WhoIsBetter.Test
{
    [TestClass]
    public class ParticipanteTest
    {
        [TestMethod]
        public void InscribirParticipante()
        {
            using (var proxyTorneo = new WSTorneo.TorneoServiceClient())
            {
                var torneo = proxyTorneo.ObtenerTorneoPorEnlace("http://localhost/WhoIsBetter/index.html#/Torneos/mejoralumnodelcursodsd");

                using (var proxy = new WSParticipante.ParticipanteServiceClient())
                {
                    var idParticipante = proxy.InscribirParticipante(torneo.ID, new Participante
                    {
                        Correo = "abc@test.com",
                        Nombre = "ABC"
                    });
                    
                    Assert.IsTrue(idParticipante > 0);
                }
            }
        }
    }
}
