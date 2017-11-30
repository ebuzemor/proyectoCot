using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class SucursalesJson
    {
        [JsonProperty("sucursales")]
        public List<Sucursal> Sucursales { get; set; }
    }

    public class Sucursal
    {
        [JsonProperty("claveEntidadFiscalEmpresa")]
        public long ClaveEntidadFiscalEmpresa { get; set; }

        [JsonProperty("claveEntidadFiscalInmueble")]
        public long ClaveEntidadFiscalInmueble { get; set; }

        [JsonProperty("claveInmueble")]
        public long ClaveInmueble { get; set; }

        [JsonProperty("codigoDeInmueble")]
        public string CodigoDeInmueble { get; set; }

        [JsonProperty("nombreCorto")]
        public string NombreCorto { get; set; }

        public override string ToString()
        {
            return NombreCorto;
        }
    }
}
