using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Media;

namespace Cotizador.ViewModel
{
    public class ReporteVendedor2ViewModel : Notificador
    {
        #region Commands
        public RelayCommand ElegirVendedorCommand { get; set; }
        public RelayCommand ElegirPeriodoCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        public RelayCommand ClienteMaxCtzCommand { get; set; }
        public RelayCommand ClienteDscMaxCommand { get; set; }
        public RelayCommand ClienteMaxFacCommand { get; set; }
        public RelayCommand ClienteDscFacCommand { get; set; }
        #endregion

        #region Variables
        private ApiKey _appKey;
        private Usuario _usuario;
        private Usuario _infoUsr;
        private DateTime _fechaInicio;
        private DateTime _fechaFinal;
        private string _localhost;
        private string _txtPeriodo;
        private string _txtVendedor;
        private string _txtMensaje;
        private string[] _lblGenCtz;
        private string[] _lblMontos;
        private bool _verMensaje;
        private DatosUsuarios _datUsuario;
        private ObservableCollection<AccionesDefinidas> _listaAcciones;
        private SeriesCollection _seriesCotizaciones;
        private SeriesCollection _seriesMontosCtz;
        private SeriesCollection _seriesCtzFacturadas;
        private SeriesCollection _seriesDscFacturados;
        private Func<double, string> _porcFormato;
        private Func<double, string> _montFormato;
        private Func<double, string> _numFormato;
        private double _montoCtzFact;
        private double _cantCtzFact;
        private double _montoMaxCte;
        private double _datosCteCtz;
        private double _datosCteDsc;
        private double _datosCteCtzFac;
        private double _datosCteDscFac;

        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public Usuario InfoUsr { get => _infoUsr; set { _infoUsr = value; OnPropertyChanged(); } }
        public DateTime FechaInicio { get => _fechaInicio; set { _fechaInicio = value; OnPropertyChanged(); } }
        public DateTime FechaFinal { get => _fechaFinal; set { _fechaFinal = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public string TxtPeriodo { get => _txtPeriodo; set { _txtPeriodo = value; OnPropertyChanged(); } }
        public string TxtVendedor { get => _txtVendedor; set { _txtVendedor = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public string[] LblGenCtz { get => _lblGenCtz; set { _lblGenCtz = value; OnPropertyChanged(); } }
        public string[] LblMontos { get => _lblMontos; set { _lblMontos = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public DatosUsuarios DatUsuario { get => _datUsuario; set { _datUsuario = value; OnPropertyChanged(); } }
        public ObservableCollection<AccionesDefinidas> ListaAcciones { get => _listaAcciones; set { _listaAcciones = value; OnPropertyChanged(); } }
        public SeriesCollection SeriesCotizaciones { get => _seriesCotizaciones; set { _seriesCotizaciones = value; OnPropertyChanged(); } }
        public SeriesCollection SeriesMontosCtz { get => _seriesMontosCtz; set { _seriesMontosCtz = value; OnPropertyChanged(); } }
        public SeriesCollection SeriesCtzFacturadas { get => _seriesCtzFacturadas; set { _seriesCtzFacturadas = value; OnPropertyChanged(); } }
        public SeriesCollection SeriesDscFacturados { get => _seriesDscFacturados; set { _seriesDscFacturados = value; OnPropertyChanged(); } }
        public Func<double, string> PorcFormato { get => _porcFormato; set { _porcFormato = value; OnPropertyChanged(); } }
        public Func<double, string> MontFormato { get => _montFormato; set { _montFormato = value; OnPropertyChanged(); } }
        public Func<double, string> NumFormato { get => _numFormato; set { _numFormato = value; OnPropertyChanged(); } }
        public double MontoCtzFact { get => _montoCtzFact; set { _montoCtzFact = value; OnPropertyChanged(); } }
        public double CantCtzFact { get => _cantCtzFact; set { _cantCtzFact = value; OnPropertyChanged(); } }
        public double MontoMaxCte { get => _montoMaxCte; set { _montoMaxCte = value; OnPropertyChanged(); } }
        public double DatosCteCtz { get => _datosCteCtz; set { _datosCteCtz = value; OnPropertyChanged(); } }
        public double DatosCteDsc { get => _datosCteDsc; set { _datosCteDsc = value; OnPropertyChanged(); } }
        public double DatosCteCtzFac { get => _datosCteCtzFac; set { _datosCteCtzFac = value; OnPropertyChanged(); } }
        public double DatosCteDscFac { get => _datosCteDscFac; set { _datosCteDscFac = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public ReporteVendedor2ViewModel()
        {
            ElegirPeriodoCommand = new RelayCommand(ElegirPeriodo);
            ElegirVendedorCommand = new RelayCommand(ElegirVendedor);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            ClienteMaxCtzCommand = new RelayCommand(ClienteMaxCtz);
            ClienteDscMaxCommand = new RelayCommand(ClienteDscMax);
            ClienteMaxFacCommand = new RelayCommand(ClienteMaxFac);
            ClienteDscFacCommand = new RelayCommand(ClienteDscFac);
            DateTime hoy = DateTime.Now;
            FechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            FechaFinal = FechaInicio.AddMonths(1).AddDays(-1);
            TxtPeriodo = " | Período: " + FechaInicio.ToString("dd-MM-yyyy") + " al " + FechaFinal.ToString("dd-MM-yyyy");
        }
        #endregion

        #region Métodos
        public void ObtenerReporte(bool UsuarioInicio)
        {
            if (UsuarioInicio == true)
            {
                DatUsuario = new DatosUsuarios
                {
                    ClaveEntidadFiscalUsuario = Usuario.ClaveEntidadFiscalEmpleado,
                    Nickname = Usuario.NombreUsuario,
                    RazonSocial = Usuario.RazonSocialUsuario
                };
            }
            TxtVendedor = "Vendedor: " + DatUsuario.RazonSocial;
            var rest = new RestClient(Localhost);
            var req = new RestRequest("reporteCotizacionesUsr", Method.POST);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            req.AddParameter("claveEF_Inmueble", ""); // Usuario.ClaveEntidadFiscalInmueble);
            req.AddParameter("claveEF_Usuario", DatUsuario.ClaveEntidadFiscalUsuario);
            req.AddParameter("fechaInicio", FechaInicio.ToString("yyyy-MM-dd"));
            req.AddParameter("fechaFinal", FechaFinal.ToString("yyyy-MM-dd"));

            //Se obtienen los datos de las cotizaciones generadas en el período
            IRestResponse resp = rest.Execute(req);
            if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
            {
                List<ReporteVendedor> listaRpt = JsonConvert.DeserializeObject<List<ReporteVendedor>>(resp.Content);
                if (listaRpt != null && listaRpt.Count > 0)
                {
                    ReporteVendedor rptVen = listaRpt.First();
                    //COTIZACIONES GENERADAS
                    SeriesCotizaciones = new SeriesCollection
                    {
                        new ColumnSeries { Title = "Borrador", Foreground = Brushes.White, DataLabels = true, Values = new ChartValues<int> { rptVen.Borrador} },
                        new ColumnSeries { Title = "Pendientes", Foreground = Brushes.White, DataLabels = true, Values = new ChartValues<int> { rptVen.Pendientes} },
                        new ColumnSeries { Title = "Autorizadas", Foreground = Brushes.White, DataLabels = true, Values = new ChartValues<int> { rptVen.Autorizadas} },
                        new ColumnSeries { Title = "Canceladas", Foreground = Brushes.White, DataLabels = true, Values = new ChartValues<int> { rptVen.Canceladas} },
                        new ColumnSeries { Title = "Facturadas", Foreground = Brushes.White, DataLabels = true, Values = new ChartValues<int> { rptVen.Facturadas} },
                        new ColumnSeries { Title = "Máx. Cotizaciones Cliente", Foreground = Brushes.White, DataLabels = true, Values = new ChartValues<double> { rptVen.TotalCtzsCte} }
                    };
                    LblGenCtz = new[] { "Borrador", "Pendientes", "Autorizadas", "Canceladas", "Facturadas", "Máx. Cotizaciones Cliente" };
                    NumFormato = value => value.ToString("N");
                    //MONTOS EN COTIZACIONES
                    SeriesMontosCtz = new SeriesCollection
                    {
                        new RowSeries { Title = "Total Máximo por Cliente", Foreground = Brushes.White, DataLabels = true, LabelPoint = point => point.X.ToString("C2"), Values = new ChartValues<double> { rptVen.MaxCtzCte } },
                        new RowSeries { Title = "Total General Cotizaciones", Foreground = Brushes.White, DataLabels = true, LabelPoint = point => point.X.ToString("C2"), Values = new ChartValues<double> { rptVen.TotalCtzs } },
                        new RowSeries { Title = "Máximo Descuento Cliente", Foreground = Brushes.White, DataLabels = true, LabelPoint = point => point.X.ToString("C2"), Values = new ChartValues<double> { rptVen.MaxDesctoCte } },
                        new RowSeries { Title = "Total Descuentos Cotizaciones", Foreground = Brushes.White, DataLabels = true, LabelPoint = point => point.X.ToString("C2"), Values = new ChartValues<double> { rptVen.TotalDesctos } }
                    };
                    LblMontos = new[] { "Total Máximo por Cliente", "Total General Cotizaciones", "Máximo Descuento Cliente", "Total Descuentos Cotizaciones" };
                    MontFormato = value => value.ToString("C2");
                    //COTIZACIONES FACTURADAS
                    SeriesCtzFacturadas = new SeriesCollection
                    {
                        new PieSeries
                        {
                            Title = "Monto Máx. Facturado Cliente",
                            Foreground = Brushes.White,
                            DataLabels = true,
                            LabelPoint = chartPoint => string.Format("{0:C2}", chartPoint.Y, chartPoint.Participation),
                            Values = new ChartValues<double>{ rptVen.MaxCtzCteFact }
                        },
                        new PieSeries
                        {
                            Title = "Total Facturado Cotizaciones",
                            Foreground = Brushes.White,
                            DataLabels = true,
                            LabelPoint = chartPoint => string.Format("{0:C2}", chartPoint.Y, chartPoint.Participation),
                            Values = new ChartValues<double>{ Math.Round(rptVen.TotalCtzsFact - rptVen.MaxCtzCteFact, 2) }
                        }
                    };
                    SeriesDscFacturados = new SeriesCollection
                    {
                        new PieSeries
                        {
                            Title = "Descto. Máx. Facturado Cliente",
                            Foreground = Brushes.White,
                            Fill = new SolidColorBrush(Color.FromRgb(178, 34, 34)),
                            DataLabels = true,
                            LabelPoint = chartPoint => string.Format("{0:C2}", chartPoint.Y, chartPoint.Participation),
                            Values = new ChartValues<double>{ rptVen.MaxDesctoCteFact }
                        },
                        new PieSeries
                        {
                            Title = "Total Desctos. Facturado Cotizaciones",
                            Foreground = Brushes.White,
                            Fill = new SolidColorBrush(Color.FromRgb(68, 39, 161)),
                            DataLabels = true,
                            LabelPoint = chartPoint => string.Format("{0:C2}", chartPoint.Y, chartPoint.Participation),
                            Values = new ChartValues<double>{ Math.Round(rptVen.TotalCtzsDesctoFact - rptVen.MaxDesctoCteFact, 2) }
                        }
                    };
                    //PORCENTAJE COTIZACIONES
                    MontoCtzFact = Math.Round(rptVen.TotalCtzsFact / rptVen.TotalCtzs, 4);
                    CantCtzFact = Math.Round(rptVen.Facturadas * 1.0 / rptVen.NumCotizaciones, 4);
                    MontoMaxCte = Math.Round(rptVen.MaxCtzCteFact / rptVen.TotalCtzs, 4);
                    PorcFormato = value => value.ToString("P2");
                    //DATOS PARA MOSTRAR CLIENTES
                    DatosCteCtz = rptVen.MaxCtzCte;
                    DatosCteDsc = rptVen.MaxDesctoCte;
                    DatosCteCtzFac = rptVen.MaxCtzCteFact;
                    DatosCteDscFac = rptVen.MaxDesctoCteFact;
                }
                else
                {
                    TxtMensaje = "No existe información de cotizaciones generadas por el vendedor en el período establecido";
                    VerMensaje = true;
                }
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
            var msUltima = await DialogHost.Show(vwElegir, "ReporteVendedor2");
            if (msUltima.Equals("OK") == true)
            {
                FechaInicio = vmElegir.FechaInicial;
                FechaFinal = vmElegir.FechaFinal;
                if (FechaInicio < FechaFinal)
                {
                    TxtPeriodo = "Período: " + FechaInicio.ToString("dd-MM-yyyy") + " al " + FechaFinal.ToString("dd-MM-yyyy");
                    ObtenerReporte(false);
                }
                else
                {
                    TxtMensaje = "La fecha de inicio del período debe ser menor o igual a la fecha final, por favor corrija.";
                    VerMensaje = true;
                }
            }
        }

        private async void ElegirVendedor(object parameter)
        {
            var vmBuscar = new BuscarUsuariosViewModel
            {
                AppKey = AppKey,
                Localhost = Localhost,
                Usuario = Usuario
            };
            var vwBuscar = new BuscarUsuariosView
            {
                DataContext = vmBuscar
            };
            var result = await DialogHost.Show(vwBuscar, "ReporteVendedor2");
            if (result.Equals("DatUsuario") == true)
            {
                if (vmBuscar.DatUsuario != null)
                {
                    DatUsuario = vmBuscar.DatUsuario;
                    TxtVendedor = "Vendedor: " + DatUsuario.RazonSocial;
                    ObtenerReporte(false);
                }
                else
                {
                    TxtMensaje = "Debe seleccionar un usuario para generar el reporte";
                    VerMensaje = true;
                }
            }
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;

        private async void ClienteMaxCtz(object parameter)
        {
            var rest = new RestClient(Localhost);
            var req = new RestRequest("datosClienteMaxCtz", Method.POST);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            req.AddParameter("claveEF_Inmueble", ""); // Usuario.ClaveEntidadFiscalInmueble);
            req.AddParameter("claveEF_Usuario", DatUsuario.ClaveEntidadFiscalUsuario);
            req.AddParameter("fechaInicio", FechaInicio.ToString("yyyy-MM-dd"));
            req.AddParameter("fechaFinal", FechaFinal.ToString("yyyy-MM-dd"));
            req.AddParameter("montoCotizado", DatosCteCtz);

            //Se obtienen los datos de las cotizaciones generadas en el período
            IRestResponse resp = rest.Execute(req);
            if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
            {
                var ltaDatosCte = JsonConvert.DeserializeObject<List<DatosClientesCtz>>(resp.Content);
                if (ltaDatosCte.Count > 0)
                {
                    var vmMsj = new MensajeViewModel
                    {
                        TituloMensaje = "Información",
                        CuerpoMensaje = "Cliente con mayor monto cotizado:\n" + ltaDatosCte.First().NombreCliente,
                        MostrarCancelar = false
                    };
                    var vwMsj = new MensajeView
                    {
                        DataContext = vmMsj
                    };
                    var resMsj = await DialogHost.Show(vwMsj, "ReporteVendedor2");
                }
                else
                {
                    TxtMensaje = "No es posible encontrar información del Cliente";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "Error al mostrar información del Cliente";
                VerMensaje = true;
            }
        }

        private async void ClienteDscMax(object parameter)
        {
            var rest = new RestClient(Localhost);
            var req = new RestRequest("datosClienteDscMax", Method.POST);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            req.AddParameter("claveEF_Inmueble", ""); // Usuario.ClaveEntidadFiscalInmueble);
            req.AddParameter("claveEF_Usuario", DatUsuario.ClaveEntidadFiscalUsuario);
            req.AddParameter("fechaInicio", FechaInicio.ToString("yyyy-MM-dd"));
            req.AddParameter("fechaFinal", FechaFinal.ToString("yyyy-MM-dd"));
            req.AddParameter("montoDescuento", DatosCteDsc);

            //Se obtienen los datos del cliente con mayor descuento
            IRestResponse resp = rest.Execute(req);
            if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
            {
                var ltaDatosCte = JsonConvert.DeserializeObject<List<DatosClientesCtz>>(resp.Content);
                if (ltaDatosCte.Count > 0)
                {
                    var vmMsj = new MensajeViewModel
                    {
                        TituloMensaje = "Información",
                        CuerpoMensaje = "Cliente con mayor descuento cotizado:\n" + ltaDatosCte.First().NombreCliente,
                        MostrarCancelar = false
                    };
                    var vwMsj = new MensajeView
                    {
                        DataContext = vmMsj
                    };
                    var resMsj = await DialogHost.Show(vwMsj, "ReporteVendedor2");
                }
                else
                {
                    TxtMensaje = "No es posible encontrar información del Cliente";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "Error al mostrar información del Cliente";
                VerMensaje = true;
            }
        }

        private async void ClienteMaxFac(object parameter)
        {
            var rest = new RestClient(Localhost);
            var req = new RestRequest("datosClienteMaxFac", Method.POST);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            req.AddParameter("claveEF_Inmueble", ""); // Usuario.ClaveEntidadFiscalInmueble);
            req.AddParameter("claveEF_Usuario", DatUsuario.ClaveEntidadFiscalUsuario);
            req.AddParameter("fechaInicio", FechaInicio.ToString("yyyy-MM-dd"));
            req.AddParameter("fechaFinal", FechaFinal.ToString("yyyy-MM-dd"));
            req.AddParameter("montoFacturado", DatosCteCtzFac);

            //Se obtienen los datos del cliente con mayor descuento
            IRestResponse resp = rest.Execute(req);
            if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
            {
                var ltaDatosCte = JsonConvert.DeserializeObject<List<DatosClientesCtz>>(resp.Content);
                if (ltaDatosCte.Count > 0)
                {
                    var vmMsj = new MensajeViewModel
                    {
                        TituloMensaje = "Información",
                        CuerpoMensaje = "Cliente con mayor monto facturado:\n" + ltaDatosCte.First().NombreCliente,
                        MostrarCancelar = false
                    };
                    var vwMsj = new MensajeView
                    {
                        DataContext = vmMsj
                    };
                    var resMsj = await DialogHost.Show(vwMsj, "ReporteVendedor2");
                }
                else
                {
                    TxtMensaje = "No es posible encontrar información del Cliente";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "Error al mostrar información del Cliente";
                VerMensaje = true;
            }
        }

        private async void ClienteDscFac(object parameter)
        {
            var rest = new RestClient(Localhost);
            var req = new RestRequest("datosClienteDscFac", Method.POST);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            req.AddParameter("claveEF_Inmueble", ""); // Usuario.ClaveEntidadFiscalInmueble);
            req.AddParameter("claveEF_Usuario", DatUsuario.ClaveEntidadFiscalUsuario);
            req.AddParameter("fechaInicio", FechaInicio.ToString("yyyy-MM-dd"));
            req.AddParameter("fechaFinal", FechaFinal.ToString("yyyy-MM-dd"));
            req.AddParameter("desctoFacturado", DatosCteDscFac);

            //Se obtienen los datos del cliente con mayor descuento
            IRestResponse resp = rest.Execute(req);
            if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
            {
                var ltaDatosCte = JsonConvert.DeserializeObject<List<DatosClientesCtz>>(resp.Content);
                if (ltaDatosCte.Count > 0)
                {
                    var vmMsj = new MensajeViewModel
                    {
                        TituloMensaje = "Información",
                        CuerpoMensaje = "Cliente con mayor descuento facturado:\n" + ltaDatosCte.First().NombreCliente,
                        MostrarCancelar = false
                    };
                    var vwMsj = new MensajeView
                    {
                        DataContext = vmMsj
                    };
                    var resMsj = await DialogHost.Show(vwMsj, "ReporteVendedor2");
                }
                else
                {
                    TxtMensaje = "No es posible encontrar información del Cliente";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "Error al mostrar información del Cliente";
                VerMensaje = true;
            }
        }
        #endregion
    }
}