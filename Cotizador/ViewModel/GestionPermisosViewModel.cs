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
    public class GestionPermisosViewModel : Notificador
    {
        #region Commands
        public RelayCommand BuscarUsuarioCommand { get; set; }
        public RelayCommand GuardarPermisosCommand { get; set; }
        public RelayCommand ResetFormularioCommand { get; set; }
        #endregion

        #region Variables
        private ObservableCollection<AccionesDefinidas> _listaAcciones;
        private ObservableCollection<AccionesDefinidas> _permisosAplicacion;
        private ApiKey _appKey;
        private string _localhost;
        private Usuario _usuario;
        private DatosUsuarios _datUsuario;
        private string _infoUsuario;
        private List<Permisos> _listaPermisos;
        private string _txtMensaje;
        private bool _verMensaje;

        public ObservableCollection<AccionesDefinidas> ListaAcciones { get => _listaAcciones; set { _listaAcciones = value; OnPropertyChanged(); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public DatosUsuarios DatUsuario { get => _datUsuario; set { _datUsuario = value; OnPropertyChanged(); } }
        public string InfoUsuario { get => _infoUsuario; set { _infoUsuario = value; OnPropertyChanged(); } }
        public List<Permisos> ListaPermisos { get => _listaPermisos; set { _listaPermisos = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public ObservableCollection<AccionesDefinidas> PermisosAplicacion { get => _permisosAplicacion; set { _permisosAplicacion = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public GestionPermisosViewModel()
        {
            BuscarUsuarioCommand = new RelayCommand(BuscarUsuario);
            GuardarPermisosCommand = new RelayCommand(GuardarPermisos);
            ResetFormularioCommand = new RelayCommand(ResetFormulario);
        }
        #endregion

        #region Métodos
        private async void GuardarPermisos(object parameter)
        {
            string tituloMsj = "";
            string cuerpoMsj = "";
            if (ListaAcciones != null && ListaAcciones.Count > 0)
            {
                string lista = JsonConvert.SerializeObject(ListaAcciones);
                UsuarioPermisos usrpermisos = new UsuarioPermisos
                {
                    ClaveEFEmpresa = Usuario.ClaveEntidadFiscalEmpresa,
                    ClaveEFUsuario = DatUsuario.ClaveEntidadFiscalUsuario,
                    ListaPermisos = lista
                };
                var paramUsrPermisos = JsonConvert.SerializeObject(usrpermisos);
                var restclient = new RestClient(Localhost);
                var request = new RestRequest("guardarPermisos", Method.POST);
                request.AddHeader("Authorization", "Bearer " + AppKey.Token);
                request.AddHeader("Accept", "application/json");
                request.AddParameter("text/json", paramUsrPermisos, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                IRestResponse response = restclient.Execute(request);
                if (response.IsSuccessful == true && response.StatusCode == HttpStatusCode.OK)
                {
                    tituloMsj = "Aviso";
                    cuerpoMsj = "La información de los permisos han sido guardados correctamente";
                    LimpiarFormulario();
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    tituloMsj = "Advertencia";
                    cuerpoMsj = "La información de los permisos no fue guardada, por favor intente de nuevo";
                }
                else
                {
                    tituloMsj = "Error";
                    cuerpoMsj = "Hubo un problema en el servidor que no permitió guardar la información, notifique el error";
                }
            }
            else
            {
                tituloMsj = "Error";
                cuerpoMsj = "La lista de permisos está vacía, debe elegir un usuario y modificar sus permisos para poder guardar los cambios";
            }
            var vmMensaje = new MensajeViewModel
            {
                TituloMensaje = tituloMsj,
                CuerpoMensaje = cuerpoMsj,
                MostrarCancelar = false
            };
            var vwMensaje = new MensajeView
            {
                DataContext = vmMensaje
            };
            var dialog = await DialogHost.Show(vwMensaje, "GestionPermisos");
        }

        private async void ResetFormulario(object parameter)
        {
            var vmMensaje = new MensajeViewModel
            {
                TituloMensaje = "Advertencia",
                CuerpoMensaje = "¿Desea reiniciar el formulario?",
                MostrarCancelar = true
            };
            var vwMensaje = new MensajeView
            {
                DataContext = vmMensaje
            };
            var dialog = await DialogHost.Show(vwMensaje, "GestionPermisos");
            if (dialog.Equals("OK") == true)
            {
                LimpiarFormulario();
            }
        }

        private async void BuscarUsuario(object parameter)
        {
            var vmBuscar = new BuscarUsuariosViewModel
            {
                AppKey = AppKey,
                Localhost = Localhost,
                Usuario = Usuario
            };
            var vwBuscar = new BuscarUsuariosView
            {
                DataContext = vmBuscar
            };
            var result = await DialogHost.Show(vwBuscar, "GestionPermisos");
            if (result.Equals("DatUsuario") == true)
            {
                DatUsuario = vmBuscar.DatUsuario;
                InfoUsuario = "Nombre: " + DatUsuario.RazonSocial + " | Nickname: " + DatUsuario.Nickname;                
                ValidarPermisosUsuario();
            }
        }

        private void CargarPermisosUsuario()
        {
            try
            {
                var rest = new RestClient(Localhost);
                var reqPer = new RestRequest("permisosUsuario/" + Usuario.ClaveEntidadFiscalEmpresa + "/" + DatUsuario.ClaveEntidadFiscalUsuario, Method.GET);
                reqPer.AddHeader("Accept", "application/json");
                reqPer.AddHeader("Authorization", "Bearer " + AppKey.Token);
                IRestResponse<PermisosJson> respPer = rest.Execute<PermisosJson>(reqPer);
                if (respPer.IsSuccessful == true && respPer.StatusCode == HttpStatusCode.OK)
                {
                    var PermisosJson = JsonConvert.DeserializeObject<PermisosJson>(respPer.Content);
                    ListaPermisos = PermisosJson.Permisos;
                }
                else
                    ListaPermisos = new List<Permisos>();
            }
            catch (Exception)
            {
                TxtMensaje = "EXCEPCION AL CARGAR PERMISOS DE USUARIO";
                VerMensaje = true;
            }
        }

        private void CargarPermisosAplicacion()
        {
            try
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("cargarPermisos/100000005", Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                IRestResponse<AccionesDefinidasJson> resp = rest.Execute<AccionesDefinidasJson>(req);
                if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
                {
                    var accionesJson = JsonConvert.DeserializeObject<AccionesDefinidasJson>(resp.Content);
                    ListaAcciones = new ObservableCollection<AccionesDefinidas>(accionesJson.AccionesDefinidas);
                }
            }
            catch (Exception)
            {
                TxtMensaje = "EXCEPCION AL CARGAR PERMISOS DE APLICACION";
                VerMensaje = true;
            }
        }

        private void ValidarPermisosUsuario()
        {
            CargarPermisosAplicacion();
            CargarPermisosUsuario();
            if (ListaPermisos != null)
            {
                foreach (Permisos p in ListaPermisos)
                {
                    var accion = ListaAcciones.SingleOrDefault(x => x.ClaveSeccion == p.ClaveSeccion);
                    if (accion != null)
                        accion.Activo = true;
                }
            }
        }

        private void LimpiarFormulario()
        {
            InfoUsuario = string.Empty;
            ListaAcciones = new ObservableCollection<AccionesDefinidas>();
        }
        #endregion
    }
}
