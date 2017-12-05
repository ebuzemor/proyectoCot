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
        private ObservableCollection<Cliente> _listaClientes;
        private ObservableCollection<ProductoSeleccionado> _listaProductos;
        private ObservableCollection<EstatusCotizacion> _listaEstatusCtz;
        private String _datosCliente;
        private String _cteRazonSocial;
        private ApiKey _appKey;
        private Usuario _usuario;
        private ProductoSeleccionado _productoSel;
        private String _localhost;
        private Boolean _esImportado;
        private Boolean _cambiarEstatusCtz;
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

        public ObservableCollection<Cliente> ListaClientes { get => _listaClientes; set { _listaClientes = value; OnPropertyChanged("ListaClientes"); } }
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
        public DateTime FechaCotizacion { get => _fechaCotizacion; set { _fechaCotizacion = value; OnPropertyChanged("Fecha");  ChecarVigencia(_fechaCotizacion); ChecarFechaEntrega(); } }
        public DateTime FechaVigencia { get => _fechaVigencia; set { _fechaVigencia = value; OnPropertyChanged("FechaVigencia"); } }
        public DateTime FechaEntrega { get => _fechaEntrega; set { _fechaEntrega = value; OnPropertyChanged("FechaEntrega"); } }
        public Sucursal SucursalSel { get => _sucursalSel; set { _sucursalSel = value; OnPropertyChanged("SucursalSel"); } }
        public string TxtSucursal { get => _txtSucursal; set { _txtSucursal = value; OnPropertyChanged("TxtSucursal"); } }
        public bool CambiarEstatusCtz { get => _cambiarEstatusCtz; set { _cambiarEstatusCtz = value; OnPropertyChanged("CambiarEstatusCtz"); } }
        public ObservableCollection<EstatusCotizacion> ListaEstatusCtz { get => _listaEstatusCtz; set { _listaEstatusCtz = value; OnPropertyChanged("ListaEstatusCtz"); } }
        public EstatusCotizacion EstatusCotizacion { get => _estatusCotizacion; set { _estatusCotizacion = value; OnPropertyChanged("EstatusCotizacion"); } }
        public string Observaciones { get => _observaciones; set { _observaciones = value; OnPropertyChanged("Observaciones"); } }
        public CondicionesComerciales Condiciones { get => _condiciones; set { _condiciones = value; OnPropertyChanged("Condiciones"); } }        
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
            CambiarEstatusCtz = true;
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
                var result = await DialogHost.Show(vwBuscarCliente, "Prueba", ClosingEventHandler);
                ClienteSel = vmBuscarCliente.NvoCliente;
                CteRazonSocial = ClienteSel.RazonSocial + " | RFC:" + ClienteSel.Rfc + " | Codigo:" + ClienteSel.CodigoDeCliente;
                DatosCliente = "Contacto(s):" + ClienteSel.Contacto + " | Teléfono(s): " + ClienteSel.NumeroTelefono + " | Direccion: " + ClienteSel.Direccion;
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
                    var result = await DialogHost.Show(vwBuscarProducto, "Prueba", ClosingEventHandler);
                    if (result.Equals("SelProducto") == true)
                    {
                        ProductoSel = vmBuscarProducto.SelProducto;
                        ProductoSeleccionado ps = ListaProductos.SingleOrDefault(x => x.Producto.ClaveProducto == ProductoSel.Producto.ClaveProducto);
                        if (ps == null)
                        {
                            ListaProductos.Add(ProductoSel);
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
                    var result = await DialogHost.Show(vwMensaje, "Prueba");
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
            var result = await DialogHost.Show(vwEditarItem, "Prueba", ClosingEventHandler);
            if (result.Equals("OK") == true)
            {
                vmEditarItem.ActualizarProducto();
                producto = vmEditarItem.ProdSeleccionado;
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
            CalcularTotales();
            ChecarFechaEntrega();
        }        

        private void CalcularTotales()
        {
            PrecioUniTotal = 0.0;
            CantidadTotal = 0.0;
            DescuentoTotal = 0.0;
            ImporteTotal = 0.0;
            ImpuestoTotal = 0.0;
            SumaSubTotal = 0.0;
            foreach(ProductoSeleccionado p in ListaProductos)
            {
                PrecioUniTotal += p.Producto.PrecioUnitario;
                CantidadTotal += p.Cantidad;
                DescuentoTotal += p.ImporteDesc;
                ImporteTotal += p.Importe;
                ImpuestoTotal += p.Impuesto;
                SumaSubTotal += p.SubTotal;
            }
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

        private void GuardarCotizacion(object parameter)
        {
            
        }

        private async void CancelarCotizacion(object parameter)
        {
            if (ClienteSel != null)
            {
                var vmMensaje = new MensajeViewModel
                {
                    TituloMensaje = "Advertencia",
                    CuerpoMensaje = "¿Desea cancelar la cotización?"
                };
                var vwMensaje = new MensajeView
                {
                    DataContext = vmMensaje
                };
                var result = await DialogHost.Show(vwMensaje, "Prueba");
                if (result.Equals("ACEPTAR") == true)
                {
                    ListaProductos.Clear();
                    ClienteSel = null;
                    FechaCotizacion = DateTime.Now;
                    EstatusCotizacion = ListaEstatusCtz.First();
                    CteRazonSocial = string.Empty;
                    DatosCliente = string.Empty;
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
                    List<CondicionesComerciales> lista = JsonConvert.DeserializeObject<List<CondicionesComerciales>>(resp.Content);
                    Condiciones = lista.First();
                }
            }
            catch (Exception ex)
            {
                TxtMensaje = "Excepción: " + ex.Message;
                VerMensaje = true;
            }
        }
        #endregion
    }
}
