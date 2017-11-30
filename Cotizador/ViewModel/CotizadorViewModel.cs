using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cotizador.ViewModel
{
    public class CotizadorViewModel : Notificador
    {
        #region Variables
        private Cliente _clienteSel;
        private ObservableCollection<Cliente> _listaClientes;
        private ObservableCollection<ProductoSeleccionado> _listaProductos;
        private String _datosCliente;
        private String _cteRazonSocial;
        private ApiToken _apiToken;
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
        private DateTime _fecha;
        private DateTime _fechaVigencia;
        private DateTime _fechaEntrega;
        private Sucursal _sucursalSel;

        public ObservableCollection<Cliente> ListaClientes { get => _listaClientes; set { _listaClientes = value; OnPropertyChanged("ListaClientes"); } }
        public Cliente ClienteSel { get => _clienteSel; set { _clienteSel = value; OnPropertyChanged("NvoCliente"); } }
        public string DatosCliente { get => _datosCliente;  set { _datosCliente = value; OnPropertyChanged("DatosCliente"); } }
        public string CteRazonSocial { get => _cteRazonSocial; set { _cteRazonSocial = value; OnPropertyChanged("CteRazonSocial"); } }
        public ApiToken ApiToken { get => _apiToken; set { _apiToken = value; OnPropertyChanged("ApiToken"); } }
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
        public DateTime Fecha { get => _fecha; set { _fecha = value; OnPropertyChanged("Fecha"); } }
        #endregion

        #region Commands
        public RelayCommand AgregarClienteCommand { get; set; }
        public RelayCommand AgregarProductoCommand { get; set; }
        public RelayCommand EditarProductoCommand { get; set; }
        public RelayCommand QuitarProductoCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }        
        #endregion

        #region Constructor
        public CotizadorViewModel()
        {
            AgregarClienteCommand = new RelayCommand(AgregarCliente);
            AgregarProductoCommand = new RelayCommand(AgregarProducto);
            EditarProductoCommand = new RelayCommand(EditarProducto);
            QuitarProductoCommand = new RelayCommand(QuitarProducto);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            ListaProductos = new ObservableCollection<ProductoSeleccionado>();
            Fecha = DateTime.Now;
        }        
        #endregion

        #region Métodos
        private async void AgregarCliente(object parameter)
        {
            try
            {
                var vmBuscarCliente = new BuscarClientesViewModel
                {
                    ApiToken = ApiToken,
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
                var vmBuscarProducto = new BuscarProductosViewModel
                {
                    ApiToken = ApiToken,
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
                    EsImportado = (ProductoSel.Producto.EsImportado == 1) ? true : false;
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

        private void CerrarMensaje(object parameter)
        {
            VerMensaje = false;
        }
        #endregion
    }
}
