using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Data;

namespace Cotizador.ViewModel
{
    public class BuscarFichaTecnicaViewModel : Notificador
    {
        #region Commands
        public RelayCommand BuscarProductosCommand { get; set; }
        public RelayCommand InicioCommand { get; set; }
        public RelayCommand AnteriorCommand { get; set; }
        public RelayCommand SiguienteCommand { get; set; }
        public RelayCommand FinalCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        public RelayCommand ExistenciasProductoCommand { get; set; }
        #endregion

        #region Variables
        private ObservableCollection<Producto> _listaProductos;
        private ObservableCollection<Existencias> _listaExistencias;
        private Producto _nvoProducto;
        private ProductosJson _productosJson;
        private ApiKey _appKey;
        private Usuario _usuario;
        private CollectionViewSource _cvsProductos;
        private string _txtProducto;
        private string _localhost;
        private int _pagsTotales;
        private int _indicePagActual;
        private int _itemsPorPag;
        private int _pagActual;
        private bool _activoInicio;
        private bool _activoAnterior;
        private bool _activoSiguiente;
        private bool _activoFinal;
        private bool _verMensaje;
        private string _txtMensaje;

        public ObservableCollection<Producto> ListaProductos { get => _listaProductos; set { _listaProductos = value; OnPropertyChanged(); } }
        public ObservableCollection<Existencias> ListaExistencias { get => _listaExistencias; set { _listaExistencias = value; OnPropertyChanged(); } }
        public Producto NvoProducto { get => _nvoProducto; set { _nvoProducto = value; OnPropertyChanged(); } }
        public ProductosJson ProductosJson { get => _productosJson; set { _productosJson = value; OnPropertyChanged(); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public CollectionViewSource CvsProductos { get => _cvsProductos; set { _cvsProductos = value; OnPropertyChanged(); } }
        public string TxtProducto { get => _txtProducto; set { _txtProducto = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public int PagsTotales { get => _pagsTotales; set { _pagsTotales = value; OnPropertyChanged(); } }
        public int IndicePagActual { get => _indicePagActual; set { _indicePagActual = value; OnPropertyChanged(); } }
        public int ItemsPorPag { get => _itemsPorPag; set { _itemsPorPag = value; OnPropertyChanged(); } } 
        public int PagActual { get => _pagActual + 1; set { _pagActual = value; OnPropertyChanged(); } }
        public bool ActivoInicio { get => _activoInicio; set { _activoInicio = value; OnPropertyChanged(); } }
        public bool ActivoAnterior { get => _activoAnterior; set { _activoAnterior = value; OnPropertyChanged(); } }
        public bool ActivoSiguiente { get => _activoSiguiente; set { _activoSiguiente = value; OnPropertyChanged(); } }
        public bool ActivoFinal { get => _activoFinal; set { _activoFinal = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public BuscarFichaTecnicaViewModel()
        {
            BuscarProductosCommand = new RelayCommand(BuscarProductos);
            InicioCommand = new RelayCommand(Inicio);
            AnteriorCommand = new RelayCommand(Anterior);
            SiguienteCommand = new RelayCommand(Siguiente);
            FinalCommand = new RelayCommand(Final);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            ExistenciasProductoCommand = new RelayCommand(ExistenciasProducto);
            IndicePagActual = 0;
            ItemsPorPag = 10;
        }
        #endregion

        #region Metodos
        private void BuscarProductos(object parameter)
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

        private void Inicio(object parameter)
        {
            IndicePagActual = 0;
            ActualizarPagina();
        }

        private void Anterior(object parameter)
        {
            IndicePagActual -= 1;
            ActualizarPagina();
        }

        private void Siguiente(object parameter)
        {
            IndicePagActual += 1;
            ActualizarPagina();
        }

        private void Final(object parameter)
        {
            IndicePagActual = PagsTotales - 1;
            ActualizarPagina();
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;

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
                    var result = await DialogHost.Show(vwExistencias, "BuscarFichaTecnica");
                }
            }
            catch (Exception ex)
            {
                TxtMensaje = "Error: " + ex.Message;
                VerMensaje = true;
            }
        }

        private void ActualizarPagina()
        {
            PagActual = IndicePagActual;
            CvsProductos.View.Refresh();
            ActivarBotones();
        }

        private void CalcularPagsTotales()
        {
            if (ListaProductos.Count % ItemsPorPag == 0)
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
        #endregion
    }
}