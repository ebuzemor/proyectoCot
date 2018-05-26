using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class UsuariosJson
    {
        [JsonProperty("usuarios")]
        public List<Usuario> ListaUsuarios { get; set; }
    }

    public class Usuario
    {
        [JsonProperty("claveEntidadFiscalEmpleado")]
        public long ClaveEntidadFiscalEmpleado { get; set; }

        [JsonProperty("claveEntidadFiscalEmpresa")]
        public long ClaveEntidadFiscalEmpresa { get; set; }

        [JsonProperty("claveEntidadFiscalInmueble")]
        public long ClaveEntidadFiscalInmueble { get; set; }

        [JsonProperty("empresa")]
        public long Empresa { get; set; }

        [JsonProperty("nombreCorto")]
        public string NombreCorto { get; set; }

        [JsonProperty("nombreUsuario")]
        public string NombreUsuario { get; set; }

        [JsonProperty("razonSocialUsuario")]
        public string RazonSocialUsuario { get; set; }

        [JsonProperty("razonSocialEmpresa")]
        public string RazonSocialEmpresa { get; set; }

        [JsonProperty("sucursal")]
        public long Sucursal { get; set; }
    }
}
