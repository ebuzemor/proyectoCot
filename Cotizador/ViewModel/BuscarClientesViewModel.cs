using Cotizador.Common;
using Cotizador.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Data;

namespace Cotizador.ViewModel
{
    public class BuscarClientesViewModel : Notificador
    {
        #region Commands
        public RelayCommand BuscarClienteCommand { get; set; }
        public RelayCommand SelectedCommand { get; set; }
        public RelayCommand InicioCommand { get; set; }
        public RelayCommand AnteriorCommand { get; set; }
        public RelayCommand SiguienteCommand { get; set; }
        public RelayCommand FinalCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        #endregion

        #region Variables
        private ObservableCollection<Cliente> _listaClientes;
        private Cliente _NvoCliente;
        private String _TxtCliente;
        private ApiKey _appKey;
        private Usuario _usuario;
        private ClientesJson _clientesJson;
        private CollectionViewSource _cvsClientes;
        private String _localhost;
        private int _itemsPorPag;
        private int _pagsTotales;
        private int _indicePagActual;
        private int _pagActual;
        private Boolean _activoInicio;
        private Boolean _activoAnterior;
        private Boolean _activoSiguiente;
        private Boolean _activoFinal;
        private Boolean _verMensaje;
        private String _txtMensaje;

        public ObservableCollection<Cliente> ListaClientes { get => _listaClientes; set { _listaClientes = value; OnPropertyChanged(); } }
        public Cliente NvoCliente { get => _NvoCliente; set { _NvoCliente = value; OnPropertyChanged(); } }
        public string TxtCliente { get => _TxtCliente; set { _TxtCliente = value; OnPropertyChanged(); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public ClientesJson ClientesJson { get => _clientesJson; set { _clientesJson = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }    
        public CollectionViewSource CvsClientes { get => _cvsClientes; set { _cvsClientes = value; OnPropertyChanged(); } }
        public int ItemsPorPag { get => _itemsPorPag; set => _itemsPorPag = value; }
        public int PagsTotales { get => _pagsTotales; set { _pagsTotales = value; OnPropertyChanged(); } }
        public int IndicePagActual { get => _indicePagActual; set { _indicePagActual = value; OnPropertyChanged(); } }
        public int PagActual { get => _pagActual + 1; set { _pagActual = value; OnPropertyChanged(); } }
        public bool ActivoInicio { get => _activoInicio; set { _activoInicio = value; OnPropertyChanged(); } }
        public bool ActivoAnterior { get => _activoAnterior; set { _activoAnterior = value; OnPropertyChanged(); } }
        public bool ActivoSiguiente { get => _activoSiguiente; set { _activoSiguiente = value; OnPropertyChanged(); } }
        public bool ActivoFinal { get => _activoFinal; set { _activoFinal = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        #endregion


        #region Constructor
        public BuscarClientesViewModel()
        {
            BuscarClienteCommand = new RelayCommand(BuscarCliente);
            InicioCommand = new RelayCommand(Inicio);
            AnteriorCommand = new RelayCommand(Anterior);
            SiguienteCommand = new RelayCommand(Siguiente);
            FinalCommand = new RelayCommand(Final);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            ItemsPorPag = 10;
        }        
        #endregion

        #region Metodos
        private void BuscarCliente(object parameter)
        {
            if (string.IsNullOrEmpty(TxtCliente) != true)
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("buscarClientes/" + Usuario.ClaveEntidadFiscalEmpresa + "/" + TxtCliente, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse<ClientesJson> resp = rest.Execute<ClientesJson>(req);
                if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
                {
                    ClientesJson = JsonConvert.DeserializeObject<ClientesJson>(resp.Content);
                    ListaClientes = new ObservableCollection<Cliente>(ClientesJson.Clientes);
                    ///Paginación de los resultados
                    CvsClientes = new CollectionViewSource
                    {
                        Source = ListaClientes
                    };
                    CvsClientes.Filter += new FilterEventHandler(FiltroPaginas);                                        
                }
                if (ListaClientes != null && ListaClientes.Count == 0)
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

        private void ActivarBotones()
        {
            if (ListaClientes != null && ListaClientes.Count > 0)
            {
                ActivoInicio = (IndicePagActual != 0) ? true : false;
                ActivoAnterior = (IndicePagActual != 0) ? true : false;
                ActivoSiguiente = (IndicePagActual < PagsTotales - 1) ? true : false;
                ActivoFinal = (PagActual != PagsTotales) ? true : false;
            }
        }

        private void CalcularPagsTotales()
        {
            if (ListaClientes != null)
            {
                if (ListaClientes.Count % ItemsPorPag == 0)
                    PagsTotales = ListaClientes.Count / ItemsPorPag;
                else
                    PagsTotales = (ListaClientes.Count / ItemsPorPag) + 1;
            }
        }

        private void FiltroPaginas(object sender, FilterEventArgs e)
        {
            int index = ListaClientes.IndexOf((Cliente)e.Item);
            if (index >= ItemsPorPag * IndicePagActual && index < ItemsPorPag * (IndicePagActual + 1))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
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
            if (CvsClientes != null)
            {
                PagActual = IndicePagActual;
                CvsClientes.View.Refresh();
                ActivarBotones();
            }
        }

        private void CerrarMensaje(object paramter) => VerMensaje = false;
        #endregion
    }
}
