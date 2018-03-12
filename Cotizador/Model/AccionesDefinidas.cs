using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class AccionesDefinidasJson
    {
        [JsonProperty("acciones")]
        public List<AccionesDefinidas> AccionesDefinidas { get; set; }
    }

    public class AccionesDefinidas
    {
        [JsonProperty("claveSeccion")]
        public long ClaveSeccion { get; set; }

        [JsonProperty("claveSubmodulo")]
        public long ClaveSubmodulo { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("constante")]
        public string Constante { get; set; }

        public bool Activo { get; set; }
    }
}
