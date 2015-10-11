using Newtonsoft.Json;

namespace WhoIsBetter.Entity
{
    public class Favorito
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("idTorneo")]
        public int IDTorneo { get; set; }

        [JsonProperty("idParticipante")]
        public int IDParticipante { get; set; }

        [JsonProperty("idContendor1")]
        public int IDContendor1 { get; set; }

        [JsonProperty("idContendor2")]
        public int IDContendor2 { get; set; }

        [JsonProperty("idGanador")]
        public int IDGanador { get; set; }

        [JsonProperty("etapa")]
        public short Etapa { get; set; }
    }
}