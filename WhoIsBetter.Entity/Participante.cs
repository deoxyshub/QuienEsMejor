using Newtonsoft.Json;

namespace WhoIsBetter.Entity
{
    public class Participante
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }
}