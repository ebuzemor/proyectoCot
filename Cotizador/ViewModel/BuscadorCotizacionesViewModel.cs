using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using System;
using MaterialDesignThemes.Wpf;
using System.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Cotizador.ViewModel
{
    public class BuscadorCotizacionesViewModel : Notificador
    {
        #region Commands
        public RelayCommand BuscarCotizacionesCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        #endregion

        #region Variables
        private ApiKey _appKey;
        private Usuario _usuario;
        private String _localhost;
        private String _claveEF_Empresa;
        private Boolean _verMensaje;
        private String _txtMensaje;
        private EstatusCotizacion _estatusCotizacion;
        private ObservableCollection<EstatusCotizacion> _listaEstatusCtz;
        private ObservableCollection<Sucursal> _listaSucursales;
        private ObservableCollection<InfoCotizaciones> _listaCotizaciones;
        private InfoCotizaciones _infoCotizacion;
        private DateTime _fechaInicial;
        private DateTime _fechaFinal;
        private String _txtUsuario;
        private Sucursal _miSucursal;

        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged("VerMensaje"); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged("TxtMensaje"); } }
        public EstatusCotizacion EstatusCotizacion { get => _estatusCotizacion; set { _estatusCotizacion = value; OnPropertyChanged("EstatusCotizacion"); } }
        public ObservableCollection<EstatusCotizacion> ListaEstatusCtz { get => _listaEstatusCtz; set { _listaEstatusCtz = value; OnPropertyChanged("ListaEstatusCtz"); } }
        public ObservableCollection<Sucursal> ListaSucursales { get => _listaSucursales; set { _listaSucursales = value; OnPropertyChanged("ListaSucursales"); } }
        public DateTime FechaInicial { get => _fechaInicial; set { _fechaInicial = value; OnPropertyChanged("FechaInicial"); } }
        public DateTime FechaFinal { get => _fechaFinal; set { _fechaFinal = value; OnPropertyChanged("FechaFinal"); } }
        public string TxtUsuario { get => _txtUsuario; set { _txtUsuario = value; OnPropertyChanged("TxtUsuario"); } }
        public Sucursal MiSucursal { get => _miSucursal; set { _miSucursal = value; OnPropertyChanged("MiSucursal"); } }
        public string ClaveEF_Empresa { get => _claveEF_Empresa; set { _claveEF_Empresa = value; OnPropertyChanged("ClaveEF_Empresa"); } }
        public ObservableCollection<InfoCotizaciones> ListaCotizaciones { get => _listaCotizaciones; set { _listaCotizaciones = value; OnPropertyChanged("ListaCotizaciones"); } }
        public InfoCotizaciones InfoCotizacion { get => _infoCotizacion; set { _infoCotizacion = value; OnPropertyChanged("InfoCotizacion"); } }
        #endregion

        #region Constructor
        public BuscadorCotizacionesViewModel()
        {
            BuscarCotizacionesCommand = new RelayCommand(BuscarCotizaciones);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
        }
        #endregion

        #region Métodos
        public void CargarEstatusCotizacion()
        {
            try
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("estatusCotizacion", Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse resp = rest.Execute(req);
                if (resp.IsSuccessful && resp.StatusCode == HttpStatusCode.OK)
                {
                    List<EstatusCotizacion> lista = JsonConvert.DeserializeObject<List<EstatusCotizacion>>(resp.Content);
                    ListaEstatusCtz = new ObservableCollection<EstatusCotizacion>(lista);
                    //se crea y se inserta una opción que permite buscar por todos los tipos de estatus
                    EstatusCotizacion estcual = new EstatusCotizacion { ClaveTipoDeComprobante = 0, ClaveTipoDeStatusDeComprobante = 0, Descripcion = "Cualquiera" };
                    ListaEstatusCtz.Insert(0, estcual);
                }
            }
            catch (Exception)
            {
                TxtMensaje = "Error al cargar estatus de la cotización, verifique que el servidor esté en línea";
                VerMensaje = true;
            }
        }

        public void CargarSucursales()
        {
            try
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("listaSucursales/" + Usuario.ClaveEntidadFiscalEmpresa, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse<SucursalesJson> resp = rest.Execute<SucursalesJson>(req);
                if (resp.IsSuccessful && resp.StatusCode == HttpStatusCode.OK)
                {
                    SucursalesJson jsonSuc = JsonConvert.DeserializeObject<SucursalesJson>(resp.Content);
                    ListaSucursales = new ObservableCollection<Sucursal>(jsonSuc.Sucursales);
                    //Se crea y se inserta una opción que permite buscar por cualquier sucursal
                    //Sucursal cualsuc = new Sucursal { ClaveEntidadFiscalEmpresa = 0, ClaveEntidadFiscalInmueble = 0, ClaveInmueble = 0, CodigoDeInmueble = "0", NombreCorto = "Cualquiera" };
                    //ListaSucursales.Insert(0, cualsuc);
                }
                else
                {
                    TxtMensaje = "Error al cargar las sucursales, verifique que el servidor esté en línea";
                    VerMensaje = true;
                }
            }
            catch (Exception)
            {
                TxtMensaje = "Error al cargar las sucursales, verifique que el servidor esté en línea";
                VerMensaje = true;
            }            
        }

        private void BuscarCotizaciones(object parameter)
        {
            if (MiSucursal != null)
            {
                try
                {
                    var rest = new RestClient(Localhost);
                    var req = new RestRequest("mostrarCotizaciones", Method.POST);
                    req.AddHeader("Accept", "application/json");
                    req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                    req.AddParameter("claveEF_Inmueble", Usuario.ClaveEntidadFiscalInmueble);
                    req.AddParameter("fechaInicial", "");
                    req.AddParameter("fechaFinal", "");
                    req.AddParameter("txtCliente", "");
                    req.AddParameter("claveTipoEstatus", "");

                    IRestResponse resp = rest.Execute(req);
                    if (resp.IsSuccessful && resp.StatusCode == HttpStatusCode.OK)
                    {
                        List<InfoCotizaciones> lista = JsonConvert.DeserializeObject<List<InfoCotizaciones>>(resp.Content);
                        ListaCotizaciones = new ObservableCollection<InfoCotizaciones>(lista);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;
        #endregion
    }
}
