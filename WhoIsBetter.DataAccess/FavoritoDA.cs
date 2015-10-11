using System.Collections.Generic;
using WhoIsBetter.Entity;

namespace WhoIsBetter.DataAccess
{
    public class FavoritoDA : DataAccessBase
    {
        public FavoritoDA() : base("DB") { }

        public int CrearFavorito(Favorito favorito)
        {
            return this.ExecuteScalar<int>("usp_CrearFavorito",
                new DataParameter("@pIDTorneo", favorito.IDTorneo),
                new DataParameter("@pIDParticipante", favorito.IDParticipante),
                new DataParameter("@pIDContendor1", favorito.IDContendor1),
                new DataParameter("@pIDContendor2", favorito.IDContendor2),
                new DataParameter("@pIDGanador", favorito.IDGanador),
                new DataParameter("@pEtapa", favorito.Etapa)
            );
        }

        public ICollection<Favorito> ListarFavoritos(int idTorneo, int idParticipante)
        {
            return this.ExecuteEntityCollection<Favorito>("usp_ListarFavoritos",
                dr => new Favorito
                {
                    ID = dr.GetValue<int>("ID"),
                    IDTorneo = dr.GetValue<int>("IDTorneo"),
                    IDParticipante = dr.GetValue<int>("IDParticipante"),
                    IDContendor1 = dr.GetValue<int>("IDContendor1"),
                    IDContendor2 = dr.GetValue<int>("IDContendor2"),
                    IDGanador = dr.GetValue<int>("IDGanador"),
                    Etapa = dr.GetValue<short>("Etapa")
                },
                new DataParameter("@pIDTorneo", idTorneo),
                new DataParameter("@pIDParticipante", idParticipante)
            );
        }
    }
}