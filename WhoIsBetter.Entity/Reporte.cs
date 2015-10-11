using Newtonsoft.Json;

namespace WhoIsBetter.Entity
{
    public class ReporteTorneo
    {
        [JsonProperty("torneo")]
        public Torneo Torneo { get; set; }

        [JsonProperty("participante")]
        public Participante Participante { get; set; }

        [JsonProperty("contendor1")]
        public Contendor Contendor1 { get; set; }

        [JsonProperty("contendor2")]
        public Contendor Contendor2 { get; set; }

        [JsonProperty("ganador")]
        public Contendor Ganador { get; set; }

        [JsonIgnore()]
        public long PosFixture { get; set; }
    }
}