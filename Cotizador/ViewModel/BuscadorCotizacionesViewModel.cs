﻿using Cotizador.Common;
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
        public RelayCommand VerCotizacionCommand { get; set; }
        #endregion

        #region Variables
        private ApiKey _appKey;
        private Usuario _usuario;
        private String _localhost;
        private String _claveEF_Empresa;
        private Boolean _verMensaje;
        private String _txtMensaje;
        private EstatusCotizacion _actualEstatus;
        private ObservableCollection<EstatusCotizacion> _listaEstatusCtz;
        private ObservableCollection<Sucursal> _listaSucursales;
        private ObservableCollection<InfoCotizaciones> _listaCotizaciones;
        private InfoCotizaciones _infoCotizacion;
        private DateTime _fechaInicial;
        private DateTime _fechaFinal;
        private String _txtUsuario;
        private Sucursal _miSucursal;
        private String _txtCliente;
        private Cliente _clienteCtz;
        private ObservableCollection<ProductoSeleccionado> _listaProductosCtz;

        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged("VerMensaje"); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged("TxtMensaje"); } }
        public EstatusCotizacion ActualEstatus { get => _actualEstatus; set { _actualEstatus = value; OnPropertyChanged("ActualEstatus"); } }
        public ObservableCollection<EstatusCotizacion> ListaEstatusCtz { get => _listaEstatusCtz; set { _listaEstatusCtz = value; OnPropertyChanged("ListaEstatusCtz"); } }
        public ObservableCollection<Sucursal> ListaSucursales { get => _listaSucursales; set { _listaSucursales = value; OnPropertyChanged("ListaSucursales"); } }
        public DateTime FechaInicial { get => _fechaInicial; set { _fechaInicial = value; OnPropertyChanged("FechaInicial"); } }
        public DateTime FechaFinal { get => _fechaFinal; set { _fechaFinal = value; OnPropertyChanged("FechaFinal"); } }
        public string TxtUsuario { get => _txtUsuario; set { _txtUsuario = value; OnPropertyChanged("TxtUsuario"); } }
        public Sucursal MiSucursal { get => _miSucursal; set { _miSucursal = value; OnPropertyChanged("MiSucursal"); } }
        public string ClaveEF_Empresa { get => _claveEF_Empresa; set { _claveEF_Empresa = value; OnPropertyChanged("ClaveEF_Empresa"); } }
        public ObservableCollection<InfoCotizaciones> ListaCotizaciones { get => _listaCotizaciones; set { _listaCotizaciones = value; OnPropertyChanged("ListaCotizaciones"); } }
        public InfoCotizaciones InfoCotizacion { get => _infoCotizacion; set { _infoCotizacion = value; OnPropertyChanged("InfoCotizacion"); } }
        public string TxtCliente { get => _txtCliente; set { _txtCliente = value; OnPropertyChanged("TxtCliente"); } }
        public Cliente ClienteCtz { get => _clienteCtz; set { _clienteCtz = value; OnPropertyChanged("ClienteCtz"); } }
        public ObservableCollection<ProductoSeleccionado> ListaProductosCtz { get => _listaProductosCtz; set { _listaProductosCtz = value; OnPropertyChanged("ListaProductosCtz"); } }
        #endregion

        #region Constructor
        public BuscadorCotizacionesViewModel()
        {
            BuscarCotizacionesCommand = new RelayCommand(BuscarCotizaciones);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            VerCotizacionCommand = new RelayCommand(VerCotizaciones);
            DateTime hoy = DateTime.Now;
            FechaInicial = new DateTime(hoy.Year, hoy.Month, 1);
            FechaFinal = FechaInicial.AddMonths(1).AddDays(-1);
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
                    ActualEstatus = ListaEstatusCtz.First();
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
                    MiSucursal = ListaSucursales.Where(x=> x.ClaveEntidadFiscalInmueble == Usuario.ClaveEntidadFiscalInmueble).FirstOrDefault();
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
                    if (FechaFinal >= FechaInicial)
                    {
                        var estatus = ActualEstatus.ClaveTipoDeStatusDeComprobante;
                        var rest = new RestClient(Localhost);
                        var req = new RestRequest("mostrarCotizaciones", Method.POST);
                        req.AddHeader("Accept", "application/json");
                        req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                        req.AddParameter("claveEF_Inmueble", MiSucursal.ClaveEntidadFiscalInmueble);
                        req.AddParameter("fechaInicial", FechaInicial.ToString("yyyy-MM-dd"));
                        req.AddParameter("fechaFinal", FechaFinal.ToString("yyyy-MM-dd"));
                        //if(FechaInicial == DateTime.MinValue)
                        req.AddParameter("txtCliente", TxtCliente ?? string.Empty); //reduccion del operador condicional ternario
                        req.AddParameter("claveTipoEstatus", value: (estatus == 0) ? string.Empty : Convert.ToString(estatus));

                        IRestResponse resp = rest.Execute(req);
                        if (resp.IsSuccessful && resp.StatusCode == HttpStatusCode.OK)
                        {
                            List<InfoCotizaciones> lista = JsonConvert.DeserializeObject<List<InfoCotizaciones>>(resp.Content);
                            ListaCotizaciones = new ObservableCollection<InfoCotizaciones>(lista);
                        }
                        if (ListaCotizaciones.Count == 0)
                        {
                            TxtMensaje = "La búsqueda no obtuvo resultados, verifique los filtros por favor.";
                            VerMensaje = true;
                        }
                    }
                    else
                    {
                        TxtMensaje = "La fecha inicial debe ser menor o igual a la fecha final.";
                        VerMensaje = true;
                    }
                }
                catch (Exception)
                {
                    TxtMensaje = "La búsqueda no obtuvo resultados, verifique los filtros por favor.";
                    VerMensaje = true;
                }
            }
        }

        private void VerCotizaciones(object parameter)
        {
            string numCot = parameter as string;
            InfoCotizacion = ListaCotizaciones.Where(x => x.ClaveComprobanteDeCotizacion == numCot).First();
            var vmInicio = new InicioViewModel(AppKey, Usuario, Localhost);
            vmInicio.CargarMenuInicial();
            vmInicio.IdVentana = 0;
            vmInicio.VmCotizador.InfoCotizacion = InfoCotizacion;
            CargarCotizacion();
            vmInicio.VmCotizador.ClienteSel = ClienteCtz;            
            vmInicio.VmCotizador.Observaciones = InfoCotizacion.Observaciones;
            vmInicio.VmCotizador.CteRazonSocial = ClienteCtz.RazonSocial + " | RFC:" + ClienteCtz.Rfc + " | Codigo:" + ClienteCtz.CodigoDeCliente;
            vmInicio.VmCotizador.DatosCliente = "Contacto(s):" + ClienteCtz.Contacto + " | Teléfono(s): " + ClienteCtz.NumeroTelefono + " | Direccion: " + ClienteCtz.Direccion;
            vmInicio.VmCotizador.ActivaFechaCot = false;
            vmInicio.VmCotizador.FechaCotizacion = Convert.ToDateTime(InfoCotizacion.FechaEmision);
            vmInicio.VmCotizador.ListaProductos = ListaProductosCtz;
            vmInicio.VmCotizador.ListaDetalles = new ObservableCollection<ProductoSeleccionado>(ListaProductosCtz);
            vmInicio.VmCotizador.CalcularTotales();

            var vwInicio = new InicioView
            {
                DataContext = vmInicio
            };
            Navigator.NavigationService.Navigate(vwInicio);
        }

        public void CargarCotizacion()
        {
            try
            {
                /// CARGA DE LA CABECERA DE LA COTIZACION
                var rest = new RestClient(Localhost);
                var req = new RestRequest("buscarClientes/" + Usuario.ClaveEntidadFiscalEmpresa + "/" + InfoCotizacion.CodigoDeCliente, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse<ClientesJson> resp = rest.Execute<ClientesJson>(req);
                if (resp.IsSuccessful && resp.StatusCode == HttpStatusCode.OK)
                {
                    var info = JsonConvert.DeserializeObject<ClientesJson>(resp.Content);
                    ClienteCtz = info.Clientes.First();                    
                }

                /// CARGA DE DETALLES DE LA COTIZACION
                req = new RestRequest("cargarDetallesCotizacion/" + InfoCotizacion.ClaveComprobanteDeCotizacion, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse respdet = rest.Execute(req);
                if (respdet.IsSuccessful && respdet.StatusCode == HttpStatusCode.OK)
                {
                    List<InfoDetallesCotizacion> detalles = JsonConvert.DeserializeObject<List<InfoDetallesCotizacion>>(respdet.Content);
                    ListaProductosCtz = new ObservableCollection<ProductoSeleccionado>();
                    foreach (InfoDetallesCotizacion fila in detalles)
                    {
                        double pdesc = Math.Round(fila.ImporteDescuento / fila.PrecioUnitario, 2);
                        ProductoSeleccionado psel = new ProductoSeleccionado
                        {
                            Cantidad = fila.Cantidad,
                            Descuento = pdesc,
                            Importe = fila.Importe,
                            ImporteDesc = fila.ImporteDescuento,
                            Impuesto = fila.Impuestos,
                            SubTotal = fila.Subtotal,
                            Estatus = 3,
                            Producto = new Producto
                            {
                                ClaveProducto = fila.ClaveProducto,
                                Descripcion = fila.Descripcion,
                                PrecioUnitario = fila.PrecioUnitario,
                                SumaImpuestos = fila.SumaImpuestos,
                                CodigoInterno = fila.CodigoInterno,
                                Tasas = fila.Tasas,
                                ClavesImpuestos = fila.ClavesImpuestos                                
                            }
                        };
                        ListaProductosCtz.Add(psel);
                    }
                }
            }
            catch (Exception ex)
            {
                TxtMensaje = "ERROR" + ex.Message;
                VerMensaje = true;
            }
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;
        #endregion
    }
}
