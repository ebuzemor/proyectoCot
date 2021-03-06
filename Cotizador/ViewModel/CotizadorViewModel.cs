﻿using Cotizador.Common;
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
using System.Text.RegularExpressions;

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
        public RelayCommand FechaEntregaCommand { get; set; }
        public RelayCommand GuardarCtzCommand { get; set; }
        public RelayCommand NuevaCtzCommand { get; set; }
        public RelayCommand VerObservacionCommand { get; set; }
        public RelayCommand EstatusBorradorCommand { get; set; }
        public RelayCommand EstatusPendienteCommand { get; set; }
        public RelayCommand EstatusDefinitivaCommand { get; set; }
        #endregion

        #region Variables
        private Cliente _clienteSel;
        private ObservableCollection<ProductoSeleccionado> _listaProductos;
        private ObservableCollection<ProductoSeleccionado> _listaProductosFT;
        private ObservableCollection<EstatusCotizacion> _listaEstatusCtz;
        private string _datosCliente;
        private string _cteRazonSocial;
        private ApiKey _appKey;
        private Usuario _usuario;
        private ProductoSeleccionado _productoSel;
        private string _localhost;
        private double _precioUniTotal;
        private double _cantidadTotal;
        private double _descuentoTotal;
        private double _desctoUniTotal;
        private double _importeTotal;
        private double _importeNetoTotal;
        private double _impuestoTotal;
        private double _sumaSubTotal;
        private bool _verMensaje;
        private int _enviarFichaTecnica;
        private string _txtMensaje;
        private DateTime _fechaCotizacion;
        private DateTime _fechaCtzVigencia;
        private DateTime _fechaCtzEntrega;
        private Sucursal _sucursalSel;
        private string _txtSucursal;
        private EstatusCotizacion _estatusCotizacion;
        private string _observaciones;
        private CondicionesComerciales _condiciones;
        private Cotizacion _miCotizacion;
        private InfoCotizaciones _infoCotizacion;
        private List<ComprobantesImpuestos> _listaCotizacionImpuestos;
        private List<ComprobantesImpuestos> _listaImpuestosXlinea;
        private List<DetalleComprobantes> _listaDetalleComprobantes;        
        private string _numCotizacion;
        private string _correosElectronicos;
        private int _indexEstatusCtz;
        private long _claveEstatusCtz;
        private long _editaSucursal;
        private long _editaUsuario;
        private bool _borradorSeleccionado;
        private bool _pendienteSeleccionado;
        private bool _definitivaSeleccionado;
        private bool _aceptaCambios;
        private bool _aceptaCambiosCliente;
        private ObservableCollection<AccionesDefinidas> _listaAcciones;

        public Cliente ClienteSel { get => _clienteSel; set { _clienteSel = value; OnPropertyChanged(); } }
        public string DatosCliente { get => _datosCliente;  set { _datosCliente = value; OnPropertyChanged(); } }
        public string CteRazonSocial { get => _cteRazonSocial; set { _cteRazonSocial = value; OnPropertyChanged(); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public ProductoSeleccionado ProductoSel { get => _productoSel; set { _productoSel = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductoSeleccionado> ListaProductos { get => _listaProductos; set { _listaProductos = value; OnPropertyChanged(); } }
        public double PrecioUniTotal { get => _precioUniTotal; set { _precioUniTotal = value; OnPropertyChanged(); } }
        public double CantidadTotal { get => _cantidadTotal; set { _cantidadTotal = value;  OnPropertyChanged(); } }
        public double DescuentoTotal { get => _descuentoTotal; set { _descuentoTotal = value; OnPropertyChanged(); } }
        public double DesctoUniTotal { get => _desctoUniTotal; set { _desctoUniTotal = value; OnPropertyChanged(); } }
        public double ImporteTotal { get => _importeTotal; set { _importeTotal = value; OnPropertyChanged(); } }
        public double ImporteNetoTotal { get => _importeNetoTotal; set { _importeNetoTotal = value; OnPropertyChanged(); } }
        public double ImpuestoTotal { get => _impuestoTotal; set { _impuestoTotal = value; OnPropertyChanged(); } }
        public double SumaSubTotal { get => _sumaSubTotal; set { _sumaSubTotal = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public int EnviarFichaTecnica { get => _enviarFichaTecnica; set { _enviarFichaTecnica = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public DateTime FechaCotizacion { get => _fechaCotizacion; set { _fechaCotizacion = value; OnPropertyChanged();  ChecarVigencia(_fechaCotizacion); } }
        public DateTime FechaCtzVigencia { get => _fechaCtzVigencia; set { _fechaCtzVigencia = value; OnPropertyChanged(); } }
        public DateTime FechaCtzEntrega { get => _fechaCtzEntrega; set { _fechaCtzEntrega = value; OnPropertyChanged(); } }
        public Sucursal SucursalSel { get => _sucursalSel; set { _sucursalSel = value; OnPropertyChanged(); } }
        public string TxtSucursal { get => _txtSucursal; set { _txtSucursal = value; OnPropertyChanged(); } }
        public ObservableCollection<EstatusCotizacion> ListaEstatusCtz { get => _listaEstatusCtz; set { _listaEstatusCtz = value; OnPropertyChanged(); } }
        public EstatusCotizacion EstatusCotizacion { get => _estatusCotizacion; set { _estatusCotizacion = value; OnPropertyChanged(); ActualizarEstatusRB(); } }
        public string Observaciones { get => _observaciones; set { _observaciones = value; OnPropertyChanged(); } }
        public CondicionesComerciales Condiciones { get => _condiciones; set { _condiciones = value; OnPropertyChanged(); } }
        public Cotizacion MiCotizacion { get => _miCotizacion; set { _miCotizacion = value; OnPropertyChanged(); } }
        public List<ComprobantesImpuestos> ListaCotizacionImpuestos { get => _listaCotizacionImpuestos; set => _listaCotizacionImpuestos = value; }
        public List<ComprobantesImpuestos> ListaImpuestosXlinea { get => _listaImpuestosXlinea; set => _listaImpuestosXlinea = value; }
        public List<DetalleComprobantes> ListaDetalleComprobantes { get => _listaDetalleComprobantes; set => _listaDetalleComprobantes = value; }
        public InfoCotizaciones InfoCotizacion { get => _infoCotizacion; set { _infoCotizacion = value; OnPropertyChanged(); } }
        public ObservableCollection<ProductoSeleccionado> ListaProductosFT { get => _listaProductosFT; set { _listaProductosFT = value; OnPropertyChanged(); } }
        public string NumCotizacion { get => _numCotizacion; set { _numCotizacion = value; OnPropertyChanged(); } }
        public int IndexEstatusCtz { get => _indexEstatusCtz; set { _indexEstatusCtz = value; OnPropertyChanged(); } }
        public long ClaveEstatusCtz { get => _claveEstatusCtz; set { _claveEstatusCtz = value; OnPropertyChanged(); } }
        public bool BorradorSeleccionado { get => _borradorSeleccionado; set { _borradorSeleccionado = value; OnPropertyChanged(); } }
        public bool PendienteSeleccionado { get => _pendienteSeleccionado; set { _pendienteSeleccionado = value; OnPropertyChanged(); } }
        public bool DefinitivaSeleccionado { get => _definitivaSeleccionado; set { _definitivaSeleccionado = value; OnPropertyChanged(); } }
        public bool AceptaCambiosCtz { get => _aceptaCambios; set { _aceptaCambios = value; OnPropertyChanged(); } }
        public bool AceptaCambiosCliente { get => _aceptaCambiosCliente; set { _aceptaCambiosCliente = value; OnPropertyChanged(); } }
        public string CorreosElectronicos { get => _correosElectronicos; set { _correosElectronicos = value; OnPropertyChanged(); } }
        public long EditaSucursal { get => _editaSucursal; set { _editaSucursal = value; OnPropertyChanged(); } }
        public long EditaUsuario { get => _editaUsuario; set { _editaUsuario = value; OnPropertyChanged(); } }
        public ObservableCollection<AccionesDefinidas> ListaAcciones { get => _listaAcciones; set { _listaAcciones = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public CotizadorViewModel()
        {
            AgregarClienteCommand = new RelayCommand(AgregarCliente);
            AgregarProductoCommand = new RelayCommand(AgregarProducto);
            EditarProductoCommand = new RelayCommand(EditarProducto);
            QuitarProductoCommand = new RelayCommand(QuitarProducto);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            FechaEntregaCommand = new RelayCommand(DefineFechaEntrega);
            GuardarCtzCommand = new RelayCommand(GuardarCotizacion);
            NuevaCtzCommand = new RelayCommand(NuevaCotizacion);
            VerObservacionCommand = new RelayCommand(VerObservacion);
            EstatusBorradorCommand = new RelayCommand(EstatusBorrador);
            EstatusPendienteCommand = new RelayCommand(EstatusPendiente);
            EstatusDefinitivaCommand = new RelayCommand(EstatusDefinitiva);
            ListaProductos = new ObservableCollection<ProductoSeleccionado>();
            FechaCotizacion = DateTime.Now;
            BorradorSeleccionado = true;
            AceptaCambiosCtz = true;
            AceptaCambiosCliente = true;
            EditaSucursal = 0;
            EditaUsuario = 0;
        }        
        #endregion

        #region Métodos
        private async void AgregarCliente(object parameter)
        {
            try
            {
                var permiso = ListaAcciones.Single(x => x.Constante.Equals("AGREGAR_CLIENTE") == true);
                if (permiso.Activo == true)
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
                    var result = await DialogHost.Show(vwBuscarCliente, "CrearCotizacion");
                    if (result.Equals("NvoCliente") == true)
                    {
                        ClienteSel = vmBuscarCliente.NvoCliente;
                        CteRazonSocial = ClienteSel.RazonSocial + " | RFC:" + ClienteSel.Rfc + " | Codigo:" + ClienteSel.CodigoDeCliente;
                        DatosCliente = "Contacto(s):" + ClienteSel.Contacto + " | Teléfono(s): " + ClienteSel.NumeroTelefono + " | Direccion: " + ClienteSel.Direccion;
                    }
                }
                else
                {
                    TxtMensaje = "Usted no tiene permiso para agregar Clientes, verifique con el Administrador.";
                    VerMensaje = true;
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
                var permiso = ListaAcciones.Single(x => x.Constante.Equals("AGREGAR_PRODUCTO") == true);
                if (permiso.Activo == true)
                {
                    bool validarAutor = (InfoCotizacion != null) ? ValidarAutorCotizacion() : true;
                    if (validarAutor == true)
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
                            var result = await DialogHost.Show(vwBuscarProducto, "CrearCotizacion");
                            if (result.Equals("SelProducto") == true)
                            {
                                ProductoSel = vmBuscarProducto.SelProducto;
                                ProductoSel.FechaEntrega = DateTime.Now;
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
                                ChecarFechaEntrega(FechaCotizacion, ListaProductos);
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
                            var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                        }
                    }
                    else
                    {
                        TxtMensaje = "Esta cotización ha sido creada por otro usuario, no es posible realizar modificaciones.";
                        VerMensaje = true;
                    }
                }
                else
                {
                    TxtMensaje = "Usted no tiene permiso para agregar productos.";
                    VerMensaje = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void EditarProducto(object parameter)
        {
            var permiso = ListaAcciones.Single(x => x.Constante.Equals("EDITAR_PRODUCTO") == true);
            if (permiso.Activo == true)
            {
                bool validarAutor = (InfoCotizacion != null) ? ValidarAutorCotizacion() : true;
                if (validarAutor == true)
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
                    var result = await DialogHost.Show(vwEditarItem, "CrearCotizacion");
                    if (result.Equals("OK") == true)
                    {
                        vmEditarItem.ActualizarProducto();
                        producto = vmEditarItem.ProdSeleccionado;
                        CalcularTotales();
                    }
                }
                else
                {
                    TxtMensaje = "Esta cotización ha sido creada por otro usuario, no es posible realizar modificaciones.";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "Usted no tiene permitido editar cantidad y descuento de productos.";
                VerMensaje = true;
            }
        }

        private async void DefineFechaEntrega(object parameter)
        {
            try
            {
                var permiso = ListaAcciones.Single(x => x.Constante.Equals("DEFINE_ENTREGA") == true);
                if (permiso.Activo == true)
                {
                    bool validarAutor = (InfoCotizacion != null) ? ValidarAutorCotizacion() : true;
                    if (validarAutor == true)
                    {
                        var clvProducto = Convert.IsDBNull(parameter) ? 0 : Convert.ToInt64(parameter);
                        ProductoSeleccionado prodsel = ListaProductos.Single(x => x.Producto.ClaveProducto == clvProducto);
                        DateTime defineFecha = (prodsel.DiasEntrega > 0) ? FechaCotizacion.AddDays(prodsel.DiasEntrega) : FechaCtzVigencia;
                        var vmFecEntrega = new FechaEntregaViewModel
                        {
                            ProdSeleccionado = prodsel,
                            FechaLimite = FechaCotizacion.Date,
                            FechaEntrega = defineFecha.Date
                        };
                        var vwFecEntrega = new FechaEntregaView
                        {
                            DataContext = vmFecEntrega
                        };
                        object resFec = await DialogHost.Show(vwFecEntrega, "EmailCotizacion");
                        if (resFec.Equals("OK") == true)
                        {
                            prodsel.FechaEntrega = vmFecEntrega.FechaEntrega;
                            TimeSpan ts = vmFecEntrega.FechaEntrega - FechaCotizacion.Date;
                            prodsel.DiasEntrega = ts.Days;
                            ChecarFechaEntrega(FechaCotizacion, ListaProductos);
                        }
                    }
                    else
                    {
                        TxtMensaje = "Esta cotización ha sido creada por otro usuario, no es posible realizar modificaciones.";
                        VerMensaje = true;
                    }
                }
                else
                {
                    TxtMensaje = "No tiene permitido establecer la fecha de entrega de productos.";
                    VerMensaje = true;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void QuitarProducto(object parameter)
        {
            var permiso = ListaAcciones.Single(x => x.Constante.Equals("EDITAR_PRODUCTO") == true);
            if (permiso.Activo == true)
            {
                bool validarAutor = (InfoCotizacion != null) ? ValidarAutorCotizacion() : true;
                if (validarAutor == true)
                {
                    Int64 clvProducto = Convert.IsDBNull(parameter) ? 0 : Convert.ToInt64(parameter);
                    ProductoSeleccionado prodsel = ListaProductos.Single(x => x.Producto.ClaveProducto == clvProducto);
                    var vmMsj = new MensajeViewModel
                    {
                        TituloMensaje = "Advertencia",
                        CuerpoMensaje = "Confirme la eliminación de la lista del producto: " + prodsel.Producto.Descripcion,
                        MostrarCancelar = true
                    };
                    var vwMsj = new MensajeView
                    {
                        DataContext = vmMsj
                    };
                    var result = await DialogHost.Show(vwMsj, "CrearCotizacion");
                    if (result.Equals("OK") == true)
                    {
                        ListaProductos.Remove(prodsel);
                        CalcularTotales();
                        ChecarFechaEntrega(FechaCotizacion, ListaProductos);
                    }
                }
                else
                {
                    TxtMensaje = "Esta cotización ha sido creada por otro usuario, no es posible realizar modificaciones.";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "No tiene permitido eliminar productos de la lista.";
                VerMensaje = true;
            }
        }

        public void CalcularTotales()
        {
            PrecioUniTotal = 0.0;
            CantidadTotal = 0.0;
            DescuentoTotal = 0.0;
            DesctoUniTotal = 0.0;
            ImporteTotal = 0.0;
            ImporteNetoTotal = 0.0;
            ImpuestoTotal = 0.0;
            SumaSubTotal = 0.0;
            foreach (ProductoSeleccionado p in ListaProductos)
            {
                PrecioUniTotal += p.Producto.PrecioUnitario;
                CantidadTotal += p.Cantidad;
                DescuentoTotal += p.ImporteDesc;
                DesctoUniTotal += p.DesctoUnitario;
                ImporteTotal += p.Importe;
                ImpuestoTotal += p.Impuesto;
                SumaSubTotal += p.SubTotal;
            }
            PrecioUniTotal = Math.Round(PrecioUniTotal, 2);
            CantidadTotal = Math.Round(CantidadTotal, 2);
            DescuentoTotal = Math.Round(DescuentoTotal, 2);
            DesctoUniTotal = Math.Round(DesctoUniTotal, 2);
            ImporteTotal = Math.Round(ImporteTotal, 2);
            ImporteNetoTotal = Math.Round(ImporteTotal - DescuentoTotal, 2);
            ImpuestoTotal = Math.Round(ImpuestoTotal, 2);
            SumaSubTotal = Math.Round(SumaSubTotal, 2);
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;

        private void ChecarVigencia(DateTime fecha)
        {
            FechaCtzVigencia = FechaCotizacion.AddDays(15);
            FechaCtzEntrega = FechaCtzVigencia;
        }

        public void ChecarFechaEntrega(DateTime FechaCtz, ObservableCollection<ProductoSeleccionado> Lista)
        {            
            int dias = 0;
            if (Lista != null)
            {
                foreach (ProductoSeleccionado p in Lista)
                {
                    if (p.DiasEntrega > dias)
                    {
                        dias = p.DiasEntrega;
                        FechaCtzEntrega = FechaCtz.AddDays(dias);
                    }
                }
            }
        }

        private async void GuardarCotizacion(object parameter)
        {
            var permiso = ListaAcciones.Single(x => x.Constante.Equals("GUARDAR_COTIZACION") == true);
            if (permiso.Activo == true)
            {
                bool validarAutor = (InfoCotizacion != null) ? ValidarAutorCotizacion() : true;
                if (validarAutor == true || EstatusCotizacion.ClaveTipoDeStatusDeComprobante == 162)
                {
                    if (ClienteSel != null && ListaProductos.Count > 0)
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
                                Importe = item.Cantidad * item.Producto.PrecioUnitario, //item.Importe,
                                ImporteDescuento = item.ImporteDesc,
                                Impuestos = JsonConvert.SerializeObject(ListaImpuestosXlinea),
                                NumeroPartidas = c,
                                PrecioUnitario = item.Producto.PrecioUnitario,
                                DiasDeEntrega = item.DiasEntrega,
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
                        string clvcomprobante = null;
                        string codcomprobante = null;
                        ///Si es edición de cotización se reutilizan el código y la clave de comprobante
                        if (InfoCotizacion != null)
                        {
                            clvcomprobante = InfoCotizacion.ClaveComprobanteDeCotizacion;
                            codcomprobante = InfoCotizacion.CodigoDeComprobante;
                        }
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
                            ClaveEntidadFiscalInmueble = (EditaSucursal > 0) ? EditaSucursal : Usuario.ClaveEntidadFiscalInmueble,
                            ClaveEntidadFiscalResponsable = (EditaUsuario > 0) ? EditaUsuario : Usuario.ClaveEntidadFiscalEmpleado,
                            ClaveTipoEstatusRecepcion = EstatusCotizacion.ClaveTipoDeStatusDeComprobante,
                            ClaveComprobante = clvcomprobante,
                            CodigoDeComprobante = codcomprobante,
                            ListaComprobantesImpuestos = JsonConvert.SerializeObject(listaSumaImpuestos),
                            ClaveEntidadFiscalCliente = ClienteSel.ClaveEntidadFiscalCliente,
                            ClaveListaDePrecios = 1,
                            FechaVigencia = FechaCtzVigencia.ToString("yyyy-MM-dd HH:mm:ss"),
                            SubTotal = ImporteTotal,
                            Descuento = DescuentoTotal,
                            Impuestos = ImpuestoTotal,
                            Total = SumaSubTotal,
                            Observaciones = Observaciones,
                            DetallesDeComprobante = JsonConvert.SerializeObject(ListaDetalleComprobantes)
                        };
                        String AprosiCtz = JsonConvert.SerializeObject(MiCotizacion);

                        // se procede a ejecutar los webservices de guardado o edición de cotización
                        var rest = new RestClient(Localhost);
                        var req = new RestRequest();
                        string cuerpomsj = null;
                        if (InfoCotizacion == null)
                        {
                            req = new RestRequest("guardarCotizacion", Method.POST);
                            cuerpomsj = "La cotizacion {0} fue generada de manera exitosa.";
                        }
                        else
                        {
                            req = new RestRequest("editarCotizacion", Method.POST);
                            cuerpomsj = "Se guardaron los cambios correctamente en la cotizacion: {0}";
                        }
                        req.AddHeader("Accept", "application/json");
                        req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                        req.AddParameter("text/json", AprosiCtz, ParameterType.RequestBody);
                        req.RequestFormat = DataFormat.Json;

                        IRestResponse response = rest.Execute(req);
                        if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                        {
                            if (InfoCotizacion == null)
                            {
                                List<ComprobanteGenerado> compgen = JsonConvert.DeserializeObject<List<ComprobanteGenerado>>(response.Content);
                                NumCotizacion = compgen.First().FolioCodigoComprobante;
                            }
                            else
                            {
                                var respuesta = JsonConvert.DeserializeObject(response.Content);
                                NumCotizacion = InfoCotizacion.CodigoDeComprobante;
                            }
                            var vmMensaje = new MensajeViewModel
                            {
                                TituloMensaje = "Aviso",
                                MostrarCancelar = false,
                                CuerpoMensaje = string.Format(cuerpomsj, NumCotizacion)
                            };
                            var vwMensaje = new MensajeView
                            {
                                DataContext = vmMensaje
                            };
                            var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                            if (MiCotizacion.ClaveTipoEstatusRecepcion == 160)
                            {
                                EnviarCotizacion();
                            }
                            LimpiarCotizacion();
                        }
                    }
                    else
                    {
                        TxtMensaje = "Para guardar una cotización debe tener un Cliente seleccionado y al menos un producto en la lista.";
                        VerMensaje = true;
                    }
                }
                else
                {
                    TxtMensaje = "Esta cotización ha sido creada por otro usuario, no es posible realizar modificaciones.";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "No tiene permitido guardar cotizaciones";
                VerMensaje = true;
            }
        }        

        private async void NuevaCotizacion(object parameter)
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
                var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                if (result.Equals("OK") == true)
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
            Observaciones = null;
            NumCotizacion = null;
            AceptaCambiosCtz = true;
            AceptaCambiosCliente = true;
            InfoCotizacion = null;
            IndexEstatusCtz = 0;
            EditaSucursal = 0;
            EditaUsuario = 0;
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

        private void ActualizarEstatusRB()
        {
            switch (EstatusCotizacion.ClaveTipoDeStatusDeComprobante)
            {
                case 160:
                    BorradorSeleccionado = true;
                    PendienteSeleccionado = false;
                    DefinitivaSeleccionado = false;
                    AceptaCambiosCtz = true;
                    AceptaCambiosCliente = (InfoCotizacion != null) ? false : true;
                    ClaveEstatusCtz = 160;
                    break;
                case 161:
                    BorradorSeleccionado = false;
                    PendienteSeleccionado = true;
                    DefinitivaSeleccionado = false;
                    AceptaCambiosCtz = true;
                    AceptaCambiosCliente = (InfoCotizacion != null) ? false : true;
                    ClaveEstatusCtz = 161;
                    break;
                case 162:
                case 164:
                    BorradorSeleccionado = false;
                    PendienteSeleccionado = false;
                    DefinitivaSeleccionado = true;
                    AceptaCambiosCtz = false;
                    AceptaCambiosCliente = false;
                    ClaveEstatusCtz = 162;
                    break;
                default: break;
            }
        }

        private void EstatusBorrador(object parameter)
        {
            var permiso = ListaAcciones.Single(x => x.Constante.Equals("PENDIENTE_BORRADOR") == true);
            if (permiso.Activo == true)
            {
                bool validarAutor = (InfoCotizacion != null) ? ValidarAutorCotizacion() : true;
                if (validarAutor == true)
                {
                    EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == 160);
                }
                else
                {
                    TxtMensaje = "Esta cotización ha sido creada por otro usuario, no es posible realizar modificaciones.";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "No tiene permitido cambiar el estatus de la cotización a borrador";
                VerMensaje = true;
            }
            ActualizarEstatusRB();
        }

        private async void EstatusPendiente(object parameter)
        {
            var permiso1 = ListaAcciones.Single(x => x.Constante.Equals("BORRADOR_PENDIENTE") == true);
            var permiso2 = ListaAcciones.Single(x => x.Constante.Equals("DEFINITIVA_PENDIENTE") == true);
            if (permiso1.Activo == true || permiso2.Activo == true)
            {
                bool validarAutor = (InfoCotizacion != null) ? ValidarAutorCotizacion() : true;
                if (validarAutor == true)
                {
                    if (ListaProductos.Count > 0)
                    {
                        if (InfoCotizacion != null && InfoCotizacion.ClaveEstatus == 160) //Sólo mostrará el mensaje si la cotización estaba en borrador, para no enviar demasiados emails.
                        {
                            var vmMensaje = new MensajeViewModel
                            {
                                TituloMensaje = "Aviso",
                                CuerpoMensaje = "El cambio de estatus de la Cotización a Pendiente permitirá ser autorizada para su facturación, ¿Desea cambiar el estatus?",
                                MostrarCancelar = true
                            };
                            var vwMensaje = new MensajeView
                            {
                                DataContext = vmMensaje
                            };
                            var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                            if (result.Equals("OK") == true)
                            {
                                EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == 161);
                                GuardarCotizacion(ClienteSel);
                            }
                            else
                                EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == 160);
                        }
                        else if (InfoCotizacion == null)
                        {
                            var vmMensaje = new MensajeViewModel
                            {
                                TituloMensaje = "Error",
                                CuerpoMensaje = "La cotización debe haberse guardado con el estatus de borrador antes de poder cambiar el estatus a Pendiente",
                                MostrarCancelar = false
                            };
                            var vwMensaje = new MensajeView
                            {
                                DataContext = vmMensaje
                            };
                            var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                        }
                        else //if (InfoCotizacion.ClaveEstatus == 162) //Si la cotización estaba en Definitiva no se requiere mostrar mensaje.
                        {
                            EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == 161);
                        }
                    }
                    else
                    {
                        var vmMensaje = new MensajeViewModel
                        {
                            TituloMensaje = "Error",
                            CuerpoMensaje = "Para cambiar el estatus a Pendiente de Autorizar debe haber un cliente seleccionado y al menos un producto en la lista.",
                            MostrarCancelar = false
                        };
                        var vwMensaje = new MensajeView
                        {
                            DataContext = vmMensaje
                        };
                        var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                        EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == 160);
                    }
                }
                else
                {
                    TxtMensaje = "Esta cotización ha sido creada por otro usuario, no es posible realizar modificaciones.";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "No tiene permitido cambiar el estatus de la cotización a pendiente de autorizar";
                VerMensaje = true;
            }
            ActualizarEstatusRB();
        }

        private async void EstatusDefinitiva(object parameter)
        {
            var permiso = ListaAcciones.Single(x => x.Constante.Equals("PENDIENTE_DEFINITIVA") == true);
            if (permiso.Activo == true)
            {
                if (ListaProductos.Count > 0)
                {
                    if (InfoCotizacion.ClaveEstatus == 161)
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
                        var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                        if (result.Equals("OK") == true)
                        {
                            EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == 162);
                            GuardarCotizacion(ClienteSel);
                        }
                        else
                            EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == 161); //Si cancela el cambio de estatus, se regresa a Pendiente
                    }
                    else
                    {
                        var vmMensaje = new MensajeViewModel
                        {
                            TituloMensaje = "Error",
                            CuerpoMensaje = "Para cambiar el estatus a Definitiva, la cotización debe estar en estatus de Pendiente de Autorizar.",
                            MostrarCancelar = false
                        };
                        var vwMensaje = new MensajeView
                        {
                            DataContext = vmMensaje
                        };
                        var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                        EstatusCotizacion = ListaEstatusCtz.Single(x => x.ClaveTipoDeStatusDeComprobante == 160);
                    }
                }
                else
                {
                    var vmMensaje = new MensajeViewModel
                    {
                        TituloMensaje = "Error",
                        CuerpoMensaje = "No es posible cambiar el estatus de la cotización si no tiene productos agregados.",
                        MostrarCancelar = false
                    };
                    var vwMensaje = new MensajeView
                    {
                        DataContext = vmMensaje
                    };
                    var result = await DialogHost.Show(vwMensaje, "CrearCotizacion");
                }
            }
            else
            {
                TxtMensaje = "No tiene permitido autorizar cotizaciones.";
                VerMensaje = true;
            }
            ActualizarEstatusRB();
        }

        private async void EnviarCotizacion()
        {
            if (string.IsNullOrEmpty(NumCotizacion) == false)
            {
                if (InfoCotizacion != null)
                {
                    CorreosElectronicos = InfoCotizacion.CorreoElectronico;
                    NumCotizacion = InfoCotizacion.CodigoDeComprobante;
                }
                else if (string.IsNullOrEmpty(ClienteSel.CorreoElectronico) == false)
                    CorreosElectronicos = ClienteSel.CorreoElectronico;
                else
                    CorreosElectronicos = string.Empty;
                ListaProductosFT = new ObservableCollection<ProductoSeleccionado>();
                foreach (ProductoSeleccionado p in ListaProductos)
                {
                    ListaProductosFT.Add(p);
                }
                var vmEmailCtz = new EnviarCotizacionViewModel
                {
                    TituloEnvio = "Enviar cotizacion #" + NumCotizacion,
                    ListaProductosFT = ListaProductosFT,
                    NumCotizacion = NumCotizacion,
                    CorreosElectronicos = CorreosElectronicos
                };
                var vwEmailCtz = new EnviarCotizacionView()
                {
                    DataContext = vmEmailCtz
                };
                var envio = await DialogHost.Show(vwEmailCtz,"EmailCotizacion");
                if (envio.Equals("ENVIAR") == true)
                {
                    CorreosElectronicos = vmEmailCtz.CorreosElectronicos;
                    bool existeError = ValidarCorreo();
                    if (existeError == true)
                    {
                        TxtMensaje = "La cotización no puede ser enviada hasta que las direcciones de email estén escritas de manera correcta y no existan espacios en blanco.";
                        VerMensaje = true;
                    }
                    else
                    {
                        string prmCotizacion = vmEmailCtz.NumCotizacion;
                        string prmEmails = String.Join(",", Regex.Split(CorreosElectronicos, @"\r\n"));
                        List<EnvioFichaTecnica> listaFT = new List<EnvioFichaTecnica>();
                        foreach (ProductoSeleccionado ps in vmEmailCtz.ListaProductosFT)
                        {
                            var envft = new EnvioFichaTecnica
                            {
                                ClaveProducto = ps.Producto.ClaveProducto,
                                EnvioFicha = (ps.FichaTecnica == true) ? 1 : 0
                            };
                            listaFT.Add(envft);
                        }
                        string datosFT = JsonConvert.SerializeObject(listaFT);
                        var rest = new RestClient(Localhost);
                        var req = new RestRequest("enviarMail", Method.POST);
                        req.AddHeader("Accept", "application/json");
                        req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                        req.AddParameter("claveComprobante", prmCotizacion);
                        req.AddParameter("emails", prmEmails);
                        req.AddParameter("claveEF_Empresa", Usuario.ClaveEntidadFiscalEmpresa);
                        req.AddParameter("fichaTecnica", datosFT);

                        IRestResponse response = rest.Execute(req);
                        if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                        {
                            TxtMensaje = "La cotización fue enviada correctamente";
                            VerMensaje = true;
                        }
                        else
                        {
                            TxtMensaje = "Hubo un error al enviar la cotizacion: " + response.Content;
                            VerMensaje = true;
                        }
                        EnviarFichaTecnica = 0;
                    }
                }
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

        private async void VerObservacion(object parameter)
        {
            var vmObs = new ObservacionesViewModel
            {
                Observaciones = Observaciones,
                DefinitivaSeleccionado = DefinitivaSeleccionado
            };
            var vwObs = new ObservacionesView
            {
                DataContext = vmObs
            };
            var result = await DialogHost.Show(vwObs, "CrearCotizacion");
            Observaciones = vmObs.Observaciones;
        }

        private bool ValidarAutorCotizacion()
        {
            bool ban = false;
            if (InfoCotizacion != null)
            {
                if (InfoCotizacion.ClaveEntidadFiscalResponsable == Usuario.ClaveEntidadFiscalEmpleado)
                    ban = true;
            }
            return ban;
        }
        #endregion
    }
}