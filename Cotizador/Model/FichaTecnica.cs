using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cotizador.Model
{

    public class FichaTecnicaJson
    {
        [JsonProperty("fichaTecnica")]
        public List<FichaTecnica> ListaFichaTecnica { get; set; }
    }

    public class FichaTecnica
    {

        [JsonProperty("claveProducto")]
        public long ClaveProducto { get; set; }

        [JsonProperty("resumen")]
        public string Resumen { get; set; }

        [JsonProperty("imagen")]
        public string Imagen { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("codigoDeProducto")]
        public string CodigoDeProducto { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }
    }
}
