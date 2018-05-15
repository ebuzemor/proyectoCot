using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

namespace Cotizador.ViewModel
{
    public class HistorialClienteViewModel : Notificador
    {
        #region Commands
        public RelayCommand ResetFormularioCommand { get; set; }
        public RelayCommand BuscarClienteCommand { get; set; }
        public RelayCommand ElegirPeriodoCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        public RelayCommand VerCtzGeneradaCommand { get; set; }
        public RelayCommand VerCtzFacturadaCommand { get; set; }
        #endregion

        #region Variables
        private bool _verMensaje;
        private double _cantidadTotal;
        private double _importeTotal;
        private double _descuentoTotal;
        private double _sumaTotal;
        private string _cteRazonSocial;
        private string _datosCliente;
        private string _localhost;
        private string _txtMensaje;
        private string _txtPeriodo;
        private ApiKey _appKey;
        private Cliente _infoCliente;
        private DateTime _fechaInicio;
        private DateTime _fechaFinal;
        private Usuario _usuario;
        private ObservableCollection<InfoFacturas> _listaCtzFacturadas;
        private ObservableCollection<InfoCotizaciones> _listaCtzUltimas;
        private ObservableCollection<ProductoVendido> _listaProdCotizados;
        private ObservableCollection<ProductoVendido> _listaProdVendidos;
        private ObservableCollection<ProductoSeleccionado> _listaProductos;
        private ObservableCollection<AccionesDefinidas> _listaAcciones;

        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public double CantidadTotal { get => _cantidadTotal; set { _cantidadTotal = value; OnPropertyChanged(); } }
        public double ImporteTotal { get => _importeTotal; set { _importeTotal = value; OnPropertyChanged(); } }
        public double DescuentoTotal { get => _descuentoTotal; set { _descuentoTotal = value; OnPropertyChanged(); } }
        public double SumaTotal { get => _sumaTotal; set { _sumaTotal = value; OnPropertyChanged(); } }
        public string CteRazonSocial { get => _cteRazonSocial; set { _cteRazonSocial = value; OnPropertyChanged(); } }
        public string DatosCliente { get => _datosCliente; set { _datosCliente = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public string TxtPeriodo { get => _txtPeriodo; set { _txtPeriodo = value; OnPropertyChanged(); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Cliente InfoCliente { get => _infoCliente; set { _infoCliente = value; OnPropertyChanged(); } }
        public DateTime FechaInicio { get => _fechaInicio; set { _fechaInicio = value; OnPropertyChanged(); } }
        public DateTime FechaFinal { get => _fechaFinal; set { _fechaFinal = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public ObservableCollection<InfoFacturas> ListaCtzFacturadas { get => _listaCtzFacturadas; set { _listaCtzFacturadas = value; OnPropertyChanged(); } }
        public ObservableCollection<InfoCotizaciones> ListaCtzUltimas { get => _listaCtzUltimas; set { _listaCtzUltimas = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductoVendido> ListaProdCotizados { get => _listaProdCotizados; set { _listaProdCotizados = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductoVendido> ListaProdVendidos { get => _listaProdVendidos; set { _listaProdVendidos = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductoSeleccionado> ListaProductos { get => _listaProductos; set { _listaProductos = value; OnPropertyChanged(); } }
        public ObservableCollection<AccionesDefinidas> ListaAcciones { get => _listaAcciones; set { _listaAcciones = value; OnPropertyChanged(); } }        
        #endregion

        #region Constructor
        public HistorialClienteViewModel()
        {
            ResetFormularioCommand = new RelayCommand(ResetFormulario);
            BuscarClienteCommand = new RelayCommand(BuscarCliente);
            ElegirPeriodoCommand = new RelayCommand(ElegirPeriodo);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            VerCtzGeneradaCommand = new RelayCommand(VerCtzGenerada);
            VerCtzFacturadaCommand = new RelayCommand(VerCtzFacturada);
            DateTime hoy = DateTime.Now;
            FechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            FechaFinal = FechaInicio.AddMonths(1).AddDays(-1);
            TxtPeriodo = "Período: " + FechaInicio.ToString("dd-MM-yyyy") + " al " + FechaFinal.ToString("dd-MM-yyyy");
        }
        #endregion

        #region Métodos
        private void ResetFormulario(object parameter)
        {
            InfoCliente = null;
            CteRazonSocial = string.Empty;
            DatosCliente = string.Empty;
            if (ListaCtzFacturadas != null) { ListaCtzFacturadas.Clear(); }
            if (ListaCtzUltimas != null) { ListaCtzUltimas.Clear(); }
            if (ListaProdCotizados != null) { ListaProdCotizados.Clear(); }
            if (ListaProdVendidos != null) { ListaProdVendidos.Clear(); }
            DateTime hoy = DateTime.Now;
            FechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            FechaFinal = FechaInicio.AddMonths(1).AddDays(-1);
            TxtPeriodo = "Período: " + FechaInicio.ToString("dd-MM-yyyy") + " al " + FechaFinal.ToString("dd-MM-yyyy");
        }

        private async void BuscarCliente(object parameter)
        {
            if (FechaInicio < FechaFinal)
            {
                var vmBuscarCliente = new BuscarClientesViewModel
                {
                    AppKey = AppKey,
                    Usuario = Usuario,
                    Localhost = Localhost
                };
                var vwBuscarCliente = new BuscarClientesView
                {
                    DataContext = vmBuscarCliente
                };
                var buscarCte = await DialogHost.Show(vwBuscarCliente, "HistorialCliente");
                if (buscarCte.Equals("NvoCliente") == true)
                {
                    if (vmBuscarCliente.NvoCliente != null)
                    {
                        InfoCliente = vmBuscarCliente.NvoCliente;
                        CteRazonSocial = InfoCliente.RazonSocial + " | RFC:" + InfoCliente.Rfc + " | Codigo:" + InfoCliente.CodigoDeCliente;
                        DatosCliente = "Contacto(s):" + InfoCliente.Contacto + " | Teléfono(s): " + InfoCliente.NumeroTelefono + " | Direccion: " + InfoCliente.Direccion;
                        ObtenerHistorial();
                    }
                }
            }
            else
            {
                TxtMensaje = "La fecha de inicio del período debe ser menor o igual a la fecha final, por favor corrija.";
                VerMensaje = true;
            }
        }

        private async void ElegirPeriodo(object parameter)
        {
            var vmElegir = new ElegirPeriodoViewModel
            {
                FechaInicial = FechaInicio,
                FechaFinal = FechaFinal
            };
            var vwElegir = new ElegirPeriodoView
            {
                DataContext = vmElegir
            };
            var msUltima = await DialogHost.Show(vwElegir, "HistorialCliente");
            if (msUltima.Equals("OK") == true)
            {
                FechaInicio = vmElegir.FechaInicial;
                FechaFinal = vmElegir.FechaFinal;
                if (FechaInicio < FechaFinal)
                {
                    TxtPeriodo = "Período: " + FechaInicio.ToString("dd-MM-yyyy") + " al " + FechaFinal.ToString("dd-MM-yyyy");
                    if (InfoCliente != null)
                    {
                        ObtenerHistorial();
                    }
                }
                else
                {
                    TxtMensaje = "La fecha de inicio del período debe ser menor o igual a la fecha final, por favor corrija.";
                    VerMensaje = true;
                }
            }
        }

        private void ObtenerHistorial()
        {
            var rest = new RestClient(Localhost);
            var req = new RestRequest("obtenerCtzGeneradas/" + Usuario.ClaveEntidadFiscalInmueble + "/" + FechaInicio.ToString("yyyy-MM-dd") + "/"
                                      + FechaFinal.ToString("yyyy-MM-dd") + "/" + InfoCliente.ClaveEntidadFiscalCliente, Method.GET);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            // Se obtienen las cotizaciones que han sido generadas en el período
            IRestResponse resp = rest.Execute(req);
            if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
            {
                List<InfoCotizaciones> lista = JsonConvert.DeserializeObject<List<InfoCotizaciones>>(resp.Content);
                ListaCtzUltimas = new ObservableCollection<InfoCotizaciones>(lista.OrderBy(x => x.CodigoDeComprobante));
            }
            // Se obtienen las cotizaciones que han sido facturadas en el período
            req = new RestRequest("obtenerCtzFacturadas/" + Usuario.ClaveEntidadFiscalInmueble + "/" + FechaInicio.ToString("yyyy-MM-dd") + "/"
                                  + FechaFinal.ToString("yyyy-MM-dd") + "/" + InfoCliente.ClaveEntidadFiscalCliente, Method.GET);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            resp = rest.Execute(req);
            if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
            {
                List<InfoFacturas> lista = JsonConvert.DeserializeObject<List<InfoFacturas>>(resp.Content);
                ListaCtzFacturadas = new ObservableCollection<InfoFacturas>(lista.OrderBy(x => x.CodigoFactura));
            }
            // Se obtienen los últimos productos cotizados en el período
            req = new RestRequest("obtenerListaProdCotizados/" + Usuario.ClaveEntidadFiscalInmueble + "/" + FechaInicio.ToString("yyyy-MM-dd") + "/"
                                  + FechaFinal.ToString("yyyy-MM-dd") + "/" + InfoCliente.ClaveEntidadFiscalCliente, Method.GET);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            IRestResponse respv = rest.Execute(req);
            if (respv.IsSuccessful == true && respv.StatusCode == HttpStatusCode.OK)
            {
                List<ProductoVendido> ProdVendJson = JsonConvert.DeserializeObject<List<ProductoVendido>>(respv.Content);
                ListaProdCotizados = new ObservableCollection<ProductoVendido>(ProdVendJson);
            }
            // Se obtienen los últimos productos vendidos en el período
            req = new RestRequest("obtenerListaProdVendidos/" + Usuario.ClaveEntidadFiscalInmueble + "/" + FechaInicio.ToString("yyyy-MM-dd") + "/"
                                  + FechaFinal.ToString("yyyy-MM-dd") + "/" + InfoCliente.ClaveEntidadFiscalCliente, Method.GET);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            respv = rest.Execute(req);
            if (respv.IsSuccessful == true && respv.StatusCode == HttpStatusCode.OK)
            {
                List<ProductoVendido> ProdVendJson = JsonConvert.DeserializeObject<List<ProductoVendido>>(respv.Content);
                ListaProdVendidos = new ObservableCollection<ProductoVendido>(ProdVendJson);
            }
            // Muestra un mensaje si no existen datos que mostrar en los cuadros
            if (ListaCtzUltimas.Count == 0 && ListaCtzFacturadas.Count == 0 && ListaProdCotizados.Count == 0 && ListaProdVendidos.Count == 0)
            {
                TxtMensaje = "No se encontró información sobre cotizaciones del Cliente seleccionado en este período y en esta sucursal.";
                VerMensaje = true;
            }
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;

        private void VerCtzGenerada(object parameter)
        {
            string numctz = parameter as string;
            var comp = ListaCtzUltimas.Where(x => x.ClaveComprobanteDeCotizacion == numctz).Single();
            CargarDetallesCtz(numctz, comp.CodigoDeComprobante, string.Empty);
        }

        private void VerCtzFacturada(object parameter)
        {
            string numctz = parameter as string;
            var comp = ListaCtzFacturadas.Where(x => x.ClaveComprobante == numctz).Single();
            CargarDetallesCtz(numctz, comp.CodigoFactura, comp.RazonSocial);
        }

        private async void CargarDetallesCtz(string numctz, string infoctz, string razonSocial)
        {
            var rest = new RestClient(Localhost);
            var req = new RestRequest("cargarDetallesCotizacion/" + numctz, Method.GET);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            IRestResponse resp = rest.Execute(req);
            if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
            {
                List<InfoDetallesCotizacion> detalles = JsonConvert.DeserializeObject<List<InfoDetallesCotizacion>>(resp.Content);
                ListaProductos = new ObservableCollection<ProductoSeleccionado>();
                CantidadTotal = 0;
                ImporteTotal = 0;
                DescuentoTotal = 0;
                SumaTotal = 0;
                foreach (InfoDetallesCotizacion fila in detalles)
                {
                    double pdesc = Math.Round(fila.ImporteDescuento / (fila.PrecioUnitario * fila.Cantidad), 2);
                    ProductoSeleccionado psel = new ProductoSeleccionado
                    {
                        Cantidad = fila.Cantidad,
                        Descuento = pdesc,
                        Importe = fila.Importe,
                        ImporteDesc = fila.ImporteDescuento,
                        DesctoUnitario = Math.Round(fila.PrecioUnitario * pdesc, 2),
                        Impuesto = fila.Impuestos,
                        SubTotal = fila.Subtotal,
                        Estatus = 3,
                        ClaveDetalleDeComprobante = fila.ClaveDetalleDeComprobante,
                        DiasEntrega = fila.DiasDeEntrega,
                        Producto = new Producto
                        {
                            ClaveProducto = fila.ClaveProducto,
                            Descripcion = fila.Descripcion.Trim(),
                            PrecioUnitario = fila.PrecioUnitario,
                            SumaImpuestos = fila.SumaImpuestos,
                            CodigoInterno = fila.CodigoInterno,
                            Tasas = fila.Tasas,
                            ClavesImpuestos = fila.ClavesImpuestos
                        }
                    };
                    CantidadTotal += psel.Cantidad;
                    ImporteTotal += psel.Importe;
                    DescuentoTotal += psel.ImporteDesc;
                    SumaTotal += psel.SubTotal;
                    ListaProductos.Add(psel);
                }
                var vmDetCtz = new DetalleCotizacionViewModel
                {
                    ListaProductos = ListaProductos,
                    NumCotizacion = (infoctz.Contains("CTZ") == true) ? "Cotización: " + infoctz : "# Factura: " + infoctz,
                    RazonSocial = (infoctz.Contains("CTZ") == true) ? string.Empty : "Vendedor: " + razonSocial,
                    CantidadTotal = CantidadTotal,
                    DescuentoTotal = DescuentoTotal,
                    ImporteTotal = ImporteTotal,
                    SumaTotal = SumaTotal
                };
                var vwDetCtz = new DetalleCotizacionView
                {
                    DataContext = vmDetCtz
                };
                var dlgHost = await DialogHost.Show(vwDetCtz, "HistorialCliente");
            }
        }
        #endregion
    }
}