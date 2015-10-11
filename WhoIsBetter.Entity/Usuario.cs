using Newtonsoft.Json;
using System;

namespace WhoIsBetter.Entity
{
    public class Usuario
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("apellidoPaterno")]
        public string ApellidoPaterno { get; set; }

        [JsonProperty("apellidoMaterno")]
        public string ApellidoMaterno { get; set; }

        [JsonProperty("clave")]
        public string Clave { get; set; }

        [JsonProperty("sexo")]
        public bool? Sexo { get; set; }

        [JsonProperty("numeroCelular")]
        public int? NumeroCelular { get; set; }

        [JsonProperty("numeroTelefono")]
        public int? NumeroTelefono { get; set; }

        [JsonProperty("fechaNacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [JsonProperty("idRol")]
        public int IDRol { get; set; }
    }
}