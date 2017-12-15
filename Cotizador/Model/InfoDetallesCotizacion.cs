using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class InfoDetallesCotizacion
    {
        [JsonProperty("claveDetalleDeComprobante")]
        public string ClaveDetalleDeComprobante { get; set; }

        [JsonProperty("codigoInterno")]
        public string CodigoInterno { get; set; }

        [JsonProperty("claveProducto")]
        public long ClaveProducto { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("esImportado")]
        public long EsImportado { get; set; }

        [JsonProperty("sumaImpuestos")]
        public long SumaImpuestos { get; set; }

        [JsonProperty("tasas")]
        public string Tasas { get; set; }

        [JsonProperty("clavesImpuestos")]
        public string ClavesImpuestos { get; set; }

        [JsonProperty("precioUnitario")]
        public double PrecioUnitario { get; set; }

        [JsonProperty("cantidad")]
        public double Cantidad { get; set; }

        [JsonProperty("importeDescuento")]
        public double ImporteDescuento { get; set; }

        [JsonProperty("importe")]
        public double Importe { get; set; }

        [JsonProperty("impuestos")]
        public double Impuestos { get; set; }

        [JsonProperty("subtotal")]
        public double Subtotal { get; set; }
    }
}
