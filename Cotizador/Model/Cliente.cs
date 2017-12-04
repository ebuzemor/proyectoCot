using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ClientesJson
    {
        [JsonProperty("clientes")]
        public List<Cliente> Clientes { get; set; }
    }

    public class Cliente
    {
        [JsonProperty("calle")]
        public string Calle { get; set; }

        [JsonProperty("clasificador")]
        public string Clasificador { get; set; }

        [JsonProperty("claveClasificador")]
        public long ClaveClasificador { get; set; }

        [JsonProperty("claveDireccionFiscal")]
        public long ClaveDireccionFiscal { get; set; }

        [JsonProperty("claveEntidadFiscalCliente")]
        public long ClaveEntidadFiscalCliente { get; set; }

        [JsonProperty("claveEntidadFiscalCobrador")]
        public object ClaveEntidadFiscalCobrador { get; set; }

        [JsonProperty("claveEntidadFiscalEmpresa")]
        public long ClaveEntidadFiscalEmpresa { get; set; }

        [JsonProperty("claveEntidadFiscalVendedor")]
        public long ClaveEntidadFiscalVendedor { get; set; }

        [JsonProperty("claveEstado")]
        public long ClaveEstado { get; set; }

        [JsonProperty("clavePais")]
        public long ClavePais { get; set; }

        [JsonProperty("claveTipoDeCliente")]
        public long ClaveTipoDeCliente { get; set; }

        [JsonProperty("codigoDeCliente")]
        public string CodigoDeCliente { get; set; }

        [JsonProperty("codigoPostal")]
        public string CodigoPostal { get; set; }

        [JsonProperty("colonia")]
        public string Colonia { get; set; }

        [JsonProperty("contacto")]
        public string Contacto { get; set; }

        [JsonProperty("correoElectronico")]
        public string CorreoElectronico { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("especial")]
        public long Especial { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("localidad")]
        public string Localidad { get; set; }

        [JsonProperty("municipio")]
        public string Municipio { get; set; }
        
        [JsonProperty("numeroExterior")]
        public string NumeroExterior { get; set; }

        [JsonProperty("numeroInterior")]
        public string NumeroInterior { get; set; }

        [JsonProperty("numeroTelefono")]
        public string NumeroTelefono { get; set; }

        [JsonProperty("pais")]
        public string Pais { get; set; }

        [JsonProperty("personaFisica")]
        public long PersonaFisica { get; set; }

        [JsonProperty("razonSocial")]
        public string RazonSocial { get; set; }

        [JsonProperty("rfc")]
        public string Rfc { get; set; }

        [JsonProperty("tipoDeCliente")]
        public string TipoDeCliente { get; set; }
    }
}
