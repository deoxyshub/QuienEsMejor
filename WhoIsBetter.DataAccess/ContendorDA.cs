using System.Collections.Generic;
using WhoIsBetter.Entity;

namespace WhoIsBetter.DataAccess
{
    public class ContendorDA: DataAccessBase
    {
        public ContendorDA() : base("DB") { }
 
        public int CrearContendor(Contendor contendor)
        {
            return this.ExecuteScalar<int>("usp_CrearContendor",
                new DataParameter("@pNombre", contendor.Nombre),
                new DataParameter("@pTexto", contendor.Texto),
                new DataParameter("@pRutaImagen", contendor.RutaImagen),
                new DataParameter("@pIDTorneo", contendor.IDTorneo)
            );
        }

        public Contendor Obtener(int id)
        {
            return this.ExecuteEntity<Contendor>("usp_ObtenerContendor",
                dr => new Contendor
                {
                    ID = dr.GetValue<int>("ID"),
                    Nombre = dr.GetString("Nombre"),
                    Texto = dr.GetString("Texto"),
                    RutaImagen = dr.GetString("RutaImagen"),
                    IDTorneo = dr.GetValue<int>("IDTorneo")
                },
                new DataParameter("@pID", id)
            );
        }

        public void ActualizarContendor(Contendor contendor)
        {
            this.ExecuteNonQuery("usp_ActualizarContendor",
                new DataParameter("@pID", contendor.ID),
                new DataParameter("@pNombre", contendor.Nombre),
                new DataParameter("@pTexto", contendor.Texto),
                new DataParameter("@pRutaImagen", contendor.RutaImagen),
                new DataParameter("@pIDTorneo", contendor.IDTorneo)
            );
        }

        public void EliminarContendor(int id)
        {
            this.ExecuteNonQuery("usp_EliminarContendor",
                new DataParameter("@pID", id)
            );
        }

        public ICollection<Contendor> ListarContendores(int idTorneo)
        {
            return this.ExecuteEntityCollection<Contendor>("usp_ListarContendores",
                dr => new Contendor
                {
                    IDTorneo = dr.GetValue<int>("IDTorneo"),
                    ID = dr.GetValue<int>("ID"),
                    Nombre = dr.GetString("Nombre"),
                    Texto = dr.GetString("Texto"),
                    RutaImagen = dr.GetString("RutaImagen")
                },
                new DataParameter("@pIDTorneo", idTorneo)
            );
        }
    }
}