using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ProductoVendido
    {
        [JsonProperty("claveProducto")]
        public long ClaveProducto { get; set; }

        [JsonProperty("codigoDeProducto")]
        public string CodigoDeProducto { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("precioUnitario")]
        public double PrecioUnitario { get; set; }

        [JsonProperty("cantidad")]
        public double Cantidad { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}