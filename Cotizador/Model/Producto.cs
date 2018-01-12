using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class ProductosJson
    {
        [JsonProperty("productos")]
        public List<Producto> Productos { get; set; }
    }

    public class Producto
    {
        [JsonProperty("abreviatura")]
        public string Abreviatura { get; set; }

        [JsonProperty("abreviaturaPeso")]
        public string AbreviaturaPeso { get; set; }

        [JsonProperty("agrupadores")]
        public string Agrupadores { get; set; }

        [JsonProperty("agrupadoresPadre")]
        public string AgrupadoresPadre { get; set; }

        [JsonProperty("cantidadMinimaDeCompra")]
        public double CantidadMinimaDeCompra { get; set; }

        [JsonProperty("cantidadMinimaDeTraslado")]
        public double CantidadMinimaDeTraslado { get; set; }

        [JsonProperty("cantidadMinimaDeVenta")]
        public double CantidadMinimaDeVenta { get; set; }

        [JsonProperty("cantidadPieza")]
        public double CantidadPieza { get; set; }

        [JsonProperty("claveCuentaBancaria")]
        public string ClaveCuentaBancaria { get; set; }

        [JsonProperty("claveEntidadFiscalEmpresa")]
        public long ClaveEntidadFiscalEmpresa { get; set; }

        [JsonProperty("claveEntidadFiscalInmueble")]
        public long ClaveEntidadFiscalInmueble { get; set; }

        [JsonProperty("claveFraccionReenvasable")]
        public double ClaveFraccionReenvasable { get; set; }

        [JsonProperty("claveListaDePrecios")]
        public long ClaveListaDePrecios { get; set; }

        [JsonProperty("claveListaDePreciosCliente")]
        public long ClaveListaDePreciosCliente { get; set; }

        [JsonProperty("claveMoneda")]
        public long ClaveMoneda { get; set; }

        [JsonProperty("claveProducto")]
        public long ClaveProducto { get; set; }

        [JsonProperty("claveProductoPieza")]
        public long ClaveProductoPieza { get; set; }

        [JsonProperty("claveTipoDeProducto")]
        public long ClaveTipoDeProducto { get; set; }

        [JsonProperty("claveTipoDeRedondeo")]
        public long ClaveTipoDeRedondeo { get; set; }

        [JsonProperty("claveUnidadDeMedida")]
        public long ClaveUnidadDeMedida { get; set; }

        [JsonProperty("clavesAgrupadores")]
        public string ClavesAgrupadores { get; set; }

        [JsonProperty("clavesAgrupadoresPadre")]
        public string ClavesAgrupadoresPadre { get; set; }

        [JsonProperty("clavesImpuestos")]
        public string ClavesImpuestos { get; set; }

        [JsonProperty("codigoAnterior")]
        public string CodigoAnterior { get; set; }

        [JsonProperty("codigoDeBarras")]
        public string CodigoDeBarras { get; set; }

        [JsonProperty("codigoDeProveedor")]
        public string CodigoDeProveedor { get; set; }

        [JsonProperty("codigoInterno")]
        public string CodigoInterno { get; set; }

        [JsonProperty("comisionDePagoConTarjeta")]
        public double ComisionDePagoConTarjeta { get; set; }

        [JsonProperty("costobase")]
        public double Costobase { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("descripcionImpuestos")]
        public string DescripcionImpuestos { get; set; }

        [JsonProperty("esFraccionable")]
        public long EsFraccionable { get; set; }

        [JsonProperty("esImportado")]
        public long EsImportado { get; set; }

        [JsonProperty("esParte")]
        public long EsParte { get; set; }

        [JsonProperty("estaSeriado")]
        public long EstaSeriado { get; set; }

        [JsonProperty("existencia")]
        public long Existencia { get; set; }

        [JsonProperty("existenciaPack")]
        public long ExistenciaPack { get; set; }

        [JsonProperty("mensualidad")]
        public string Mensualidad { get; set; }

        [JsonProperty("moneda")]
        public string Moneda { get; set; }

        [JsonProperty("peso")]
        public string Peso { get; set; }

        [JsonProperty("pesoYUnidad")]
        public string PesoYUnidad { get; set; }

        [JsonProperty("precioCom")]
        public double PrecioCom { get; set; }

        [JsonProperty("precioPublico")]
        public double PrecioPublico { get; set; }

        [JsonProperty("precioPublicoCom")]
        public double PrecioPublicoCom { get; set; }

        [JsonProperty("precioUnitario")]
        public double PrecioUnitario { get; set; }

        [JsonProperty("precisionDeRedondeo")]
        public double PrecisionDeRedondeo { get; set; }

        [JsonProperty("requiereDatosDeImportacion")]
        public long RequiereDatosDeImportacion { get; set; }

        [JsonProperty("sumaImpuestos")]
        public long SumaImpuestos { get; set; }

        [JsonProperty("tasa")]
        public double Tasa { get; set; }

        [JsonProperty("tasas")]
        public string Tasas { get; set; }

        [JsonProperty("tipoDeProducto")]
        public string TipoDeProducto { get; set; }

        [JsonProperty("unidadDeMedida")]
        public string UnidadDeMedida { get; set; }

        [JsonProperty("unidadDeMedidaPeso")]
        public string UnidadDeMedidaPeso { get; set; }
    }
}
