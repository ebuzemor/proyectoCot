using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class PermisosJson
    {
        [JsonProperty("permisos")]
        public List<Permisos> Permisos { get; set; }
    }

    public class Permisos
    {
        [JsonProperty("claveSubmodulo")]
        public long ClaveSubmodulo { get; set; }

        [JsonProperty("claveModulo")]
        public long ClaveModulo { get; set; }

        [JsonProperty("claveSeccion")]
        public int ClaveSeccion { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("constante")]
        public string Constante { get; set; }
    }
}
