﻿using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class InfoCotizaciones
    {
        [JsonProperty("claveComprobanteDeCotizacion")]
        public string ClaveComprobanteDeCotizacion { get; set; }

        [JsonProperty("claveEntidadFiscalCliente")]
        public long ClaveEntidadFiscalCliente { get; set; }

        [JsonProperty("codigoDeCliente")]
        public string CodigoDeCliente { get; set; }

        [JsonProperty("descuento")]
        public double Descuento { get; set; }

        [JsonProperty("estatus")]
        public string Estatus { get; set; }

        [JsonProperty("fechaEmision")]
        public string FechaEmision { get; set; }

        [JsonProperty("impuesto")]
        public double Impuesto { get; set; }

        [JsonProperty("partidas")]
        public int Partidas { get; set; }

        [JsonProperty("razonSocial")]
        public string RazonSocial { get; set; }

        [JsonProperty("rfc")]
        public string Rfc { get; set; }

        [JsonProperty("subtotal")]
        public double Subtotal { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}