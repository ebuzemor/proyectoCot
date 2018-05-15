using Newtonsoft.Json;
using System;

namespace Cotizador.Model
{
    public class InfoFacturas
    {
        [JsonProperty("claveComprobante")]
        public string ClaveComprobante { get; set; }

        [JsonProperty("codigoCotizacion")]
        public string CodigoCotizacion { get; set; }

        [JsonProperty("claveComprobanteFiscal")]
        public string ClaveComprobanteFiscal { get; set; }

        [JsonProperty("codigoFactura")]
        public string CodigoFactura { get; set; }

        [JsonProperty("fechaEmision")]
        public DateTime FechaEmision { get; set; }

        [JsonProperty("fechaFactura")]
        public DateTime FechaFactura { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("folio")]
        public string Folio { get; set; }

        [JsonProperty("razonSocial")]
        public string RazonSocial { get; set; }
    }
}
