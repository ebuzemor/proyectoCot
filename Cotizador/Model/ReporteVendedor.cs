using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ReporteVendedor
    {
        [JsonProperty("claveEntidadFiscalUsuario")]
        public long ClaveEntidadFiscalUsuario { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("nombreVendedor")]
        public string NombreVendedor { get; set; }

        [JsonProperty("sucursal")]
        public long Sucursal { get; set; }

        [JsonProperty("num_cotizaciones")]
        public int NumCotizaciones { get; set; }

        [JsonProperty("borrador")]
        public int Borrador { get; set; }

        [JsonProperty("pendientes")]
        public int Pendientes { get; set; }

        [JsonProperty("autorizadas")]
        public int Autorizadas { get; set; }

        [JsonProperty("canceladas")]
        public int Canceladas { get; set; }

        [JsonProperty("facturadas")]
        public int Facturadas { get; set; }

        [JsonProperty("total_ctzs_cte")]
        public double TotalCtzsCte { get; set; }

        [JsonProperty("total_ctzs")]
        public double TotalCtzs { get; set; }

        [JsonProperty("max_ctz_cte")]
        public double MaxCtzCte { get; set; }

        [JsonProperty("total_desctos")]
        public double TotalDesctos { get; set; }

        [JsonProperty("max_descto_cte")]
        public double MaxDesctoCte { get; set; }

        [JsonProperty("total_ctzs_fact")]
        public double TotalCtzsFact { get; set; }

        [JsonProperty("max_ctz_cte_fact")]
        public double MaxCtzCteFact { get; set; }

        [JsonProperty("total_ctzs_descto_fact")]
        public double TotalCtzsDesctoFact { get; set; }

        [JsonProperty("max_descto_cte_fact")]
        public double MaxDesctoCteFact { get; set; }
    }
}