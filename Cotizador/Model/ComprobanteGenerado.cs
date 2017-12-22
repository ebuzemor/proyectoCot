using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ComprobanteGenerado
    {
        [JsonProperty("folioCodigoComprobante")]
        public string FolioCodigoComprobante { get; set; }
    }
}
