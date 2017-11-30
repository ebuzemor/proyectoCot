using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Windows.Controls;

namespace Cotizador.ViewModel
{
    public class LoginViewModel : Notificador
    {
        #region Variables
        private Boolean _esValido;
        private String _txtLogin;
        private String _txtPassword;
        private String _mensaje;
        private String _titulo;
        private ApiToken _apiToken;
        private Usuario _usuario;
        private UsuariosJson _usuariosJson;
        private SucursalesJson _sucursalesJson;
        private ObservableCollection<Sucursal> _listaSucursales;
        private Sucursal _miSucursal;
        private String _localhost;
        private String _claveEF_Empresa;

        public RelayCommand ValidarUsuarioCommand { get; set; }

        public Boolean EsValido { get => _esValido; set { _esValido = value; OnPropertyChanged("EsValido"); } }
        public String TxtLogin { get => _txtLogin; set { _txtLogin = value; OnPropertyChanged("TxtLogin"); } }
        public String TxtPassword { get => _txtPassword; set { _txtPassword = value; OnPropertyChanged("TxtPassword"); } }
        public String Mensaje { get => _mensaje; set { _mensaje = value; OnPropertyChanged("Mensaje"); } }
        public String Titulo { get => _titulo; set { _titulo = value; OnPropertyChanged("Titulo"); } }
        public ApiToken ApiToken { get => _apiToken; set { _apiToken = value; OnPropertyChanged("ApiToken"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public UsuariosJson UsuariosJson { get => _usuariosJson; set { _usuariosJson = value; OnPropertyChanged("UsuariosJson"); } }        
        public SucursalesJson SucursalesJson { get => _sucursalesJson; set { _sucursalesJson = value; OnPropertyChanged("SucursalesJson"); } }
        public ObservableCollection<Sucursal> ListaSucursales { get => _listaSucursales; set { _listaSucursales = value; OnPropertyChanged("ListaSucursales"); } }
        public Sucursal MiSucursal { get => _miSucursal; set { _miSucursal = value; OnPropertyChanged("MiSucursal"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        public string ClaveEF_Empresa { get => _claveEF_Empresa; set { _claveEF_Empresa = value; OnPropertyChanged("ClaveEF_Empresa"); } }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            Titulo = "Iniciar Sesión";
            ValidarUsuarioCommand = new RelayCommand(ValidarUsuario);
            var appconfig = ConfigurationManager.GetSection("cfgApiRestCotizador") as NameValueCollection;
            Localhost = appconfig["localhost"].ToString();
            ClaveEF_Empresa = appconfig["claveEF_empresa"].ToString();
            string nombreUsuario = appconfig["api_user"].ToString();
            string password = appconfig["api_password"].ToString();
            ApiRest(nombreUsuario, password);
        }
        #endregion

        #region Metodos
        private void ValidarUsuario(object parameter)
        {
            PasswordBox pwbox = parameter as PasswordBox;
            TxtPassword = pwbox.Password;
            ValidarCredenciales(TxtLogin, TxtPassword);
            if (EsValido == true)
            {                
                InicioViewModel vmInicio = new InicioViewModel(ApiToken, Usuario, Localhost);
                InicioView vwInicio = new InicioView
                {
                    DataContext = vmInicio
                };
                Navigator.NavigationService.Navigate(vwInicio);
            }
        }

        private void ValidarCredenciales(String login, String password)
        {
            try
            {
                var rest = new RestClient(Localhost);
                var req = new RestRequest("buscarUsuario/" + ClaveEF_Empresa + "/" + MiSucursal.ClaveEntidadFiscalInmueble + "/" + TxtLogin + "/" + TxtPassword, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + ApiToken.Login.Token);

                IRestResponse<UsuariosJson> resp = rest.Execute<UsuariosJson>(req);
                if (resp.IsSuccessful)
                {
                    if (resp.StatusCode == HttpStatusCode.NoContent)
                    {
                        Mensaje = "ERROR DE VALIDACION: Verifique nombre de usuario y/o contraseña";
                        EsValido = false;
                    }
                    else
                    {
                        UsuariosJson = JsonConvert.DeserializeObject<UsuariosJson>(resp.Content);
                        Usuario = UsuariosJson.ListaUsuarios.First();                        
                        EsValido = true;
                    }
                }
                else 
                {
                    EsValido = false;
                    Mensaje = resp.StatusDescription;
                }
            }
            catch(Exception e)
            {
                EsValido = false;
                Mensaje = e.Message;
            }
        }

        private void ApiRest(String NombreUsuario, String Password)
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
                ApiToken = JsonConvert.DeserializeObject<ApiToken>(response.Content);

                var reqSuc = new RestRequest("listaSucursales/" + 100000205, Method.GET);
                reqSuc.AddHeader("Accept", "application/json");
                reqSuc.AddHeader("Authorization", "Bearer " + ApiToken.Login.Token);

                IRestResponse<SucursalesJson> respSuc = rest.Execute<SucursalesJson>(reqSuc);
                if (respSuc.IsSuccessful && respSuc.StatusCode == HttpStatusCode.OK)
                {
                    SucursalesJson = JsonConvert.DeserializeObject<SucursalesJson>(respSuc.Content);
                    ListaSucursales = new ObservableCollection<Sucursal>(SucursalesJson.Sucursales);
                }
                else
                    Mensaje = "(ERROR LISTA SUCURSALES): " + respSuc.ErrorMessage;
            }
            else
            {
                if (string.IsNullOrEmpty(response.ErrorMessage) == true)
                    Mensaje = "ERROR DE CONEXION: " + response.StatusDescription;
                else
                    Mensaje = "ERROR DE CONEXION: " + response.ErrorMessage;
            }
        }
        #endregion
    }
}
