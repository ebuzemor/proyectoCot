using Newtonsoft.Json;

namespace Cotizador.Model
{
    public class Cotizacion
    {
        private long _empresa;
        private string _equipo;
        private string _usuario;
        private long _claveInmueble;
        private long _claveEntidadFiscalInmueble;
        private long _claveTipoDeComprobante;
        private string _fechaEmision;
        private long _partidas;
        private long _claveMoneda;
        private long _claveTipoEstatusRecepcion;
        private long _claveEntidadFiscalResponsable;
        private string _claveComprobante;
        private string _codigoDeComprobante;
        private string _listaComprobantesImpuestos;
        private long _claveEntidadFiscalCliente;
        private long _claveListaDePrecios;
        private string _fechaVigencia;
        private double _subTotal;
        private double _descuento;
        private double _impuestos;
        private double _total;
        private string _observaciones;
        private string _detallesDeComprobante;

        [JsonProperty("Empresa")]
        public long Empresa { get => _empresa; set => _empresa = value; }

        [JsonProperty("Equipo")]
        public string Equipo { get => _equipo; set => _equipo = value; }

        [JsonProperty("Usuario")]
        public string Usuario { get => _usuario; set => _usuario = value; }

        [JsonProperty("ClaveInmueble")]
        public long ClaveInmueble { get => _claveInmueble; set => _claveInmueble = value; }

        [JsonProperty("ClaveEntidadFiscalInmueble")]
        public long ClaveEntidadFiscalInmueble { get => _claveEntidadFiscalInmueble; set => _claveEntidadFiscalInmueble = value; }

        [JsonProperty("ClaveTipoDeComprobante")]
        public long ClaveTipoDeComprobante { get => _claveTipoDeComprobante; set => _claveTipoDeComprobante = value; }

        [JsonProperty("FechaEmision")]
        public string FechaEmision { get => _fechaEmision; set => _fechaEmision = value; }

        [JsonProperty("Partidas")]
        public long Partidas { get => _partidas; set => _partidas = value; }

        [JsonProperty("ClaveMoneda")]
        public long ClaveMoneda { get => _claveMoneda; set => _claveMoneda = value; }

        [JsonProperty("ClaveTipoEstatusRecepcion")]
        public long ClaveTipoEstatusRecepcion { get => _claveTipoEstatusRecepcion; set => _claveTipoEstatusRecepcion = value; }

        [JsonProperty("ClaveEntidadFiscalResponsable")]
        public long ClaveEntidadFiscalResponsable { get => _claveEntidadFiscalResponsable; set => _claveEntidadFiscalResponsable = value; }

        [JsonProperty("ClaveComprobante")]
        public string ClaveComprobante { get => _claveComprobante; set => _claveComprobante = value; }

        [JsonProperty("CodigoDeComprobante")]
        public string CodigoDeComprobante { get => _codigoDeComprobante; set => _codigoDeComprobante = value; }

        [JsonProperty("ListaComprobantesImpuestos")]
        public string ListaComprobantesImpuestos { get => _listaComprobantesImpuestos; set => _listaComprobantesImpuestos = value; }

        [JsonProperty("ClaveEntidadFiscalCliente")]
        public long ClaveEntidadFiscalCliente { get => _claveEntidadFiscalCliente; set => _claveEntidadFiscalCliente = value; }

        [JsonProperty("ClaveListaDePrecios")]
        public long ClaveListaDePrecios { get => _claveListaDePrecios; set => _claveListaDePrecios = value; }

        [JsonProperty("FechaVigencia")]
        public string FechaVigencia { get => _fechaVigencia; set => _fechaVigencia = value; }

        [JsonProperty("SubTotal")]
        public double SubTotal { get => _subTotal; set => _subTotal = value; }

        [JsonProperty("Descuento")]
        public double Descuento { get => _descuento; set => _descuento = value; }

        [JsonProperty("Impuestos")]
        public double Impuestos { get => _impuestos; set => _impuestos = value; }

        [JsonProperty("Total")]
        public double Total { get => _total; set => _total = value; }

        [JsonProperty("Observaciones")]
        public string Observaciones { get => _observaciones; set => _observaciones = value; }

        [JsonProperty("DetallesDeComprobante")]
        public string DetallesDeComprobante { get => _detallesDeComprobante; set => _detallesDeComprobante = value; }
    }

    public class ComprobantesImpuestos
    {
        private long _claveImpuesto;
        private double _importe;

        [JsonProperty("ClaveImpuesto")]
        public long ClaveImpuesto { get => _claveImpuesto; set => _claveImpuesto = value; }

        [JsonProperty("Importe")]
        public double Importe { get => _importe; set => _importe = value; }
    }

    public class DetalleComprobantes
    {
        private string _claveDetalleDeComprobante;
        private long _numeroPartidas;
        private long _claveProducto;
        private double _cantidad;
        private long _claveUnidadDeMedida;
        private double _precioUnitario;
        private double _importe;
        private double _importeDescuento;
        private string _impuestos;
        private int _estatus;
        private int _diasDeEntrega;

        [JsonProperty("claveDetalleDeComprobante")]
        public string ClaveDetalleDeComprobante { get => _claveDetalleDeComprobante; set => _claveDetalleDeComprobante = value; }

        [JsonProperty("NumeroPartidas")]
        public long NumeroPartidas { get => _numeroPartidas; set => _numeroPartidas = value; }

        [JsonProperty("ClaveProducto")]
        public long ClaveProducto { get => _claveProducto; set => _claveProducto = value; }

        [JsonProperty("Cantidad")]
        public double Cantidad { get => _cantidad; set => _cantidad = value; }

        [JsonProperty("ClaveUnidadDeMedida")]
        public long ClaveUnidadDeMedida { get => _claveUnidadDeMedida; set => _claveUnidadDeMedida = value; }

        [JsonProperty("PrecioUnitario")]
        public double PrecioUnitario { get => _precioUnitario; set => _precioUnitario = value; }

        [JsonProperty("Importe")]
        public double Importe { get => _importe; set => _importe = value; }

        [JsonProperty("ImporteDescuento")]
        public double ImporteDescuento { get => _importeDescuento; set => _importeDescuento = value; }

        [JsonProperty("Impuestos")]
        public string Impuestos { get => _impuestos; set => _impuestos = value; }

        [JsonProperty("Estatus")]
        public int Estatus { get => _estatus; set => _estatus = value; }

        [JsonProperty("DiasDeEntrega")]
        public int DiasDeEntrega { get => _diasDeEntrega; set => _diasDeEntrega = value; }
    }
}
