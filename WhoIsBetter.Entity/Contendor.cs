using Newtonsoft.Json;
using System;
using System.IO;

namespace WhoIsBetter.Entity
{
    public class Contendor
    {
        string _rutaImagen = string.Empty;

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("texto")]
        public string Texto { get; set; }

        [JsonProperty("rutaImagen")]
        public string RutaImagen
        {
            get
            {
                return _rutaImagen;
            }
            set
            {
                _rutaImagen = value;

                this.Imagen = "data:image/"
                        + Path.GetExtension(_rutaImagen).Replace(".", "")
                        + ";base64,"
                        + Convert.ToBase64String(File.ReadAllBytes(_rutaImagen));
            }
        }

        [JsonProperty("imagen")]
        public string Imagen { get; private set; }
        
        [JsonProperty("idTorneo")]
        public int IDTorneo { get; set; }

        [JsonIgnore()]
        public long AgrupadorInicial { get; set; }
    }
}
