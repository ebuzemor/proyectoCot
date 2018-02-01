using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class InfoCotizaciones
    {
        [JsonProperty("claveComprobanteDeCotizacion")]
        public string ClaveComprobanteDeCotizacion { get; set; }

        [JsonProperty("codigoDeComprobante")]
        public string CodigoDeComprobante { get; set; }

        [JsonProperty("correoElectronico")]
        public string CorreoElectronico { get; set; }

        [JsonProperty("claveEntidadFiscalCliente")]
        public long ClaveEntidadFiscalCliente { get; set; }

        [JsonProperty("claveEntidadFiscalResponsable")]
        public long ClaveEntidadFiscalResponsable { get; set; }

        [JsonProperty("claveEntidadFiscalInmueble")]
        public long ClaveEntidadFiscalInmueble { get; set; }

        [JsonProperty("codigoDeCliente")]
        public string CodigoDeCliente { get; set; }

        [JsonProperty("descuento")]
        public double Descuento { get; set; }

        [JsonProperty("estatus")]
        public string Estatus { get; set; }

        [JsonProperty("claveEstatus")]
        public int ClaveEstatus { get; set; }

        [JsonProperty("fechaEmision")]
        public string FechaEmision { get; set; }

        [JsonProperty("fechaVigencia")]
        public string FechaVigencia { get; set; }

        [JsonProperty("impuesto")]
        public double Impuesto { get; set; }

        [JsonProperty("partidas")]
        public int Partidas { get; set; }

        [JsonProperty("razonSocial")]
        public string RazonSocial { get; set; }

        [JsonProperty("rfc")]
        public string Rfc { get; set; }

        [JsonProperty("subtotal")]
        public double Subtotal { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("observaciones")]
        public string Observaciones { get; set; }
    }
}
