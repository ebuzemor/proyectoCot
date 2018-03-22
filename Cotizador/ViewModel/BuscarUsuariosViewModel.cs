using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Data;

namespace Cotizador.ViewModel
{
    public class BuscarUsuariosViewModel : Notificador
    {
        #region Commands
        public RelayCommand BuscarUsuariosCommand { get; set; }
        public RelayCommand InicioCommand { get; set; }
        public RelayCommand AnteriorCommand { get; set; }
        public RelayCommand SiguienteCommand { get; set; }
        public RelayCommand FinalCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        #endregion

        #region Variables
        private ApiKey _appKey;
        private Usuario _usuario;
        private DatosUsuariosJson _datosUsuariosJson;
        private CollectionViewSource _cvsDatosUsuarios;
        private string _localhost;
        private ObservableCollection<DatosUsuarios> _listaDatosUsuarios;
        private string _txtUsuario;
        private int _itemsPorPag;
        private int _pagsTotales;
        private int _indicePagActual;
        private int _pagActual;
        private bool _activoInicio;
        private bool _activoAnterior;
        private bool _activoSiguiente;
        private bool _activoFinal;
        private bool _verMensaje;
        private string _txtMensaje;
        private DatosUsuarios _datUsuario;

        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public DatosUsuariosJson DatosUsuariosJson { get => _datosUsuariosJson; set { _datosUsuariosJson = value; OnPropertyChanged(); } }
        public CollectionViewSource CvsDatosUsuarios { get => _cvsDatosUsuarios; set { _cvsDatosUsuarios = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public ObservableCollection<DatosUsuarios> ListaDatosUsuarios { get => _listaDatosUsuarios; set { _listaDatosUsuarios = value; OnPropertyChanged(); } }
        public string TxtUsuario { get => _txtUsuario; set { _txtUsuario = value; OnPropertyChanged(); } }
        public int ItemsPorPag { get => _itemsPorPag; set { _itemsPorPag = value; OnPropertyChanged(); } }
        public int PagsTotales { get => _pagsTotales; set { _pagsTotales = value; OnPropertyChanged(); } }
        public int IndicePagActual { get => _indicePagActual; set { _indicePagActual = value; OnPropertyChanged(); } }
        public int PagActual { get => _pagActual + 1; set { _pagActual = value; OnPropertyChanged(); } }
        public bool ActivoInicio { get => _activoInicio; set { _activoInicio = value; OnPropertyChanged(); } }
        public bool ActivoAnterior { get => _activoAnterior; set { _activoAnterior = value; OnPropertyChanged(); } }
        public bool ActivoSiguiente { get => _activoSiguiente; set { _activoSiguiente = value; OnPropertyChanged(); } }
        public bool ActivoFinal { get => _activoFinal; set { _activoFinal = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public DatosUsuarios DatUsuario { get => _datUsuario; set { _datUsuario = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public BuscarUsuariosViewModel()
        {
            BuscarUsuariosCommand = new RelayCommand(BuscarUsuarios);
            InicioCommand = new RelayCommand(Inicio);
            AnteriorCommand = new RelayCommand(Anterior);
            SiguienteCommand = new RelayCommand(Siguiente);
            FinalCommand = new RelayCommand(Final);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            ItemsPorPag = 10;
        }
        #endregion

        #region Métodos
        private void BuscarUsuarios(object parameter)
        {
            if(string.IsNullOrEmpty(TxtUsuario) != true)
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("obtenerListaUsuarios/" + TxtUsuario, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse<DatosUsuariosJson> resp = rest.Execute<DatosUsuariosJson>(req);
                if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
                {
                    DatosUsuariosJson = JsonConvert.DeserializeObject<DatosUsuariosJson>(resp.Content);
                    ListaDatosUsuarios = new ObservableCollection<DatosUsuarios>(DatosUsuariosJson.ListaDatosUsuarios);
                    ///Paginación de los resultados
                    CvsDatosUsuarios = new CollectionViewSource
                    {
                        Source = ListaDatosUsuarios
                    };
                    CvsDatosUsuarios.Filter += new FilterEventHandler(FiltroPaginas);
                }
                if (ListaDatosUsuarios != null && ListaDatosUsuarios.Count == 0)
                {
                    TxtMensaje = "La búsqueda no obtuvo resultados, intente de nuevo.";
                    VerMensaje = true;
                }
            }
            else
            {
                TxtMensaje = "Debe escribir algo para realizar la búsqueda de Usuarios.";
                VerMensaje = true;
            }
            IndicePagActual = 0;
            ActualizarPagina();
            CalcularPagsTotales();
            ActivarBotones();
        }

        private void ActivarBotones()
        {
            if (ListaDatosUsuarios != null && ListaDatosUsuarios.Count > 0)
            {
                ActivoInicio = (IndicePagActual != 0) ? true : false;
                ActivoAnterior = (IndicePagActual != 0) ? true : false;
                ActivoSiguiente = (IndicePagActual < PagsTotales - 1) ? true : false;
                ActivoFinal = (PagActual != PagsTotales) ? true : false;
            }
        }

        private void CalcularPagsTotales()
        {
            if (ListaDatosUsuarios != null)
            {
                if (ListaDatosUsuarios.Count % ItemsPorPag == 0)
                    PagsTotales = ListaDatosUsuarios.Count / ItemsPorPag;
                else
                    PagsTotales = (ListaDatosUsuarios.Count / ItemsPorPag) + 1;
            }
        }

        private void FiltroPaginas(object sender, FilterEventArgs e)
        {
            int index = ListaDatosUsuarios.IndexOf((DatosUsuarios)e.Item);
            if (index >= ItemsPorPag * IndicePagActual && index < ItemsPorPag * (IndicePagActual + 1))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void ActualizarPagina()
        {
            if (CvsDatosUsuarios != null)
            {
                PagActual = IndicePagActual;
                CvsDatosUsuarios.View.Refresh();
                ActivarBotones();
            }
        }

        private void Inicio(object parameter)
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

        private void CerrarMensaje(object parameter) => VerMensaje = false;
        #endregion
    }
}
