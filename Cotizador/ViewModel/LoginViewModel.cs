using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Windows.Controls;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace Cotizador.ViewModel
{
    public class LoginViewModel : Notificador
    {
        #region Commands
        public RelayCommand ValidarUsuarioCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        #endregion

        #region Variables
        private ApiKey _appKey;
        private Usuario _usuario;
        private UsuariosJson _usuariosJson;
        private SucursalesJson _sucursalesJson;
        private ObservableCollection<Sucursal> _listaSucursales;
        private Sucursal _miSucursal;
        private List<Permisos> _listaPermisos;
        private ObservableCollection<AccionesDefinidas> _listaAcciones;
        private bool _esValido;
        private string _txtLogin;
        private string _txtPassword;
        private string _titulo;
        private string _localhost;
        private string _claveEF_Empresa;
        private string _token;
        private bool _verMensaje;
        private string _txtMensaje;
        private string _nombreUsuario;
        private string _password;
        private string _nombreToken;

        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public UsuariosJson UsuariosJson { get => _usuariosJson; set { _usuariosJson = value; OnPropertyChanged(); } }
        public SucursalesJson SucursalesJson { get => _sucursalesJson; set { _sucursalesJson = value; OnPropertyChanged(); } }
        public ObservableCollection<Sucursal> ListaSucursales { get => _listaSucursales; set { _listaSucursales = value; OnPropertyChanged(); } }
        public Sucursal MiSucursal { get => _miSucursal; set { _miSucursal = value; OnPropertyChanged(); } }
        public List<Permisos> ListaPermisos { get => _listaPermisos; set => _listaPermisos = value; }
        public ObservableCollection<AccionesDefinidas> ListaAcciones { get => _listaAcciones; set { _listaAcciones = value; OnPropertyChanged(); } }
        public bool EsValido { get => _esValido; set { _esValido = value; OnPropertyChanged(); } }
        public string TxtLogin { get => _txtLogin; set { _txtLogin = value; OnPropertyChanged(); } }
        public string TxtPassword { get => _txtPassword; set { _txtPassword = value; OnPropertyChanged(); } }
        public string Titulo { get => _titulo; set { _titulo = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public string ClaveEF_Empresa { get => _claveEF_Empresa; set { _claveEF_Empresa = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public string Token { get => _token; set { _token = value; OnPropertyChanged(); } }
        public string NombreUsuario { get => _nombreUsuario; set { _nombreUsuario = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string NombreToken { get => _nombreToken; set { _nombreToken = value; OnPropertyChanged(); } }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
            try
            {
                Titulo = "Iniciar Sesión";
                ValidarUsuarioCommand = new RelayCommand(ValidarUsuario);
                CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
                TipoConexion("desarrollo");
                ///verifica que exista un token, en caso contrario genera uno nuevo
                if (string.IsNullOrEmpty(Token) != true)
                {
                    AppKey = new ApiKey(Token);
                }
                else
                {
                    ObtenerToken(NombreUsuario, Password);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    foreach (XmlElement element in xmlDoc.DocumentElement)
                    {
                        if (element.Name.Equals("cfgApiRestCotizador"))
                        {
                            foreach (XmlNode node in element.ChildNodes)
                            {
                                if (node.Attributes[0].Value.Equals(NombreToken))
                                {
                                    node.Attributes[1].Value = AppKey.Token; break;
                                }
                            }
                            break;
                        }
                    }
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("cfgApiRestCotizador");
                }
                CargarSucursales();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
        #endregion

        #region Metodos
        private async void ValidarUsuario(object parameter)
        {
            PasswordBox pwbox = parameter as PasswordBox;
            TxtPassword = pwbox.Password;
            ValidarCredenciales(TxtLogin, TxtPassword);
            if (EsValido == true)
            {
                if (UsuariosJson.ListaUsuarios.Count > 1)
                {
                    var vmSeleccionar = new SeleccionarSucursalViewModel
                    {
                        ListaUsuarios = UsuariosJson.ListaUsuarios
                    };
                    var vwSeleccionar = new SeleccionarSucursalView
                    {
                        DataContext = vmSeleccionar
                    };
                    var result = await DialogHost.Show(vwSeleccionar, "LoginView");
                    if (result.Equals("OK") == true)
                    {
                        Usuario = vmSeleccionar.UsuarioSel;
                    }
                }
                else
                {
                    Usuario = UsuariosJson.ListaUsuarios[0];
                }
                if (Usuario != null)
                {
                    ValidarPermisosUsuario();
                    if (ListaPermisos == null)
                    {
                        var vmMsj = new MensajeViewModel
                        {
                            CuerpoMensaje = "El usuario no tiene un esquema de privilegios asignado.",
                            TituloMensaje = "Error",
                            MostrarCancelar = false
                        };
                        var vwMsj = new MensajeView
                        {
                            DataContext = vmMsj
                        };
                        var result = await DialogHost.Show(vwMsj, "LoginView");
                    }
                    else
                    {
                        InicioViewModel vmInicio = new InicioViewModel(AppKey, Usuario, Localhost, ListaAcciones);
                        InicioView vwInicio = new InicioView
                        {
                            DataContext = vmInicio
                        };
                        Navigator.NavigationService.Navigate(vwInicio);
                    }
                }
            }
        }

        private void ValidarCredenciales(String login, String password)
        {
            try
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("buscarUsuario/" + ClaveEF_Empresa + "/" + TxtLogin + "/" + TxtPassword, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse<UsuariosJson> resp = rest.Execute<UsuariosJson>(req);
                if (resp.IsSuccessful)
                {
                    if (resp.StatusCode == HttpStatusCode.NoContent)
                    {
                        TxtMensaje = "ERROR DE VALIDACION: Verifique nombre de usuario y/o contraseña";
                        EsValido = false;
                        VerMensaje = true;
                    }
                    else
                    {
                        UsuariosJson = JsonConvert.DeserializeObject<UsuariosJson>(resp.Content);
                        EsValido = true;
                    }
                }
                else 
                {
                    EsValido = false;
                    TxtMensaje = "ERROR DE WEBSERVICE: Ocurrió un error al ejecutar el WebService para validar credenciales de Usuario. Contacte al administrador del sistema";
                    VerMensaje = true;
                }
            }
            catch(Exception)
            {
                EsValido = false;
                TxtMensaje = "ERROR DE EJECUCIÓN: Ocurrió una excepción al validar las credenciales del Usuario, reinicie la aplicación.";
                VerMensaje = true;
            }
        }        

        private void ObtenerToken(String NombreUsuario, String Password)
        {
            var rest = new RestClient(Localhost)
            {
                Authenticator = new SimpleAuthenticator("name", NombreUsuario, "password", Password)
            };

            var request = new RestRequest("login", Method.POST);
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = rest.Execute(request);
            if (response.IsSuccessful)
            {
                AppKey = JsonConvert.DeserializeObject<ApiKey>(response.Content);
            }
            else
            {
                if (string.IsNullOrEmpty(response.ErrorMessage) == true)
                    TxtMensaje = "ERROR DE CONEXION: " + response.StatusDescription;
                else
                    TxtMensaje = "ERROR DE CONEXION: " + response.ErrorMessage;
                VerMensaje = true;
            }
        }

        private void CargarSucursales()
        {
            try
            {
                var rest = new RestClient(Localhost);
                var reqSuc = new RestRequest("listaSucursales/" + ClaveEF_Empresa, Method.GET);
                reqSuc.AddHeader("Accept", "application/json");
                reqSuc.AddHeader("Authorization", "Bearer " + AppKey.Token);

                IRestResponse<SucursalesJson> respSuc = rest.Execute<SucursalesJson>(reqSuc);
                if (respSuc.IsSuccessful == true && respSuc.StatusCode == HttpStatusCode.OK)
                {
                    SucursalesJson = JsonConvert.DeserializeObject<SucursalesJson>(respSuc.Content);
                    ListaSucursales = new ObservableCollection<Sucursal>(SucursalesJson.Sucursales);
                }
                else
                {
                    if (respSuc.StatusCode == HttpStatusCode.Unauthorized)
                        TxtMensaje = "ERROR DE CONEXION: Acesso no autorizado, verifique token";
                    else
                        TxtMensaje = "ERROR DE CONEXION: " + respSuc.ErrorMessage;
                    VerMensaje = true;
                }
            }
            catch (Exception) { }
        }

        private void CargarPermisosUsuario()
        {
            try
            {
                var rest = new RestClient(Localhost);
                var reqPer = new RestRequest("permisosUsuario/" + ClaveEF_Empresa + "/" + Usuario.ClaveEntidadFiscalEmpleado, Method.GET);
                reqPer.AddHeader("Accept", "application/json");
                reqPer.AddHeader("Authorization", "Bearer " + AppKey.Token);
                IRestResponse<PermisosJson> respPer = rest.Execute<PermisosJson>(reqPer);
                if (respPer.IsSuccessful == true && respPer.StatusCode == HttpStatusCode.OK)
                {
                    var PermisosJson = JsonConvert.DeserializeObject<PermisosJson>(respPer.Content);
                    ListaPermisos = PermisosJson.Permisos;
                }
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

        private void CerrarMensaje(object parameter) => VerMensaje = false;

        private void TipoConexion(string tipoCxn)
        {
            var appconfig = ConfigurationManager.GetSection("cfgApiRestCotizador") as NameValueCollection;
            ClaveEF_Empresa = appconfig["claveEF_empresa"].ToString();
            NombreUsuario = appconfig["api_user"].ToString();
            Password = appconfig["api_password"].ToString();
            switch (tipoCxn)
            {
                case "demo":
                    Localhost = appconfig["demo"].ToString();
                    Token = appconfig["api_key_demo"].ToString();
                    NombreToken = "api_key_demo";
                    break;
                case "desarrollo":
                    Localhost = appconfig["localhost"].ToString();
                    Token = appconfig["api_key"].ToString();
                    NombreToken = "api_key";
                    break;
                case "produccion":
                    Localhost = appconfig["produccion"].ToString();
                    Token = appconfig["api_key_prod"].ToString();
                    NombreToken = "api_key_prod";
                    break;
            }
        }
        #endregion
    }
}
