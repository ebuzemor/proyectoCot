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
    public class ReporteVendedorViewModel : Notificador
    {
        #region Commands
        public RelayCommand ElegirVendedorCommand { get; set; }
        public RelayCommand ElegirPeriodoCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
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
        private bool _verMensaje;
        private object _datoSeleccionado;
        private DatosUsuarios _datUsuario;
        private ObservableCollection<AccionesDefinidas> _listaAcciones;
        private ObservableCollection<DatosGraficas> _datosReporte;
        private ObservableCollection<DatosGraficas> _datosCotizaciones;
        private ObservableCollection<DatosGraficas> _datosMontosCtz;
        private ObservableCollection<DatosGraficas> _datosDescuentosCtz;
        private ObservableCollection<DatosGraficas> _datosCtzFacturadas;
        private ObservableCollection<DatosGraficas> _datosDscFacturados;
        private List<DatosSeries> _listaSeries;

        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public Usuario InfoUsr { get => _infoUsr; set { _infoUsr = value; OnPropertyChanged(); } }
        public DateTime FechaInicio { get => _fechaInicio; set { _fechaInicio = value; OnPropertyChanged(); } }
        public DateTime FechaFinal { get => _fechaFinal; set { _fechaFinal = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public string TxtPeriodo { get => _txtPeriodo; set { _txtPeriodo = value; OnPropertyChanged(); } }
        public string TxtVendedor { get => _txtVendedor; set { _txtVendedor = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public object DatoSeleccionado { get => _datoSeleccionado; set { _datoSeleccionado = value; OnPropertyChanged(); } }
        public DatosUsuarios DatUsuario { get => _datUsuario; set { _datUsuario = value; OnPropertyChanged(); } }
        public ObservableCollection<AccionesDefinidas> ListaAcciones { get => _listaAcciones; set { _listaAcciones = value; OnPropertyChanged(); } }
        public ObservableCollection<DatosGraficas> DatosReporte { get => _datosReporte; set { _datosReporte = value; OnPropertyChanged(); } }
        public ObservableCollection<DatosGraficas> DatosCotizaciones { get => _datosCotizaciones; set { _datosCotizaciones = value; OnPropertyChanged(); } }
        public ObservableCollection<DatosGraficas> DatosMontosCtz { get => _datosMontosCtz; set { _datosMontosCtz = value; OnPropertyChanged(); } }
        public ObservableCollection<DatosGraficas> DatosDescuentosCtz { get => _datosDescuentosCtz; set { _datosDescuentosCtz = value; OnPropertyChanged(); } }
        public ObservableCollection<DatosGraficas> DatosCtzFacturadas { get => _datosCtzFacturadas; set { _datosCtzFacturadas = value; OnPropertyChanged(); } }
        public ObservableCollection<DatosGraficas> DatosDscFacturados { get => _datosDscFacturados; set { _datosDscFacturados = value; OnPropertyChanged(); } }
        public List<DatosSeries> ListaSeries { get => _listaSeries; set { _listaSeries = value; OnPropertyChanged(); } }        
        #endregion

        #region Constructor
        public ReporteVendedorViewModel()
        {
            ElegirPeriodoCommand = new RelayCommand(ElegirPeriodo);
            ElegirVendedorCommand = new RelayCommand(ElegirVendedor);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
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
                    double ctzfac = Math.Round(rptVen.TotalCtzsFact * 100.0 / rptVen.TotalCtzs, 2);
                    double canfac = Math.Round(rptVen.Facturadas * 100.0 / rptVen.NumCotizaciones, 2);//((rptVen.TotalDesctos > 0) ? rptVen.TotalDesctos : 1), 2);
                    double ctefac = Math.Round(rptVen.MaxCtzCteFact * 100.0 / rptVen.TotalCtzs, 2);
                    DatosReporte = new ObservableCollection<DatosGraficas>
                    {
                        new DatosGraficas { Categoria = "Monto Cotizaciones Facturadas", Numero = ctzfac },
                        new DatosGraficas { Categoria = "Cantidad Cotizaciones Facturadas", Numero = canfac },
                        new DatosGraficas { Categoria = "Monto Max. Facturado Cliente", Numero = ctefac }
                    };

                    DatosCotizaciones = new ObservableCollection<DatosGraficas>
                    {
                        new DatosGraficas { Categoria = "Borrador", Numero = rptVen.Borrador },
                        new DatosGraficas { Categoria = "Pendientes", Numero = rptVen.Pendientes },
                        new DatosGraficas { Categoria = "Autorizadas", Numero = rptVen.Autorizadas },
                        new DatosGraficas { Categoria = "Canceladas", Numero = rptVen.Canceladas },
                        new DatosGraficas { Categoria = "Facturadas", Numero = rptVen.Facturadas },
                        new DatosGraficas { Categoria = "Máx. Cotizaciones Cliente", Numero = rptVen.TotalCtzsCte }
                    };

                    DatosMontosCtz = new ObservableCollection<DatosGraficas>
                    {
                        new DatosGraficas { Categoria = "Total Máximo por Cliente", Numero = rptVen.MaxCtzCte },
                        new DatosGraficas { Categoria = "Total General Cotizaciones", Numero = rptVen.TotalCtzs }
                    };

                    DatosDescuentosCtz = new ObservableCollection<DatosGraficas>
                    {
                        new DatosGraficas { Categoria = "Máximo Descuento Cliente", Numero = rptVen.MaxDesctoCte },
                        new DatosGraficas { Categoria = "Total Descuentos Cotizaciones", Numero = rptVen.TotalDesctos }
                    };

                    ListaSeries = new List<DatosSeries>
                    {
                        new DatosSeries() { MostrarNombre = "Montos", Items = DatosMontosCtz },
                        new DatosSeries() { MostrarNombre = "Descuentos", Items = DatosDescuentosCtz }
                    };

                    DatosCtzFacturadas = new ObservableCollection<DatosGraficas>
                    {
                        new DatosGraficas { Categoria = "Monto Máx. Facturado Cliente", Numero = rptVen.MaxCtzCteFact },
                        new DatosGraficas { Categoria = "Total Facturado Cotizaciones", Numero = Math.Round(rptVen.TotalCtzsFact - rptVen.MaxCtzCteFact, 2) }
                    };

                    DatosDscFacturados = new ObservableCollection<DatosGraficas>
                    {
                        new DatosGraficas { Categoria = "Descto. Máx. Facturado Cliente", Numero = rptVen.MaxDesctoCteFact },
                        new DatosGraficas { Categoria = "Total Desctos. Facturado Cotizaciones", Numero = Math.Round(rptVen.TotalCtzsDesctoFact - rptVen.MaxDesctoCteFact, 2) }
                    };
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
            var msUltima = await DialogHost.Show(vwElegir, "ReporteVendedor");
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
            var result = await DialogHost.Show(vwBuscar, "ReporteVendedor");
            if (result.Equals("DatUsuario") == true)
            {
                DatUsuario = vmBuscar.DatUsuario;
                TxtVendedor = "Vendedor: " + DatUsuario.RazonSocial;
                ObtenerReporte(false);
            }
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;
        #endregion
    }
}