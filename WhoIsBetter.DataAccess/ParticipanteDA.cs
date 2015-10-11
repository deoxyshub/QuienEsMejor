using WhoIsBetter.Entity;

namespace WhoIsBetter.DataAccess
{
    public class ParticipanteDA : DataAccessBase
    {
        public ParticipanteDA() : base("DB") { }

        public int InscribirParticipante(int idTorneo, Participante participante)
        {
            return this.ExecuteScalar<int>("usp_InscribirParticipante",
                new DataParameter("@pIDTorneo", idTorneo),
                new DataParameter("@pNombre", participante.Nombre),
                new DataParameter("@pCorreo", participante.Correo)
            );
        }

        public Participante ObtenerParticipante(int idTorneo, string correo)
        {
            return this.ExecuteEntity<Participante>("usp_ObtenerParticipante",
                dr => new Participante
                {
                    ID = dr.GetValue<int>("ID"),
                    Nombre = dr.GetString("Nombre"),
                    Correo = dr.GetString("Correo")
                },
                new DataParameter("@pIDTorneo", idTorneo),
                new DataParameter("@pCorreo", correo)
            );
        }
    }
}