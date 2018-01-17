using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ExistenciasJson
    {
        [JsonProperty("existencias")]
        public List<Existencias> Existencias { get; set; }
    }

    public class Existencias
    {
        [JsonProperty("claveProducto")]
        public long ClaveProducto { get; set; }

        [JsonProperty("cantidad")]
        public double Cantidad { get; set; }

        [JsonProperty("sucursal")]
        public string Sucursal { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }
}
