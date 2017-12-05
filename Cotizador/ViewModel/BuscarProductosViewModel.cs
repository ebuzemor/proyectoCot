using Cotizador.Common;
using Cotizador.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Data;

namespace Cotizador.ViewModel
{
    public class BuscarProductosViewModel : Notificador
    {
        #region Variables
        private ObservableCollection<Producto> _listaProductos;
        private Producto _nvoProducto;
        private ProductoSeleccionado _selProducto;
        private ProductosJson _productosJson;
        private String _txtProducto;
        //private String _txtFiltrar;
        private ApiKey _appKey;
        private Usuario _usuario;
        private ICollectionView _icvProductos;
        private String _localhost;
        private CollectionViewSource _cvsProductos;
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

        public ObservableCollection<Producto> ListaProductos { get => _listaProductos; set { _listaProductos = value; OnPropertyChanged("ListaProductos"); } }        
        public ProductosJson ProductosJson { get => _productosJson; set { _productosJson = value; OnPropertyChanged("ProdutosJson"); } }
        public string TxtProducto { get => _txtProducto; set { _txtProducto = value; OnPropertyChanged("TxtProducto"); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public ICollectionView IcvProductos { get => _icvProductos; set { _icvProductos = value; OnPropertyChanged("CvProductos"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        ///Filtra resultados mientras se escribe 1/2
        //public string TxtFiltrar
        //{
        //    get => _txtFiltrar;
        //    set
        //    {
        //        if(value!=_txtFiltrar)
        //        {
        //            _txtFiltrar = value;
        //            IcvProductos.Refresh();        
        //            OnPropertyChanged("TxtFiltrar");
        //        }                
        //    }
        //}

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
                _txtCantidad = value;
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
        #endregion

        #region Commands
        public RelayCommand BuscarProductoCommand { get; set; }
        public RelayCommand SeleccionadoCommand { get; set; }
        public RelayCommand InicioCommand { get; set; }
        public RelayCommand AnteriorCommand { get; set; }
        public RelayCommand SiguienteCommand { get; set; }
        public RelayCommand FinalCommand { get; set; }        
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
                var rest = new RestClient(Localhost);
                var req = new RestRequest("buscarExistencias/" + Usuario.ClaveEntidadFiscalInmueble + "/" + Usuario.ClaveEntidadFiscalEmpresa + "/" + TxtProducto, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse<ProductosJson> resp = rest.Execute<ProductosJson>(req);
                if (resp.IsSuccessful)
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        ProductosJson = JsonConvert.DeserializeObject<ProductosJson>(resp.Content);
                        ListaProductos = new ObservableCollection<Producto>(ProductosJson.Productos);
                        ///Filtra resultados mientras se escribe 2/2
                        //IcvProductos = CollectionViewSource.GetDefaultView(ListaProductos);
                        //IcvProductos.Filter = (x => String.IsNullOrEmpty(TxtFiltrar) ? true : ((Producto)x).Descripcion.Contains(TxtFiltrar.ToUpper()));

                        ///Paginación de los resultados
                        CvsProductos = new CollectionViewSource
                        {
                            Source = ListaProductos
                        };
                        CvsProductos.Filter += new FilterEventHandler(FiltroPaginas);                    
                        IndicePagActual = 0;
                        CalcularPagsTotales();
                        ActivarBotones();
                    }
                }
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
            ActivoInicio = (IndicePagActual != 0) ? true : false;
            ActivoAnterior = (IndicePagActual != 0) ? true : false;
            ActivoSiguiente = (IndicePagActual < PagsTotales - 1) ? true : false;
            ActivoFinal = (PagActual != PagsTotales) ? true : false;
        }

        private void CalcularImporte(double? cantidad)
        {
            try
            {
                if (cantidad > 0)
                    if (NvoProducto.EsFraccionable == 0)
                        TxtImporte = Convert.ToInt32(cantidad) * NvoProducto.PrecioUnitario;
                    else
                        TxtImporte = Convert.ToDouble(cantidad) * NvoProducto.PrecioUnitario;
                else
                    TxtImporte = 0;
                SeleccionarProducto();
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
                if (descuento > 0 && TxtCantidad > 0)
                {
                    TxtImporteDesc = Convert.ToDouble(descuento) * NvoProducto.PrecioUnitario * TxtCantidad;
                    TxtImporte = NvoProducto.PrecioUnitario - TxtImporteDesc;
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
            catch (Exception)
            {
                TxtImporte = 0;
                TxtImporteDesc = 0;
            }
        }

        private void ActivarBtnSeleccionar()
        {
            ActivoSeleccionar = (TxtCantidad > 0) ? true : false;
        }

        private void SeleccionarProducto()
        {
            SelProducto.Producto = NvoProducto;
            SelProducto.Cantidad = TxtCantidad;
            SelProducto.Descuento = TxtDescuento;
            SelProducto.ImporteDesc = TxtImporteDesc;
            SelProducto.Importe = TxtImporte;
            double impuestos = Math.Round(TxtImporte * (SelProducto.Producto.SumaImpuestos / 100.0), 2);
            SelProducto.Impuesto = impuestos;
            SelProducto.SubTotal = Math.Round(TxtImporte + impuestos, 2);
        }
        #endregion
    }
}
