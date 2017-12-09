using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using System;
using MaterialDesignThemes.Wpf;
using System.Windows;

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
        private ApiKey _appKey;
        private Usuario _usuario;
        private String _localhost;
        private CotizadorView _vwCotizador;
        private CotizadorViewModel _vmCotizador;
        private BuscadorCotizacionesView _vwBuscadorCot;
        private BuscadorCotizacionesViewModel _vmBuscadorCot;

        public MenuOpciones[] MenuOpcion { get; }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public CotizadorView VwCotizador { get => _vwCotizador; set { _vwCotizador = value; OnPropertyChanged("VwCotizador"); } }
        public CotizadorViewModel VmCotizador { get => _vmCotizador; set { _vmCotizador = value; OnPropertyChanged("VmCotizador"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        public BuscadorCotizacionesView VwBuscadorCot { get => _vwBuscadorCot; set { _vwBuscadorCot = value; OnPropertyChanged("VwBuscadorCot"); } }
        public BuscadorCotizacionesViewModel VmBuscadorCot { get => _vmBuscadorCot; set { _vmBuscadorCot = value; OnPropertyChanged("VmBuscadorCot"); } }
        #endregion

        #region Constructor
        public InicioViewModel(ApiKey appkey, Usuario usuario, String localhost)
        {
            // COTIZADOR
            VmCotizador = new CotizadorViewModel
            {
                Usuario = usuario,
                AppKey = appkey,
                Localhost = localhost
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
                Usuario = usuario,
                AppKey = appkey,
                Localhost = localhost
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
        private async void CerrarSesion(object parameter)
        {
            var vmMensaje = new MensajeViewModel
            {
                TituloMensaje = "ADVERTENCIA",
                CuerpoMensaje = "¿Desea cerrar sesión?"
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
                CuerpoMensaje = "¿Desea salir de la aplicación?"
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
