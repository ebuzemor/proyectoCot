using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using System;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.ComponentModel;

namespace Cotizador.ViewModel
{
    public class InicioViewModel : Notificador
    {
        #region Commands
        public Action CerrarAction { get; set; }
        public RelayCommand CerrarSesionCommand { get; set; }
        public RelayCommand SalirAppCommand { get; set; }
        #endregion

        #region Variables
        private int _idVentana;
        private ApiKey _appKey;
        private Usuario _usuario;
        private String _localhost;
        private CotizadorView _vwCotizador;
        private CotizadorViewModel _vmCotizador;
        private BuscadorCotizacionesView _vwBuscadorCot;
        private BuscadorCotizacionesViewModel _vmBuscadorCot;

        public MenuOpciones[] MenuOpcion { get; set; }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        public CotizadorView VwCotizador { get => _vwCotizador; set { _vwCotizador = value; OnPropertyChanged("VwCotizador"); } }
        public CotizadorViewModel VmCotizador { get => _vmCotizador; set { _vmCotizador = value; OnPropertyChanged("VmCotizador"); } }
        public BuscadorCotizacionesView VwBuscadorCot { get => _vwBuscadorCot; set { _vwBuscadorCot = value; OnPropertyChanged("VwBuscadorCot"); } }
        public BuscadorCotizacionesViewModel VmBuscadorCot { get => _vmBuscadorCot; set { _vmBuscadorCot = value; OnPropertyChanged("VmBuscadorCot"); } }
        public int IdVentana { get => _idVentana; set { _idVentana = value; OnPropertyChanged("IdVentana"); } }
        #endregion

        #region Constructor
        public InicioViewModel(ApiKey apiKey, Usuario usuario, string localhost)
        {
            AppKey = apiKey;
            Usuario = usuario;
            Localhost = localhost;
            IdVentana = 0;
            //CargarMenuInicial();
            // COTIZADOR
            VmCotizador = new CotizadorViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost
            };
            //para no hacer un constructor con paso de parametros.
            VmCotizador.MostrarSucursal();
            VmCotizador.CargarEstatusCotizacion();
            VmCotizador.MostrarCondicionesComerciales();
            VwCotizador = new CotizadorView
            {
                DataContext = VmCotizador
            };
            //  BUSCADOR DE COTIZACIONES
            VmBuscadorCot = new BuscadorCotizacionesViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost
            };
            VmBuscadorCot.CargarEstatusCotizacion();
            VmBuscadorCot.CargarSucursales();
            VwBuscadorCot = new BuscadorCotizacionesView
            {
                DataContext = VmBuscadorCot
            };
            //  OPCIONES DEL MENU
            MenuOpcion = new[]
            {
                new MenuOpciones("Cotizador", VwCotizador),
                new MenuOpciones("Buscar Cotizaciones", VwBuscadorCot)
            };
            CerrarSesionCommand = new RelayCommand(CerrarSesion);
            SalirAppCommand = new RelayCommand(SalirApp);
        }
        #endregion

        #region Métodos
        public void CargarMenuInicial()
        {
            // COTIZADOR
            VmCotizador = new CotizadorViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost
            };
            //para no hacer un constructor con paso de parametros.
            VmCotizador.MostrarSucursal();
            VmCotizador.CargarEstatusCotizacion();
            VmCotizador.MostrarCondicionesComerciales();
            VwCotizador = new CotizadorView
            {
                DataContext = VmCotizador
            };
            //  BUSCADOR DE COTIZACIONES
            VmBuscadorCot = new BuscadorCotizacionesViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost
            };
            VmBuscadorCot.CargarEstatusCotizacion();
            VmBuscadorCot.CargarSucursales();
            VwBuscadorCot = new BuscadorCotizacionesView
            {
                DataContext = VmBuscadorCot
            };
            //  OPCIONES DEL MENU
            MenuOpcion = new[]
            {
                new MenuOpciones("Cotizador", VwCotizador),
                new MenuOpciones("Buscar Cotizaciones", VwBuscadorCot)
            };
            CerrarSesionCommand = new RelayCommand(CerrarSesion);
            SalirAppCommand = new RelayCommand(SalirApp);
        }

        private async void CerrarSesion(object parameter)
        {
            var vmMensaje = new MensajeViewModel
            {
                TituloMensaje = "ADVERTENCIA",
                CuerpoMensaje = "¿Desea cerrar sesión?",
                MostrarCancelar = true
            };
            var vwMensaje = new MensajeView
            {
                DataContext = vmMensaje
            };
            var result = await DialogHost.Show(vwMensaje, "Cotizador");
            if (result.Equals("ACEPTAR") == true)
            {
                Usuario = null;
                LoginView login = new LoginView();
                Navigator.NavigationService.Navigate(login);
            }
        }

        private async void SalirApp(object parameter)
        {
            var vmMensaje = new MensajeViewModel
            {
                TituloMensaje = "ADVERTENCIA",
                CuerpoMensaje = "¿Desea salir de la aplicación?",
                MostrarCancelar = true
            };
            var vwMensaje = new MensajeView
            {
                DataContext = vmMensaje
            };
            var result = await DialogHost.Show(vwMensaje, "Cotizador");
            if (result.Equals("ACEPTAR") == true)
            {
                Application.Current.MainWindow.Close();
            }
        }
        #endregion
    }
}
