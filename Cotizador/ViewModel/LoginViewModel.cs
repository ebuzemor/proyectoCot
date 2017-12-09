﻿using Cotizador.Common;
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
using System.Xml;

namespace Cotizador.ViewModel
{
    public class LoginViewModel : Notificador
    {
        #region Commands
        public RelayCommand ValidarUsuarioCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        #endregion

        #region Variables
        private Boolean _esValido;
        private String _txtLogin;
        private String _txtPassword;
        private String _titulo;
        private ApiKey _appKey;
        private Usuario _usuario;
        private UsuariosJson _usuariosJson;
        private SucursalesJson _sucursalesJson;
        private ObservableCollection<Sucursal> _listaSucursales;
        private Sucursal _miSucursal;
        private String _localhost;
        private String _claveEF_Empresa;
        private Boolean _verMensaje;
        private String _txtMensaje;

        public Boolean EsValido { get => _esValido; set { _esValido = value; OnPropertyChanged("EsValido"); } }
        public String TxtLogin { get => _txtLogin; set { _txtLogin = value; OnPropertyChanged("TxtLogin"); } }
        public String TxtPassword { get => _txtPassword; set { _txtPassword = value; OnPropertyChanged("TxtPassword"); } }
        public String Titulo { get => _titulo; set { _titulo = value; OnPropertyChanged("Titulo"); } }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public UsuariosJson UsuariosJson { get => _usuariosJson; set { _usuariosJson = value; OnPropertyChanged("UsuariosJson"); } }        
        public SucursalesJson SucursalesJson { get => _sucursalesJson; set { _sucursalesJson = value; OnPropertyChanged("SucursalesJson"); } }
        public ObservableCollection<Sucursal> ListaSucursales { get => _listaSucursales; set { _listaSucursales = value; OnPropertyChanged("ListaSucursales"); } }
        public Sucursal MiSucursal { get => _miSucursal; set { _miSucursal = value; OnPropertyChanged("MiSucursal"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        public string ClaveEF_Empresa { get => _claveEF_Empresa; set { _claveEF_Empresa = value; OnPropertyChanged("ClaveEF_Empresa"); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged("VerMensaje"); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged("TxtMensaje"); } }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            Titulo = "Iniciar Sesión";
            ValidarUsuarioCommand = new RelayCommand(ValidarUsuario);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
            var appconfig = ConfigurationManager.GetSection("cfgApiRestCotizador") as NameValueCollection;
            Localhost = appconfig["localhost"].ToString();
            ClaveEF_Empresa = appconfig["claveEF_empresa"].ToString();
            string nombreUsuario = appconfig["api_user"].ToString();
            string password = appconfig["api_password"].ToString();
            string apikey = appconfig["api_key"].ToString();
            //verifica que exista un token, en caso contrario genera uno nuevo
            if (string.IsNullOrEmpty(apikey) != true)
            {
                AppKey = new ApiKey(apikey);
            }
            else
            {
                ObtenerToken(nombreUsuario, password);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                foreach (XmlElement element in xmlDoc.DocumentElement)
                {
                    if (element.Name.Equals("cfgApiRestCotizador"))
                    {
                        foreach (XmlNode node in element.ChildNodes)
                        {
                            if (node.Attributes[0].Value.Equals("api_key"))
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
        #endregion

        #region Metodos
        private void ValidarUsuario(object parameter)
        {
            PasswordBox pwbox = parameter as PasswordBox;
            TxtPassword = pwbox.Password;
            ValidarCredenciales(TxtLogin, TxtPassword);
            if (EsValido == true)
            {                
                InicioViewModel vmInicio = new InicioViewModel(AppKey, Usuario, Localhost);
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
                        Usuario = UsuariosJson.ListaUsuarios.First();                        
                        EsValido = true;
                    }
                }
                else 
                {
                    EsValido = false;
                    TxtMensaje = resp.StatusDescription;
                    VerMensaje = true;
                }
            }
            catch(Exception)
            {
                EsValido = false;
                TxtMensaje = "ERROR DE VALIDACION: Para iniciar sesión debe escribir nombre de usuario, password y elegir una sucursal.";
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
            var rest = new RestClient(Localhost);
            var reqSuc = new RestRequest("listaSucursales/" + ClaveEF_Empresa, Method.GET);
            reqSuc.AddHeader("Accept", "application/json");
            reqSuc.AddHeader("Authorization", "Bearer " + AppKey.Token);

            IRestResponse<SucursalesJson> respSuc = rest.Execute<SucursalesJson>(reqSuc);
            if (respSuc.IsSuccessful && respSuc.StatusCode == HttpStatusCode.OK)
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

        private void CerrarMensaje(object parameter) => VerMensaje = false;
        #endregion
    }
}
