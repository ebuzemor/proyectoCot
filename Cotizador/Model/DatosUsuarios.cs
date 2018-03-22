using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class DatosUsuariosJson
    {
        [JsonProperty("datosUsuarios")]
        public List<DatosUsuarios> ListaDatosUsuarios { get; set; }
    }

    public class DatosUsuarios
    {
        [JsonProperty("claveEntidadFiscalUsuario")]
        public long ClaveEntidadFiscalUsuario { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("razonSocial")]
        public string RazonSocial { get; set; }
    }
}
