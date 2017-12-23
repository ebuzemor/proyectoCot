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
    public class CotizadorViewModel : Notificador
    {
        #region Commands
        public RelayCommand AgregarClienteCommand { get; set; }
        public RelayCommand AgregarProductoCommand { get; set; }
        public RelayCommand EditarProductoCommand { get; set; }
        public RelayCommand QuitarProductoCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        public RelayCommand GuardarCtzCommand { get; set; }
        public RelayCommand CancelarCtzCommand { get; set; }
        #endregion

        #region Variables
        private Cliente _clienteSel;
        private ObservableCollection<ProductoSeleccionado> _listaProductos;
        private ObservableCollection<ProductoSeleccionado> _listaDetalles;
        private ObservableCollection<EstatusCotizacion> _listaEstatusCtz;
        private String _datosCliente;
        private String _cteRazonSocial;
        private ApiKey _appKey;
        private Usuario _usuario;
        private ProductoSeleccionado _productoSel;
        private String _localhost;
        private Boolean _esImportado;
        private Double _precioUniTotal;
        private Double _cantidadTotal;
        private Double _descuentoTotal;
        private Double _importeTotal;
        private Double _impuestoTotal;
        private Double _sumaSubTotal;
        private Boolean _verMensaje;
        private String _txtMensaje;
        private DateTime _fechaCotizacion;
        private DateTime _fechaVigencia;
        private DateTime _fechaEntrega;
        private Sucursal _sucursalSel;
        private String _txtSucursal;
        private EstatusCotizacion _estatusCotizacion;
        private String _observaciones;
        private CondicionesComerciales _condiciones;
        private Cotizacion _miCotizacion;
        private InfoCotizaciones _infoCotizacion;
        private List<ComprobantesImpuestos> _listaCotizacionImpuestos;
        private List<ComprobantesImpuestos> _listaImpuestosXlinea;
        private List<DetalleComprobantes> _listaDetalleComprobantes;
        private Boolean _activaFechaCot;
        private String _numCotizacion;
        private int _indexEstatusCtz;
        private long _claveEstatusCtz;
        private Boolean _borradorSeleccionado;
        private Boolean _pendienteSeleccionado;
        private Boolean _definitivaSeleccionado;
        private Boolean _aceptaCambios;
        private Boolean _aceptaCambiosCliente;

        public Cliente ClienteSel { get => _clienteSel; set { _clienteSel = value; OnPropertyChanged("NvoCliente"); } }
        public string DatosCliente { get => _datosCliente;  set { _datosCliente = value; OnPropertyChanged("DatosCliente"); } }
        public string CteRazonSocial { get => _cteRazonSocial; set { _cteRazonSocial = value; OnPropertyChanged("CteRazonSocial"); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }        
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        public ProductoSeleccionado ProductoSel { get => _productoSel; set { _productoSel = value; OnPropertyChanged("ProductoSel"); } }
        public ObservableCollection<ProductoSeleccionado> ListaProductos { get => _listaProductos; set { _listaProductos = value; OnPropertyChanged("ListaProductos"); } }
        public double PrecioUniTotal { get => _precioUniTotal; set { _precioUniTotal = value; OnPropertyChanged("PrecioUniTotal"); } }
        public double CantidadTotal { get => _cantidadTotal; set { _cantidadTotal = value;  OnPropertyChanged("CantidadTotal"); } }
        public double DescuentoTotal { get => _descuentoTotal; set { _descuentoTotal = value; OnPropertyChanged("DescuentoTotal"); } }
        public double ImporteTotal { get => _importeTotal; set { _importeTotal = value; OnPropertyChanged("ImporteTotal"); } }
        public double ImpuestoTotal { get => _impuestoTotal; set { _impuestoTotal = value; OnPropertyChanged("ImpuestoTotal"); } }
        public double SumaSubTotal { get => _sumaSubTotal; set { _sumaSubTotal = value; OnPropertyChanged("SumaSubTotal"); } }
        public bool EsImportado { get => _esImportado; set { _esImportado = value; OnPropertyChanged("EsImportado"); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged("VerMensaje"); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged("TxtMensaje"); } }
        public DateTime FechaCotizacion { get => _fechaCotizacion; set { _fechaCotizacion = value; OnPropertyChanged("FechaCotizacion");  ChecarVigencia(_fechaCotizacion); ChecarFechaEntrega(); } }
        public DateTime FechaVigencia { get => _fechaVigencia; set { _fechaVigencia = value; OnPropertyChanged("FechaVigencia"); } }
        public DateTime FechaEntrega { get => _fechaEntrega; set { _fechaEntrega = value; OnPropertyChanged("FechaEntrega"); } }
        public Sucursal SucursalSel { get => _sucursalSel; set { _sucursalSel = value; OnPropertyChanged("SucursalSel"); } }
        public string TxtSucursal { get => _txtSucursal; set { _txtSucursal = value; OnPropertyChanged("TxtSucursal"); } }
        public ObservableCollection<EstatusCotizacion> ListaEstatusCtz { get => _listaEstatusCtz; set { _listaEstatusCtz = value; OnPropertyChanged("ListaEstatusCtz"); } }
        public EstatusCotizacion EstatusCotizacion { get => _estatusCotizacion; set { _estatusCotizacion = value; OnPropertyChanged("EstatusCotizacion"); } }
        public string Observaciones { get => _observaciones; set { _observaciones = value; OnPropertyChanged("Observaciones"); } }
        public CondicionesComerciales Condiciones { get => _condiciones; set { _condiciones = value; OnPropertyChanged("Condiciones"); } }
        public Cotizacion MiCotizacion { get => _miCotizacion; set { _miCotizacion = value; OnPropertyChanged("MiCotizacion"); } }
        public List<ComprobantesImpuestos> ListaCotizacionImpuestos { get => _listaCotizacionImpuestos; set => _listaCotizacionImpuestos = value; }
        public List<ComprobantesImpuestos> ListaImpuestosXlinea { get => _listaImpuestosXlinea; set => _listaImpuestosXlinea = value; }
        public List<DetalleComprobantes> ListaDetalleComprobantes { get => _listaDetalleComprobantes; set => _listaDetalleComprobantes = value; }
        public InfoCotizaciones InfoCotizacion { get => _infoCotizacion; set { _infoCotizacion = value; OnPropertyChanged("InfoCotizacion"); } }
        public bool ActivaFechaCot { get => _activaFechaCot; set { _activaFechaCot = value; OnPropertyChanged("ActivaFechaCot"); } }
        public ObservableCollection<ProductoSeleccionado> ListaDetalles { get => _listaDetalles; set { _listaDetalles = value; OnPropertyChanged("ListaDetalles"); } }
        public string NumCotizacion { get => _numCotizacion; set { _numCotizacion = value; OnPropertyChanged("NumCotizacion"); } }
        public int IndexEstatusCtz { get => _indexEstatusCtz; set { _indexEstatusCtz = value; OnPropertyChanged("IndexEstatusCtz"); ActualizarEstatus(); } }
        public long ClaveEstatusCtz { get => _claveEstatusCtz; set { _claveEstatusCtz = value; OnPropertyChanged("ClaveEstatusCtz"); } }
        public bool BorradorSeleccionado { get => _borradorSeleccionado; set { _borradorSeleccionado = value; OnPropertyChanged("BorradorSeleccionado"); } }
        public bool PendienteSeleccionado { get => _pendienteSeleccionado; set { _pendienteSeleccionado = value; OnPropertyChanged("PendienteSeleccionado"); } }
        public bool DefinitivaSeleccionado { get => _definitivaSeleccionado; set { _definitivaSeleccionado = value; OnPropertyChanged("DefinitivaSeleccionado"); } }
        public bool AceptaCambiosCtz { get => _aceptaCambios; set { _aceptaCambios = value; OnPropertyChanged("AceptaCambiosCtz"); } }
        public bool AceptaCambiosCliente { get => _aceptaCambiosCliente; set { _aceptaCambiosCliente = value; OnPropertyChanged("AceptaCambiosCliente"); } }
        #endregion

        #region Constructor
        public CotizadorViewModel()
        {
            AgregarClienteCommand = new RelayCommand(AgregarCliente);
            AgregarProductoCommand = new RelayCommand(AgregarProducto);
            EditarProductoCommand = new RelayCommand(EditarProducto);
            QuitarProductoCommand = new RelayCommand(QuitarProducto);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            GuardarCtzCommand = new RelayCommand(GuardarCotizacion);
            CancelarCtzCommand = new RelayCommand(CancelarCotizacion);
            ListaProductos = new ObservableCollection<ProductoSeleccionado>();
            FechaCotizacion = DateTime.Now;
            ActivaFechaCot = true;
            ListaDetalles = new ObservableCollection<ProductoSeleccionado>();
            BorradorSeleccionado = true;
            AceptaCambiosCtz = true;
            AceptaCambiosCliente = true;
        }        
        #endregion

        #region Métodos
        private async void AgregarCliente(object parameter)
        {
            try
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
                var result = await DialogHost.Show(vwBuscarCliente, "CotizadorView", ClosingEventHandler);
                if (result.Equals("NvoCliente") == true)
                {
                    ClienteSel = vmBuscarCliente.NvoCliente;
                    CteRazonSocial = ClienteSel.RazonSocial + " | RFC:" + ClienteSel.Rfc + " | Codigo:" + ClienteSel.CodigoDeCliente;
                    DatosCliente = "Contacto(s):" + ClienteSel.Contacto + " | Teléfono(s): " + ClienteSel.NumeroTelefono + " | Direccion: " + ClienteSel.Direccion;
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void AgregarProducto(object parameter)
        {
            try
            {
                if (ClienteSel != null)
                {
                    var vmBuscarProducto = new BuscarProductosViewModel
                    {
                        AppKey = AppKey,
                        Usuario = Usuario,
                        Localhost = Localhost
                    };
                    var vwBuscarProducto = new BuscarProductosView
                    {
                        DataContext = vmBuscarProducto
                    };
                    var result = await DialogHost.Show(vwBuscarProducto, "CotizadorView", ClosingEventHandler);
                    if (result.Equals("SelProducto") == true)
                    {
                        ProductoSel = vmBuscarProducto.SelProducto;
                        ProductoSeleccionado ps = ListaProductos.SingleOrDefault(x => x.Producto.ClaveProducto == ProductoSel.Producto.ClaveProducto);
                        if (ps == null)
                        {
                            ListaProductos.Add(ProductoSel);
                            ActualizarListaDetalles(ProductoSel, 2);
                            CalcularTotales();
                        }
                        else
                        {
                            TxtMensaje = "El producto seleccionado ya fue agregado, proceda a editar la cantidad y descuento en la partida correspondiente";
                            VerMensaje = true;
                        }
                        ChecarFechaEntrega();
                    }
                }
                else
                {
                    var vmMensaje = new MensajeViewModel
                    {
                        TituloMensaje = "Advertencia",
                        CuerpoMensaje = "Debe seleccionar a un Cliente para poder realizar una Cotización"
                    };
                    var vwMensaje = new MensajeView
                    {
                        DataContext = vmMensaje
                    };
                    var result = await DialogHost.Show(vwMensaje, "CotizadorView");
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void EditarProducto(object parameter)
        {
            Int64 clvProducto = Convert.IsDBNull(parameter) ? 0 : Convert.ToInt64(parameter);
            ProductoSeleccionado producto = ListaProductos.Single(x => x.Producto.ClaveProducto == clvProducto);
            var vmEditarItem = new EditarItemViewModel
            {
                ProdSeleccionado = producto,
                TxtCantidad = producto.Cantidad,
                TxtDescuento = producto.Descuento,
                TxtImporte = producto.Importe,
                TxtImporteDesc = producto.ImporteDesc
            };
            var vwEditarItem = new EditarItemView
            {
                DataContext = vmEditarItem
            };
            var result = await DialogHost.Show(vwEditarItem, "CotizadorView", ClosingEventHandler);
            if (result.Equals("OK") == true)
            {
                vmEditarItem.ActualizarProducto();
                producto = vmEditarItem.ProdSeleccionado;
                ActualizarListaDetalles(producto, 1);
                CalcularTotales();
            }
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            var x = eventArgs.Parameter;
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private void QuitarProducto(object parameter)
        {
            Int64 clvProducto = Convert.IsDBNull(parameter) ? 0 : Convert.ToInt64(parameter);
            ProductoSeleccionado producto = ListaProductos.Single(x => x.Producto.ClaveProducto == clvProducto);
            ListaProductos.Remove(producto);
            ActualizarListaDetalles(producto, 0);
            CalcularTotales();
            ChecarFechaEntrega();
        }

        public void CalcularTotales()
        {
            PrecioUniTotal = 0.0;
            CantidadTotal = 0.0;
            DescuentoTotal = 0.0;
            ImporteTotal = 0.0;
            ImpuestoTotal = 0.0;
            SumaSubTotal = 0.0;
            foreach (ProductoSeleccionado p in ListaProductos)
            {
                PrecioUniTotal += p.Producto.PrecioUnitario;
                CantidadTotal += p.Cantidad;
                DescuentoTotal += p.ImporteDesc;
                ImporteTotal += p.Importe;
                ImpuestoTotal += p.Impuesto;
                SumaSubTotal += p.SubTotal;
            }
            PrecioUniTotal = Math.Round(PrecioUniTotal, 2);
            CantidadTotal = Math.Round(CantidadTotal, 2);
            DescuentoTotal = Math.Round(DescuentoTotal, 2);
            ImporteTotal = Math.Round(ImporteTotal, 2);
            ImpuestoTotal = Math.Round(ImpuestoTotal, 2);
            SumaSubTotal = Math.Round(SumaSubTotal, 2);
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;

        private void ChecarVigencia(DateTime fecha)
        {
            FechaVigencia = FechaCotizacion.AddDays(15);
            ChecarFechaEntrega();
        }

        private void ChecarFechaEntrega()
        {
            foreach(ProductoSeleccionado p in ListaProductos)
            {
                EsImportado = false;
                if(p.Producto.EsImportado == 1)
                {
                    EsImportado = true;
                    break;
                }
            }
            if (EsImportado == false)
            {
                FechaEntrega = FechaVigencia.AddDays(6);
            }
            else
            {
                FechaEntrega = FechaVigencia.AddDays(45);
            }
        }

        private async void GuardarCotizacion(object parameter)
        {
            if (InfoCotizacion != null)
            {
                EditarCotizacion();
            }
            else
            {
                if (ClienteSel != null)
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    string direccionIP = host.AddressList.FirstOrDefault(h => h.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    ListaDetalleComprobantes = new List<DetalleComprobantes>();
                    ComprobantesImpuestos ImpuestoLinea = new ComprobantesImpuestos();
                    ListaCotizacionImpuestos = new List<ComprobantesImpuestos>();
                    ListaImpuestosXlinea = new List<ComprobantesImpuestos>();
                    int c = 1;

                    foreach (ProductoSeleccionado item in ListaProductos)
                    {
                        string[] cadTasas = item.Producto.Tasas.Split(',');
                        string[] cadClave = item.Producto.ClavesImpuestos.Split(',');

                        for (int i = 0; i < cadTasas.Length; i++)
                        {
                            ImpuestoLinea = new ComprobantesImpuestos
                            {
                                ClaveImpuesto = Convert.ToInt64(cadClave[i]),
                                Importe = Math.Round((item.Cantidad * item.Producto.PrecioUnitario - item.ImporteDesc) * (Convert.ToDouble(cadTasas[i]) / 100), 2)
                            };
                            ListaImpuestosXlinea.Add(ImpuestoLinea);
                            ListaCotizacionImpuestos.Add(ImpuestoLinea);
                        }

                        DetalleComprobantes detCom = new DetalleComprobantes
                        {
                            Cantidad = item.Cantidad,
                            ClaveUnidadDeMedida = item.Producto.ClaveUnidadDeMedida,
                            ClaveProducto = item.Producto.ClaveProducto,
                            Importe = item.Importe,
                            ImporteDescuento = item.ImporteDesc,
                            Impuestos = JsonConvert.SerializeObject(ListaImpuestosXlinea),
                            NumeroPartidas = c,
                            PrecioUnitario = item.Producto.PrecioUnitario
                        };
                        c += 1;
                        ListaDetalleComprobantes.Add(detCom);
                        ListaImpuestosXlinea.Clear();
                    }
                    // utilizando Linq, se hace una sumatoria por tipo de impuesto y se guarda en una variable
                    var listaSumaImpuestos = ListaCotizacionImpuestos.GroupBy(x => x.ClaveImpuesto)
                                          .Select(y => new
                                          {
                                              ClaveImpuesto = y.Key,
                                              Importe = Math.Round(y.Sum(z => z.Importe), 2)
                                          });

                    MiCotizacion = new Cotizacion
                    {
                        Empresa = Usuario.Empresa,
                        Equipo = direccionIP,
                        Usuario = Usuario.NombreUsuario,
                        ClaveInmueble = Usuario.Sucursal,
                        ClaveTipoDeComprobante = EstatusCotizacion.ClaveTipoDeComprobante,
                        FechaEmision = FechaCotizacion.ToString("yyyy-MM-dd HH:mm:ss"),
                        Partidas = ListaProductos.Count,
                        ClaveMoneda = 1,
                        ClaveEntidadFiscalInmueble = Usuario.ClaveEntidadFiscalInmueble,
                        ClaveTipoEstatusRecepcion = EstatusCotizacion.ClaveTipoDeStatusDeComprobante,
                        ClaveEntidadFiscalResponsable = Usuario.ClaveEntidadFiscalEmpleado,
                        ListaComprobantesImpuestos = JsonConvert.SerializeObject(listaSumaImpuestos),
                        ClaveEntidadFiscalCliente = ClienteSel.ClaveEntidadFiscalCliente,
                        ClaveListaDePrecios = 1,
                        FechaVigencia = FechaVigencia.ToString("yyyy-MM-dd HH:mm:ss"),
                        SubTotal = ImporteTotal,
                        Descuento = DescuentoTotal,
                        Impuestos = ImpuestoTotal,
                        Total = SumaSubTotal,
                        Observaciones = Observaciones,
                        DetallesDeComprobante = JsonConvert.SerializeObject(ListaDetalleComprobantes)
                    };
                    String AprosiCtz = JsonConvert.SerializeObject(MiCotizacion);
                    
                    //se procede a guardar la cotizacion
                    var rest = new RestClient(Localhost);
                    var req = new RestRequest("guardarCotizacion", Method.POST);
                    req.AddHeader("Accept", "application/json");
                    req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                    req.AddParameter("text/json", AprosiCtz, ParameterType.RequestBody);
                    req.RequestFormat = DataFormat.Json;

                    IRestResponse response = rest.Execute(req);
                    if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                    {
                        List<ComprobanteGenerado> compgen = JsonConvert.DeserializeObject<List<ComprobanteGenerado>>(response.Content);
                        var vmMensaje = new MensajeViewModel
                        {
                            TituloMensaje = "Aviso",
                            MostrarCancelar = false,
                            CuerpoMensaje = "La cotizacion " + compgen.First().FolioCodigoComprobante + " fue generada de manera exitosa."
                        };
                        var vwMensaje = new MensajeView
                        {
                            DataContext = vmMensaje
                        };
                        var result = await DialogHost.Show(vwMensaje, "CotizadorView");
                        LimpiarCotizacion();
                    }
                }
            }
        }

        private async void EditarCotizacion()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string direccionIP = host.AddressList.FirstOrDefault(h => h.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            ListaDetalleComprobantes = new List<DetalleComprobantes>();
            ComprobantesImpuestos ImpuestoLinea = new ComprobantesImpuestos();
            ListaCotizacionImpuestos = new List<ComprobantesImpuestos>();
            ListaImpuestosXlinea = new List<ComprobantesImpuestos>();
            int c = 1;

            foreach (ProductoSeleccionado item in ListaDetalles)
            {
                string[] cadTasas = item.Producto.Tasas.Split(',');
                string[] cadClave = item.Producto.ClavesImpuestos.Split(',');

                for (int i = 0; i < cadTasas.Length; i++)
                {
                    if (item.Estatus != 0)
                    {
                        ImpuestoLinea = new ComprobantesImpuestos
                        {
                            ClaveImpuesto = Convert.ToInt64(cadClave[i]),
                            Importe = Math.Round((item.Cantidad * item.Producto.PrecioUnitario - item.ImporteDesc) * (Convert.ToDouble(cadTasas[i]) / 100), 2)
                        };
                        ListaImpuestosXlinea.Add(ImpuestoLinea);
                        ListaCotizacionImpuestos.Add(ImpuestoLinea);
                    }                    
                }

                DetalleComprobantes detCom = new DetalleComprobantes
                {
                    Cantidad = item.Cantidad,
                    ClaveUnidadDeMedida = item.Producto.ClaveUnidadDeMedida,
                    ClaveProducto = item.Producto.ClaveProducto,
                    Importe = item.Importe,
                    ImporteDescuento = item.ImporteDesc,
                    Impuestos = JsonConvert.SerializeObject(ListaImpuestosXlinea),
                    NumeroPartidas = c,
                    PrecioUnitario = item.Producto.PrecioUnitario,
                    Estatus = item.Estatus,
                    ClaveDetalleDeComprobante = item.ClaveDetalleDeComprobante
                };
                c += 1;
                ListaDetalleComprobantes.Add(detCom);
                ListaImpuestosXlinea.Clear();
            }
            // utilizando Linq, se hace una sumatoria por tipo de impuesto y se guarda en una variable
            var listaSumaImpuestos = ListaCotizacionImpuestos.GroupBy(x => x.ClaveImpuesto)
                                  .Select(y => new
                                  {
                                      ClaveImpuesto = y.Key,
                                      Importe = Math.Round(y.Sum(z => z.Importe), 2)
                                  });
            MiCotizacion = new Cotizacion
            {
                Empresa = Usuario.Empresa,
                Equipo = direccionIP,
                Usuario = Usuario.NombreUsuario,
                ClaveInmueble = Usuario.Sucursal,
                ClaveTipoDeComprobante = EstatusCotizacion.ClaveTipoDeComprobante,
                FechaEmision = FechaCotizacion.ToString("yyyy-MM-dd HH:mm:ss"),
                Partidas = ListaProductos.Count,
                ClaveMoneda = 1,
                ClaveEntidadFiscalInmueble = Usuario.ClaveEntidadFiscalInmueble,
                ClaveTipoEstatusRecepcion = EstatusCotizacion.ClaveTipoDeStatusDeComprobante,
                ClaveEntidadFiscalResponsable = Usuario.ClaveEntidadFiscalEmpleado,
                ClaveComprobante = InfoCotizacion.ClaveComprobanteDeCotizacion,
                CodigoDeComprobante = InfoCotizacion.CodigoDeComprobante,
                ListaComprobantesImpuestos = JsonConvert.SerializeObject(listaSumaImpuestos),
                ClaveEntidadFiscalCliente = ClienteSel.ClaveEntidadFiscalCliente,
                ClaveListaDePrecios = 1,
                FechaVigencia = FechaVigencia.ToString("yyyy-MM-dd HH:mm:ss"),
                SubTotal = ImporteTotal,
                Descuento = DescuentoTotal,
                Impuestos = ImpuestoTotal,
                Total = SumaSubTotal,
                Observaciones = Observaciones,
                DetallesDeComprobante = JsonConvert.SerializeObject(ListaDetalleComprobantes)
            };
            String AprosiCtz = JsonConvert.SerializeObject(MiCotizacion);

            // se procede a editar la cotizacion
            var rest = new RestClient(Localhost);
            var req = new RestRequest("editarCotizacion", Method.POST);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);
            req.AddParameter("text/json", AprosiCtz, ParameterType.RequestBody);

            req.RequestFormat = DataFormat.Json;

            IRestResponse response = rest.Execute(req);
            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var respuesta = JsonConvert.DeserializeObject(response.Content);
                var vmMensaje = new MensajeViewModel
                {
                    TituloMensaje = "Aviso",
                    MostrarCancelar = false,
                    CuerpoMensaje = "Se guardaron los cambios correctamente en la cotizacion: " + InfoCotizacion.CodigoDeComprobante
                };
                var vwMensaje = new MensajeView
                {
                    DataContext = vmMensaje
                };
                var result = await DialogHost.Show(vwMensaje, "CotizadorView");
                LimpiarCotizacion();
            }
        }

        private async void CancelarCotizacion(object parameter)
        {
            if (ClienteSel != null)
            {
                var vmMensaje = new MensajeViewModel
                {
                    TituloMensaje = "Advertencia",
                    MostrarCancelar = true,
                    CuerpoMensaje = "¿Desea crear una nueva cotización?"
                };
                var vwMensaje = new MensajeView
                {
                    DataContext = vmMensaje
                };
                var result = await DialogHost.Show(vwMensaje, "CotizadorView");
                if (result.Equals("ACEPTAR") == true)
                {
                    LimpiarCotizacion();
                }
            }
        }

        private void LimpiarCotizacion()
        {
            ListaProductos.Clear();
            ClienteSel = null;
            FechaCotizacion = DateTime.Now;
            EstatusCotizacion = ListaEstatusCtz.First();
            CteRazonSocial = string.Empty;
            DatosCliente = string.Empty;
            CantidadTotal = 0;
            DescuentoTotal = 0;
            ImporteTotal = 0;
            ImpuestoTotal = 0;
            PrecioUniTotal = 0;
            SumaSubTotal = 0;
            ActivaFechaCot = true;
            Observaciones = null;
            NumCotizacion = null;
            AceptaCambiosCtz = true;
            AceptaCambiosCliente = true;            
            InfoCotizacion = null;
            IndexEstatusCtz = 0;
        }

        private void ActualizarListaDetalles(ProductoSeleccionado prodsel, int estatus)
        {
            switch (estatus)
            {
                case 2:
                    prodsel.Estatus = estatus;
                    ListaDetalles.Add(prodsel);
                    break;
                default:
                    {
                        var prod = ListaDetalles.First(x => x.Producto.ClaveProducto == prodsel.Producto.ClaveProducto);
                        prod.Estatus = estatus;
                        break;
                    }
            }
        }

        public void MostrarSucursal()
        {
            try
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("obtenerSucursal/" + Usuario.ClaveEntidadFiscalInmueble, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse resp = rest.Execute(req);
                if (resp.IsSuccessful && resp.StatusCode == HttpStatusCode.OK)
                {
                    SucursalesJson lista = JsonConvert.DeserializeObject<SucursalesJson>(resp.Content);
                    SucursalSel = lista.Sucursales.First();
                    TxtSucursal = "Sucursal: " + SucursalSel.CodigoDeInmueble + " | " + SucursalSel.NombreCorto;
                }
            }
            catch (Exception)
            {
                TxtMensaje = "Error al cargar sucursales, verifique que el servidor esté activo";
                VerMensaje = true;
            }
        }

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
                    EstatusCotizacion = ListaEstatusCtz.First();
                }
            }
            catch (Exception)
            {
                TxtMensaje = "Error al cargar estatus de la cotización, verifique que el servidor esté activo";
                VerMensaje = true;
            }
        }

        public void MostrarCondicionesComerciales()
        {
            try
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("mostrarCondComCtz/" + Usuario.ClaveEntidadFiscalEmpresa, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse resp = rest.Execute(req);
                if (resp.IsSuccessful && resp.StatusCode == HttpStatusCode.OK)
                {
                    CondicionesComerciales lista = JsonConvert.DeserializeObject<CondicionesComerciales>(resp.Content);                    
                    Condiciones = lista;
                }
            }
            catch (Exception ex)
            {
                TxtMensaje = "Excepción: " + ex.Message;
                VerMensaje = true;
            }
        }       

        public void ActualizarEstatus()
        {
            try
            {
                switch (IndexEstatusCtz)
                {
                    case 0:
                        BorradorSeleccionado = true;
                        PendienteSeleccionado = false;
                        DefinitivaSeleccionado = false;
                        ClaveEstatusCtz = 160;
                        AceptaCambiosCtz = true;
                        AceptaCambiosCliente = (InfoCotizacion != null) ? false : true;
                        break;
                    case 1:
                        if (ListaProductos.Count > 0 && string.IsNullOrEmpty(NumCotizacion) != true)
                        {
                            BorradorSeleccionado = false;
                            PendienteSeleccionado = true;
                            DefinitivaSeleccionado = false;
                            ClaveEstatusCtz = 161;
                            AceptaCambiosCtz = true;
                            AceptaCambiosCliente = false;
                        }
                        else
                            IndexEstatusCtz = 0;
                        break;
                    case 2:
                        if (ListaProductos.Count > 0 && string.IsNullOrEmpty(NumCotizacion) != true)
                        {
                            BorradorSeleccionado = false;
                            PendienteSeleccionado = false;
                            DefinitivaSeleccionado = true;
                            ClaveEstatusCtz = 162;
                            EstatusDefinitiva();
                            AceptaCambiosCtz = false;
                            AceptaCambiosCliente = false;
                        }
                        else
                            IndexEstatusCtz = 0;
                        break;
                    default:
                        break;
                }
                EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == ClaveEstatusCtz);
            }
            catch (Exception) { }
        }

        private async void EstatusDefinitiva()
        {
            try
            {
                var vmMensaje = new MensajeViewModel
                {
                    TituloMensaje = "Advertencia",
                    CuerpoMensaje = "El cambio de estatus de la Cotización a Definitiva es irreversible, ¿Desea cambiar el estatus?",
                    MostrarCancelar = true
                };
                var vwMensaje = new MensajeView
                {
                    DataContext = vmMensaje
                };
                var result = await DialogHost.Show(vwMensaje, "CotizadorView");
                if (result.Equals("ACEPTAR"))
                {
                    GuardarCotizacion(ClienteSel);
                }
                else
                {
                    IndexEstatusCtz = 1;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
