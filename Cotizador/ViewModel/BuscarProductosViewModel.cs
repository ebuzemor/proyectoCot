using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using Newtonsoft.Json;
using MaterialDesignThemes.Wpf;
using RestSharp;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Net;
using System.Windows.Data;

namespace Cotizador.ViewModel
{
    public class BuscarProductosViewModel : Notificador
    {
        #region Commands
        public RelayCommand BuscarProductoCommand { get; set; }
        public RelayCommand SeleccionadoCommand { get; set; }
        public RelayCommand InicioCommand { get; set; }
        public RelayCommand AnteriorCommand { get; set; }
        public RelayCommand SiguienteCommand { get; set; }
        public RelayCommand FinalCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        public RelayCommand ExistenciasProductoCommand { get; set; }
        #endregion

        #region Variables
        private ObservableCollection<Producto> _listaProductos;
        private Producto _nvoProducto;
        private ProductoSeleccionado _selProducto;
        private ProductosJson _productosJson;
        private String _txtProducto;
        private ApiKey _appKey;
        private Usuario _usuario;        
        private CollectionViewSource _cvsProductos;
        private String _localhost;        
        private int _pagsTotales;
        private int _indicePagActual;        
        private int _itemsPorPag;
        private int _pagActual;
        private Boolean _activoInicio;
        private Boolean _activoAnterior;
        private Boolean _activoSiguiente;
        private Boolean _activoFinal;
        private Boolean _activoSeleccionar;
        private Double _txtCantidad;
        private Double _txtDescuento;
        private Double _txtImporteDesc;
        private Double _txtImporte;
        private Boolean _verMensaje;
        private String _txtMensaje;
        private ObservableCollection<Existencias> _listaExistencias;

        public ObservableCollection<Producto> ListaProductos { get => _listaProductos; set { _listaProductos = value; OnPropertyChanged("ListaProductos"); } }        
        public ProductosJson ProductosJson { get => _productosJson; set { _productosJson = value; OnPropertyChanged("ProdutosJson"); } }
        public string TxtProducto { get => _txtProducto; set { _txtProducto = value; OnPropertyChanged("TxtProducto"); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }        
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }        
        public ProductoSeleccionado SelProducto { get => _selProducto; set { _selProducto = value; OnPropertyChanged("SelProducto"); } }
        public CollectionViewSource CvsProductos { get => _cvsProductos; set { _cvsProductos = value; OnPropertyChanged("CvsProductos"); } }
        public int ItemsPorPag { get => _itemsPorPag; set => _itemsPorPag = value; }
        public int PagsTotales { get => _pagsTotales; private set { _pagsTotales = value; OnPropertyChanged("PagsTotales"); } }
        public int IndicePagActual { get => _indicePagActual; set { _indicePagActual = value; OnPropertyChanged("IndicePagActual"); } }
        public int PagActual { get => _indicePagActual + 1; set { _pagActual = value; OnPropertyChanged("PagActual"); } }
        public bool ActivoInicio { get => _activoInicio; set { _activoInicio = value; OnPropertyChanged("ActivoInicio"); } }
        public bool ActivoAnterior { get => _activoAnterior; set { _activoAnterior = value; OnPropertyChanged("ActivoAnterior"); } }
        public bool ActivoSiguiente { get => _activoSiguiente; set { _activoSiguiente = value; OnPropertyChanged("ActivoSiguiente"); } }
        public bool ActivoFinal { get => _activoFinal; set { _activoFinal = value; OnPropertyChanged("ActivoFinal"); } }                
        public double TxtImporte { get => _txtImporte; set { _txtImporte = value; OnPropertyChanged("TxtImporte"); } }
        public double TxtImporteDesc { get => _txtImporteDesc; set { _txtImporteDesc = value; OnPropertyChanged("TxtImporteDesc"); } }
        public bool ActivoSeleccionar { get => _activoSeleccionar; set { _activoSeleccionar = value; OnPropertyChanged("ActivoSeleccionar"); } }
        public double TxtCantidad
        {
            get => _txtCantidad;
            set
            {
                if (NvoProducto != null && NvoProducto.EsFraccionable == 0)
                {
                    _txtCantidad = Convert.ToInt32(value);
                }
                else
                    _txtCantidad = Convert.ToDouble(value);
                OnPropertyChanged("TxtCantidad");
                CalcularImporte(_txtCantidad);
                ActivarBtnSeleccionar();
            }
        }
        public double TxtDescuento
        {
            get => _txtDescuento;
            set
            {
                _txtDescuento = value;
                OnPropertyChanged("TxtDescuento");
                CalcularDescuento(_txtDescuento);
                ActivarBtnSeleccionar();
            }
        }
        public Producto NvoProducto
        {
            get => _nvoProducto;
            set
            {
                _nvoProducto = value;
                OnPropertyChanged("NvoProducto");
                CalcularImporte(TxtCantidad);
                CalcularDescuento(TxtDescuento);
                ActivarBtnSeleccionar();
            }
        }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged("VerMensaje"); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged("TxtMensaje"); } }
        public ObservableCollection<Existencias> ListaExistencias { get => _listaExistencias; set { _listaExistencias = value; OnPropertyChanged("ListaExistencias"); } }
        #endregion

        #region Constructor
        public BuscarProductosViewModel()
        {
            BuscarProductoCommand = new RelayCommand(BuscarProducto);
            SeleccionadoCommand = new RelayCommand(Seleccionado);
            InicioCommand = new RelayCommand(Inicio);
            AnteriorCommand = new RelayCommand(Anterior);
            SiguienteCommand = new RelayCommand(Siguiente);
            FinalCommand = new RelayCommand(Final);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            ExistenciasProductoCommand = new RelayCommand(ExistenciasProducto);
            SelProducto = new ProductoSeleccionado();
            IndicePagActual = 0;
            ItemsPorPag = 10;
        }
        #endregion

        #region Metodos
        private void Seleccionado(object parameter)
        {
            Console.WriteLine(NvoProducto.Descripcion);
        }

        private void BuscarProducto(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtProducto) != true)
                {
                    var rest = new RestClient(Localhost);
                    var clvEFInmueble = (Usuario.Sucursal == 3001) ? 300000108 : Usuario.ClaveEntidadFiscalInmueble;
                    var req = new RestRequest("buscarProductos/" + clvEFInmueble + "/" + Usuario.ClaveEntidadFiscalEmpresa + "/" + TxtProducto, Method.GET);
                    req.AddHeader("Accept", "application/json");
                    req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                    IRestResponse<ProductosJson> resp = rest.Execute<ProductosJson>(req);
                    if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
                    {
                        ProductosJson = JsonConvert.DeserializeObject<ProductosJson>(resp.Content);
                        ListaProductos = new ObservableCollection<Producto>(ProductosJson.Productos);
                        ///Paginación de los resultados
                        CvsProductos = new CollectionViewSource
                        {
                            Source = ListaProductos
                        };
                        CvsProductos.Filter += new FilterEventHandler(FiltroPaginas);                        
                    }
                    if (ListaProductos != null && ListaProductos.Count == 0)
                    {
                        TxtMensaje = "La búsqueda no obtuvo resultados, intente de nuevo.";
                        VerMensaje = true;
                    }
                }
                else
                {
                    TxtMensaje = "Debe escribir algo para realizar la búsqueda de Clientes.";
                    VerMensaje = true;
                }
                IndicePagActual = 0;
                ActualizarPagina();
                CalcularPagsTotales();
                ActivarBotones();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

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
            CvsProductos.View.Refresh();
            ActivarBotones();
        }

        private void CalcularPagsTotales()
        {
            if(ListaProductos.Count % ItemsPorPag == 0)
            {
                PagsTotales = ListaProductos.Count / ItemsPorPag;
            }
            else
            {
                PagsTotales = (ListaProductos.Count / ItemsPorPag) + 1;
            }
        }        

        private void FiltroPaginas(object sender, FilterEventArgs e)
        {
            int index = ListaProductos.IndexOf((Producto)e.Item);
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
            if (ListaProductos != null && ListaProductos.Count > 0)
            {
                ActivoInicio = (IndicePagActual != 0) ? true : false;
                ActivoAnterior = (IndicePagActual != 0) ? true : false;
                ActivoSiguiente = (IndicePagActual < PagsTotales - 1) ? true : false;
                ActivoFinal = (PagActual != PagsTotales) ? true : false;
            }
        }

        private void CalcularImporte(double? cantidad)
        {
            try
            {
                if (NvoProducto != null)
                {
                    if (cantidad > 0)
                    {
                        TxtImporte = Convert.ToDouble(cantidad * NvoProducto.PrecioUnitario); // - TxtImporteDesc;
                        CalcularDescuento(TxtDescuento);
                    }
                    else
                        TxtImporte = 0;
                    SeleccionarProducto();
                }
            }
            catch(Exception)
            {
                TxtImporte = 0;
            }
        }

        private void CalcularDescuento(double? descuento)
        {
            try
            {
                if (NvoProducto != null)
                {
                    if (descuento > 0 && TxtCantidad > 0 && descuento <= 0.9999)
                    {
                        TxtImporteDesc = Convert.ToDouble(descuento) * NvoProducto.PrecioUnitario * TxtCantidad;
                        TxtImporte = (NvoProducto.PrecioUnitario * TxtCantidad);// - TxtImporteDesc;
                    }
                    else if (descuento == 0 && TxtCantidad > 0)
                    {
                        TxtImporte = NvoProducto.PrecioUnitario * TxtCantidad;
                        TxtImporteDesc = 0;
                    }
                    else
                    {
                        TxtImporte = 0;
                        TxtImporteDesc = 0;
                    }
                    SeleccionarProducto();
                }
            }
            catch (Exception)
            {
                TxtImporte = 0;
                TxtImporteDesc = 0;
            }
        }

        private void ActivarBtnSeleccionar()
        {
            bool desctoValido = (TxtDescuento >= 0 && TxtDescuento <= 0.9999) ? true : false;
            ActivoSeleccionar = (TxtCantidad > 0 && desctoValido == true && TxtImporte > 0) ? true : false;
        }

        private void SeleccionarProducto()
        {
            if (NvoProducto != null)
            {
                SelProducto.Producto = NvoProducto;
                SelProducto.Cantidad = Math.Round(TxtCantidad, 2);
                SelProducto.Descuento = Math.Round(TxtDescuento, 4);
                SelProducto.ImporteDesc = Math.Round(TxtImporteDesc, 2);
                SelProducto.Importe = Math.Round(TxtImporte, 2);
                double importeNeto = Math.Round(TxtImporte - TxtImporteDesc, 2);
                double impuestos = importeNeto * (SelProducto.Producto.SumaImpuestos / 100.0);
                SelProducto.Impuesto = Math.Round(impuestos, 2);
                SelProducto.SubTotal = Math.Round(importeNeto + impuestos, 2);
            }
        }

        private void CerrarMensaje(object paramter) => VerMensaje = false;

        private async void ExistenciasProducto(object parameter)
        {
            string claveProducto = Convert.ToString(parameter);            
            var rest = new RestClient(Localhost);
            var req = new RestRequest("buscarExistencias/" + Usuario.ClaveEntidadFiscalEmpresa + "/" + claveProducto, Method.GET);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Authorization", "Bearer " + AppKey.Token);

            try
            {
                IRestResponse resp = rest.Execute(req);
                if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
                {
                    ExistenciasJson lista = JsonConvert.DeserializeObject<ExistenciasJson>(resp.Content);
                    ListaExistencias = new ObservableCollection<Existencias>(lista.Existencias);
                }
                if (ListaExistencias != null && ListaExistencias.Count == 0)
                {
                    TxtMensaje = "No existen movimientos de inventario registrados de este producto.";
                    VerMensaje = true;
                }
                else
                {
                    var vmExistencias = new ExistenciasViewModel
                    {
                        ListaExistencias = ListaExistencias,
                        TxtProducto = ListaExistencias[0].Descripcion
                    };
                    var vwExistencias = new ExistenciasView
                    {
                        DataContext = vmExistencias
                    };
                    var result = await DialogHost.Show(vwExistencias, "BuscarProductos");
                }
            }
            catch (Exception ex)
            {
                TxtMensaje = "Error: " + ex.Message;
                VerMensaje = true;
            }            
        }
        #endregion
    }
}
