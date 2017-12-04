using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class CondicionesComerciales
    {
        [JsonProperty("activa")]
        public string Activa { get; set; }

        [JsonProperty("claveCondicionComercialDeCotizacion")]
        public long ClaveCondicionComercialDeCotizacion { get; set; }

        [JsonProperty("claveEntidadFiscalEmpresa")]
        public long ClaveEntidadFiscalEmpresa { get; set; }

        [JsonProperty("condicionComercial")]
        public string CondicionComercial { get; set; }
    }
}
