using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Cotizador.ViewModel
{
    public class BuscadorCotizacionesViewModel : Notificador
    {
        #region Commands
        public RelayCommand BuscarCotizacionesCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        public RelayCommand VerCotizacionCommand { get; set; }
        public RelayCommand EnviarCotizacionCommand { get; set; }
        public RelayCommand DescargarCotizacionCommand { get; set; }
        public RelayCommand InicioCommand { get; set; }
        public RelayCommand AnteriorCommand { get; set; }
        public RelayCommand SiguienteCommand { get; set; }
        public RelayCommand FinalCommand { get; set; }
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
        private int _pagsTotales;
        private int _indicePagActual;
        private int _itemsPorPag;
        private int _pagActual;
        private Boolean _activoInicio;
        private Boolean _activoAnterior;
        private Boolean _activoSiguiente;
        private Boolean _activoFinal;
        private CollectionViewSource _cvsCotizaciones;
        private String _correosElectronicos;
        private int _vigenciaEstatus;

        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public EstatusCotizacion ActualEstatus { get => _actualEstatus; set { _actualEstatus = value; OnPropertyChanged(); } }
        public ObservableCollection<EstatusCotizacion> ListaEstatusCtz { get => _listaEstatusCtz; set { _listaEstatusCtz = value; OnPropertyChanged(); } }
        public ObservableCollection<Sucursal> ListaSucursales { get => _listaSucursales; set { _listaSucursales = value; OnPropertyChanged(); } }
        public DateTime FechaInicial { get => _fechaInicial; set { _fechaInicial = value; OnPropertyChanged(); } }
        public DateTime FechaFinal { get => _fechaFinal; set { _fechaFinal = value; OnPropertyChanged(); } }
        public string TxtUsuario { get => _txtUsuario; set { _txtUsuario = value; OnPropertyChanged(); } }
        public Sucursal MiSucursal { get => _miSucursal; set { _miSucursal = value; OnPropertyChanged(); } }
        public string ClaveEF_Empresa { get => _claveEF_Empresa; set { _claveEF_Empresa = value; OnPropertyChanged(); } }
        public ObservableCollection<InfoCotizaciones> ListaCotizaciones { get => _listaCotizaciones; set { _listaCotizaciones = value; OnPropertyChanged(); } }
        public InfoCotizaciones InfoCotizacion { get => _infoCotizacion; set { _infoCotizacion = value; OnPropertyChanged(); } }
        public string TxtCliente { get => _txtCliente; set { _txtCliente = value; OnPropertyChanged(); } }
        public Cliente ClienteCtz { get => _clienteCtz; set { _clienteCtz = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductoSeleccionado> ListaProductosCtz { get => _listaProductosCtz; set { _listaProductosCtz = value; OnPropertyChanged(); } }
        public int PagsTotales { get => _pagsTotales; set { _pagsTotales = value; OnPropertyChanged(); } }
        public int IndicePagActual { get => _indicePagActual; set { _indicePagActual = value; OnPropertyChanged(); } }
        public int ItemsPorPag { get => _itemsPorPag; set { _itemsPorPag = value; OnPropertyChanged(); } }
        public int PagActual { get => _pagActual + 1; set { _pagActual = value; OnPropertyChanged(); } }
        public bool ActivoInicio { get => _activoInicio; set { _activoInicio = value; OnPropertyChanged(); } }
        public bool ActivoAnterior { get => _activoAnterior; set { _activoAnterior = value; OnPropertyChanged(); } }
        public bool ActivoSiguiente { get => _activoSiguiente; set { _activoSiguiente = value; OnPropertyChanged(); } }
        public bool ActivoFinal { get => _activoFinal; set { _activoFinal = value; OnPropertyChanged(); } }
        public CollectionViewSource CvsCotizaciones { get => _cvsCotizaciones; set { _cvsCotizaciones = value; OnPropertyChanged(); } }
        public string CorreosElectronicos { get => _correosElectronicos; set { _correosElectronicos = value; OnPropertyChanged(); } }
        public int VigenciaEstatus { get => _vigenciaEstatus; set { _vigenciaEstatus = value; OnPropertyChanged(); } }

        #endregion

        #region Constructor
        public BuscadorCotizacionesViewModel()
        {
            BuscarCotizacionesCommand = new RelayCommand(BuscarCotizaciones);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            VerCotizacionCommand = new RelayCommand(VerCotizaciones);
            EnviarCotizacionCommand = new RelayCommand(EnviarCotizacion);
            DescargarCotizacionCommand = new RelayCommand(DescargarCotizacion);
            InicioCommand = new RelayCommand(Inicio);
            AnteriorCommand = new RelayCommand(Anterior);
            SiguienteCommand = new RelayCommand(Siguiente);
            FinalCommand = new RelayCommand(Final);
            DateTime hoy = DateTime.Now;
            FechaInicial = new DateTime(hoy.Year, hoy.Month, 1);
            FechaFinal = FechaInicial.AddMonths(1).AddDays(-1);
            IndicePagActual = 0;
            ItemsPorPag = 10;
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
                    ListaEstatusCtz = new ObservableCollection<EstatusCotizacion>(lista.Where(x => x.ClaveTipoDeStatusDeComprobante != 161).ToList());
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

        private int CargarEstatusCotizacion(int claveEstatus)
        {
            int clvstat = 0;
            switch (claveEstatus)
            {
                case 160: clvstat = 0; break;
                //case 161: clvstat = 1; break;
                case 162: clvstat = 1; break;
            }
            return clvstat;
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
                        req.AddParameter("claveEF_Responsable", Usuario.ClaveEntidadFiscalEmpleado);
                        req.AddParameter("fechaInicial", FechaInicial.ToString("yyyy-MM-dd"));
                        req.AddParameter("fechaFinal", FechaFinal.ToString("yyyy-MM-dd"));
                        req.AddParameter("txtCliente", TxtCliente ?? string.Empty); //reduccion del operador condicional ternario
                        req.AddParameter("claveTipoEstatus", value: (estatus == 0) ? string.Empty : Convert.ToString(estatus));

                        IRestResponse resp = rest.Execute(req);
                        if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
                        {
                            List<InfoCotizaciones> lista = JsonConvert.DeserializeObject<List<InfoCotizaciones>>(resp.Content);
                            ListaCotizaciones = new ObservableCollection<InfoCotizaciones>(lista.OrderBy(x => x.CodigoDeComprobante));
                            VerificarVigenciaEstatus();

                            ///Paginacion de los resultados
                            CvsCotizaciones = new CollectionViewSource
                            {
                                Source = ListaCotizaciones
                            };
                            CvsCotizaciones.Filter += new FilterEventHandler(FiltroPaginas);
                            IndicePagActual = 0;
                            CalcularPagsTotales();
                            ActivarBotones();
                        }
                        if (ListaCotizaciones.Count == 0)
                        {
                            TxtMensaje = "La búsqueda no obtuvo resultados, verifique los filtros por favor.";
                            VerMensaje = true;
                        }
                        else
                        {
                            IndicePagActual = 0;
                            ActualizarPagina();
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
                    TxtMensaje = "Ocurrió un error al realizar la consulta, contacte al Administrador.";
                    VerMensaje = true;
                }
            }
        }

        private void VerificarVigenciaEstatus()
        {
            foreach (InfoCotizaciones info in ListaCotizaciones)
            {
                DateTime fvig = Convert.ToDateTime(info.FechaVigencia);
                TimeSpan dif = DateTime.Now.Subtract(fvig);
                int dias = dif.Days;
                if (dias > 0)
                    info.VigenciaEstatus = 1;
                else if (dias >= -7 && dias <= 0)
                    info.VigenciaEstatus = 2;
                else
                    info.VigenciaEstatus = 3;
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
            vmInicio.VmCotizador.FechaCotizacion = Convert.ToDateTime(InfoCotizacion.FechaEmision);
            vmInicio.VmCotizador.ListaProductos = ListaProductosCtz;
            vmInicio.VmCotizador.ChecarFechaEntrega(Convert.ToDateTime(InfoCotizacion.FechaEmision), ListaProductosCtz);
            vmInicio.VmCotizador.ListaDetalles = new ObservableCollection<ProductoSeleccionado>(ListaProductosCtz);
            vmInicio.VmCotizador.CalcularTotales();
            vmInicio.VmCotizador.NumCotizacion = "COTIZACION: " + InfoCotizacion.CodigoDeComprobante;
            vmInicio.VmCotizador.EditaSucursal = InfoCotizacion.ClaveEntidadFiscalInmueble;
            vmInicio.VmCotizador.EditaUsuario = InfoCotizacion.ClaveEntidadFiscalResponsable;
            //vmInicio.VmCotizador.EstatusCotizacion = vmInicio.VmCotizador.ListaEstatusCtz.Single(z => z.Descripcion.Equals(InfoCotizacion.Estatus));
            vmInicio.VmCotizador.IndexEstatusCtz = CargarEstatusCotizacion(InfoCotizacion.ClaveEstatus);

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
                        double pdesc = Math.Round(fila.ImporteDescuento / (fila.PrecioUnitario * fila.Cantidad), 2);
                        DateTime fecEntrega = (fila.DiasDeEntrega == 0) ? Convert.ToDateTime(InfoCotizacion.FechaVigencia) : Convert.ToDateTime(InfoCotizacion.FechaEmision).AddDays(fila.DiasDeEntrega);
                        ProductoSeleccionado psel = new ProductoSeleccionado
                        {
                            Cantidad = fila.Cantidad,
                            Descuento = pdesc,
                            Importe = fila.Importe,
                            ImporteDesc = fila.ImporteDescuento,
                            Impuesto = fila.Impuestos,
                            SubTotal = fila.Subtotal,
                            Estatus = 3,
                            ClaveDetalleDeComprobante = fila.ClaveDetalleDeComprobante,
                            DiasEntrega = fila.DiasDeEntrega,
                            FechaEntrega = fecEntrega,
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

        private async void EnviarCotizacion(object parameter)
        {
            string numCot = parameter as string;
            InfoCotizacion = ListaCotizaciones.Where(x => x.ClaveComprobanteDeCotizacion == numCot).First();
            CorreosElectronicos = InfoCotizacion.CorreoElectronico;
            var vmEnviarCtz = new EnviarCotizacionViewModel
            {
                NumCotizacion = InfoCotizacion.CodigoDeComprobante,
                CorreosElectronicos = CorreosElectronicos
            };
            var vwEnviarCtz = new EnviarCotizacionView
            {
                DataContext = vmEnviarCtz
            };
            var result = await DialogHost.Show(vwEnviarCtz, "BuscadorCotizacionesView");
            if (result.Equals("ENVIAR"))
            {                
                CorreosElectronicos = vmEnviarCtz.CorreosElectronicos;
                bool existeError = ValidarCorreo();
                if(existeError == true)
                {
                    TxtMensaje = "La cotización no puede ser enviada hasta que las direcciones de email estén escritas de manera correcta y no existan espacios en blanco.";
                    VerMensaje = true;
                }
                else
                {
                    string prmCotizacion = InfoCotizacion.CodigoDeComprobante;
                    string prmEmails = String.Join(",", Regex.Split(CorreosElectronicos, @"\r\n"));
                    var rest = new RestClient(Localhost);
                    var req = new RestRequest("enviarMail", Method.POST);
                    req.AddHeader("Accept", "application/json");
                    req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                    req.AddParameter("claveComprobante", prmCotizacion);
                    req.AddParameter("emails", prmEmails);
                    req.AddParameter("claveEF_Empresa", Usuario.ClaveEntidadFiscalEmpresa);

                    IRestResponse response = rest.Execute(req);
                    if(response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                    {
                        TxtMensaje = "La cotización fue enviada correctamente";
                        VerMensaje = true;
                    }
                    else
                    {
                        TxtMensaje = "Hubo un error al enviar la cotizacion: " + response.Content;
                        VerMensaje = true;
                    }
                }
            }
        }

        private void DescargarCotizacion(object parameter)
        {
            try
            {
                string numCot = parameter as string;
                InfoCotizacion = ListaCotizaciones.Where(x => x.ClaveComprobanteDeCotizacion == numCot).First();
                var rest = new RestClient(Localhost);
                var req = new RestRequest("descargarPDF/" + Usuario.ClaveEntidadFiscalEmpresa + "/" + InfoCotizacion.CodigoDeComprobante, Method.GET);
                req.AddHeader("Content-Type", "application/pdf");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                byte[] archivo = rest.DownloadData(req);
                File.WriteAllBytes(Path.GetTempPath() + InfoCotizacion.CodigoDeComprobante + ".pdf", archivo);
                System.Diagnostics.Process.Start(Path.GetTempPath() + InfoCotizacion.CodigoDeComprobante + ".pdf");
            }
            catch (Exception e)
            {
                TxtMensaje = "Error al descargar: " + e.Message;
                VerMensaje = true;
            }
        }

        private bool ValidarCorreo()
        {
            bool error = false;
            if (string.IsNullOrEmpty(CorreosElectronicos) == true || string.IsNullOrWhiteSpace(CorreosElectronicos) == true)
                error = true;
            else
            {
                string[] correos = Regex.Split(CorreosElectronicos, @"\r\n");
                foreach (string cad in correos)
                {

                    error = !Regex.IsMatch(cad, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
                    if (error == true)
                        break;
                }
            }
            return error;
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;

        private void Inicio(object obj)
        {
            IndicePagActual = 0;
            ActualizarPagina();
        }

        private void Anterior(object obj)
        {
            IndicePagActual -= 1;
            ActualizarPagina();
        }

        private void Siguiente(object obj)
        {
            IndicePagActual += 1;
            ActualizarPagina();
        }

        private void Final(object obj)
        {
            IndicePagActual = PagsTotales - 1;
            ActualizarPagina();
        }

        private void ActualizarPagina()
        {
            PagActual = IndicePagActual;
            CvsCotizaciones.View.Refresh();
            ActivarBotones();
        }

        private void CalcularPagsTotales()
        {
            if (ListaCotizaciones.Count % ItemsPorPag == 0)
            {
                PagsTotales = ListaCotizaciones.Count / ItemsPorPag;
            }
            else
            {
                PagsTotales = (ListaCotizaciones.Count / ItemsPorPag) + 1;
            }
        }

        private void FiltroPaginas(object sender, FilterEventArgs e)
        {
            int index = ListaCotizaciones.IndexOf((InfoCotizaciones)e.Item);
            if (index >= ItemsPorPag * IndicePagActual && index < ItemsPorPag * (IndicePagActual + 1))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void ActivarBotones()
        {
            if (ListaCotizaciones.Count > 0)
            {
                ActivoInicio = (IndicePagActual != 0) ? true : false;
                ActivoAnterior = (IndicePagActual != 0) ? true : false;
                ActivoSiguiente = (IndicePagActual < PagsTotales - 1) ? true : false;
                ActivoFinal = (PagActual != PagsTotales) ? true : false;
            }
        }
        #endregion
    }
}
