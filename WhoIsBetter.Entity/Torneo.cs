using Newtonsoft.Json;
using System;

namespace WhoIsBetter.Entity
{
    public class Torneo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("cantidadParticipantes")]
        public int NumeroParticipantes { get; set; }

        [JsonProperty("cantidadContendores")]
        public int NumeroContendores { get; set; }

        [JsonProperty("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [JsonProperty("fechaFin")]
        public DateTime FechaFin { get; set; }

        [JsonProperty("enlace")]
        public string Enlace { get; set; }

        [JsonProperty("idEstado")]
        public int IDEstado { get; set; }

        [JsonIgnore()]
        public int NumeroRealParticipantes { get; set; }
    }
}