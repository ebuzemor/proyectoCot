using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class DatosClientesCtz
    {
        [JsonProperty("claveEntidadFiscalUsuario")]
        public long ClaveEntidadFiscalUsuario { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("nombreVendedor")]
        public string NombreVendedor { get; set; }

        [JsonProperty("claveEntidadFiscalCliente")]
        public long ClaveEntidadFiscalCliente { get; set; }

        [JsonProperty("nombreCliente")]
        public string NombreCliente { get; set; }

        [JsonProperty("claveEntidadFiscalInmueble")]
        public string ClaveEntidadFiscalInmueble { get; set; }

        [JsonProperty("claveInmueble")]
        public long ClaveInmueble { get; set; }

        [JsonProperty("claveTipoDeStatusDeComprobante")]
        public long ClaveTipoDeStatusDeComprobante { get; set; }

        [JsonProperty("numcotizaciones")]
        public long NumCotizaciones { get; set; }

        [JsonProperty("montocotizaciones")]
        public string MontoCotizaciones { get; set; }

        [JsonProperty("montodescuentos")]
        public string MontoDescuentos { get; set; }
    }
}