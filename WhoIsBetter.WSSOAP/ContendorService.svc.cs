using System.Collections.Generic;
using System.ServiceModel;
using WhoIsBetter.Entity;
using WhoIsBetter.DataAccess;

namespace WhoIsBetter.WSSOAP
{
    public class ContendorService : IContendorService
    {
        ContendorDA contendorDA = null;

        public ContendorService()
        {
            contendorDA = new ContendorDA();
        }

        public int CrearContendor(Contendor contendor)
        {
            if (string.IsNullOrEmpty(contendor.Nombre) ||
                string.IsNullOrEmpty(contendor.Texto) ||
                string.IsNullOrEmpty(contendor.RutaImagen))
                throw new FaultException("Información básica del contendor incompleta");

            if (!contendor.RutaImagen.ToLower().EndsWith(".jpg") &&
                !contendor.RutaImagen.ToLower().EndsWith(".gif") &&
                !contendor.RutaImagen.ToLower().EndsWith(".png") &&
                !contendor.RutaImagen.ToLower().EndsWith(".bmp") &&
                !contendor.RutaImagen.ToLower().EndsWith(".jpeg"))
                throw new FaultException("Formato de archivo inválido");


            using (var proxy = new WSTorneo.TorneoServiceClient())
            {
                var contendores = contendorDA.ListarContendores(contendor.IDTorneo);
                var torneo = proxy.ObtenerTorneoPorID(contendor.IDTorneo);

                if (contendores.Count == torneo.NumeroContendores)
                {
                    throw new FaultException("Límite de contendores ha exedido");
                }

                return contendorDA.CrearContendor(contendor);
            }
        }

        public Contendor ObtenerContendor(int id)
        {
            return contendorDA.Obtener(id);
        }

        public void ActualizarContendor(Contendor contendor)
        {
            if (string.IsNullOrEmpty(contendor.Nombre) ||
                string.IsNullOrEmpty(contendor.Texto) ||
                string.IsNullOrEmpty(contendor.RutaImagen))
                throw new FaultException("Información básica del contendor incompleta");

            if (!contendor.RutaImagen.ToLower().EndsWith(".jpg") &&
                !contendor.RutaImagen.ToLower().EndsWith(".gif") &&
                !contendor.RutaImagen.ToLower().EndsWith(".png") &&
                !contendor.RutaImagen.ToLower().EndsWith(".bmp") &&
                !contendor.RutaImagen.ToLower().EndsWith(".jpeg"))
                throw new FaultException("Formato de archivo inválido");

            contendorDA.ActualizarContendor(contendor);
        }

        public void EliminarContendor(int id)
        {
            contendorDA.EliminarContendor(id);
        }
        
        public ICollection<Contendor> ListarContendores(int idTorneo)
        {
            return contendorDA.ListarContendores(idTorneo);
        }
    }
}