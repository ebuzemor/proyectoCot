using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class Cotizacion
    {
        [JsonProperty("Empresa")]
        public long Empresa { get; set; }

        [JsonProperty("Equipo")]
        public string Equipo { get; set; }

        [JsonProperty("Usuario")]
        public string Usuario { get; set; }

        [JsonProperty("ClaveInmueble")]
        public long ClaveInmueble { get; set; }

        [JsonProperty("ClaveEntidadFiscalInmueble")]
        public long ClaveEntidadFiscalInmueble { get; set; }

        [JsonProperty("ClaveTipoDeComprobante")]
        public long ClaveTipoDeComprobante { get; set; }

        [JsonProperty("FechaEmision")]
        public string FechaEmision { get; set; }

        [JsonProperty("Partidas")]
        public long Partidas { get; set; }

        [JsonProperty("ClaveMoneda")]
        public long ClaveMoneda { get; set; }

        [JsonProperty("ClaveTipoEstatusRecepcion")]
        public long ClaveTipoEstatusRecepcion { get; set; }

        [JsonProperty("ClaveEntidadFiscalResponsable")]
        public long ClaveEntidadFiscalResponsable { get; set; }

        [JsonProperty("ListaComprobantesImpuestos")]
        public string ListaComprobantesImpuestos { get; set; }

        [JsonProperty("ClaveEntidadFiscalCliente")]
        public long ClaveEntidadFiscalCliente { get; set; }

        [JsonProperty("ClaveListaDePrecios")]
        public long ClaveListaDePrecios { get; set; }

        [JsonProperty("FechaVigencia")]
        public string FechaVigencia { get; set; }

        [JsonProperty("SubTotal")]
        public double SubTotal { get; set; }

        [JsonProperty("Descuento")]
        public double Descuento { get; set; }

        [JsonProperty("Impuestos")]
        public double Impuestos { get; set; }

        [JsonProperty("Total")]
        public double Total { get; set; }

        [JsonProperty("Observaciones")]
        public string Observaciones { get; set; }

        [JsonProperty("DetallesDeComprobante")]
        public string DetallesDeComprobante { get; set; }
    }

    public class ComprobantesImpuestos
    {
        [JsonProperty("ClaveImpuesto")]
        public long ClaveImpuesto { get; set; }

        [JsonProperty("Importe")]
        public double Importe { get; set; }
    }

    public class DetalleComprobantes
    {
        [JsonProperty("NumeroPartidas")]
        public long NumeroPartidas { get; set; }

        [JsonProperty("ClaveProducto")]
        public long ClaveProducto { get; set; }

        [JsonProperty("Cantidad")]
        public double Cantidad { get; set; }

        [JsonProperty("ClaveUnidadDeMedida")]
        public long ClaveUnidadDeMedida { get; set; }

        [JsonProperty("PrecioUnitario")]
        public double PrecioUnitario { get; set; }

        [JsonProperty("Importe")]
        public double Importe { get; set; }

        [JsonProperty("ImporteDescuento")]
        public double ImporteDescuento { get; set; }

        [JsonProperty("Impuestos")]
        public string Impuestos { get; set; }
    }
}
