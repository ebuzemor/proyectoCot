using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ComprobanteGenerado
    {
        [JsonProperty("claveComprobante")]
        public string ClaveComprobante { get; set; }
    }
}
