using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class EstatusCotizacion
    {
        [JsonProperty("claveTipoDeStatusDeComprobante")]
        public long ClaveTipoDeStatusDeComprobante { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("claveTipoDeComprobante")]
        public long ClaveTipoDeComprobante { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
