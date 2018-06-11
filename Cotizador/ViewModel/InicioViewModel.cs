using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        //private int _idVentana;
        private ApiKey _appKey;
        private Usuario _usuario;
        private String _localhost;
        private CotizadorView _vwCotizador;
        private CotizadorViewModel _vmCotizador;
        private BuscadorCotizacionesView _vwBuscadorCot;
        private BuscadorCotizacionesViewModel _vmBuscadorCot;
        private ObservableCollection<AccionesDefinidas> _listaAcciones;
        private GestionPermisosView _vwGestionP;
        private GestionPermisosViewModel _vmGestionP;
        private FichaTecnicaView _vwFichaT;
        private FichaTecnicaViewModel _vmFichaT;
        private HistorialClienteView _vwHistorialCte;
        private HistorialClienteViewModel _vmHistorialCte;
        private ReporteVendedorView _vwRptVendedor;
        private ReporteVendedorViewModel _vmRptVendedor;
        private ReporteVendedor2View _vwRptVendedor2;
        private ReporteVendedor2ViewModel _vmRptVendedor2;

        public MenuOpciones[] MenuOpcion { get; set; }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public CotizadorView VwCotizador { get => _vwCotizador; set { _vwCotizador = value; OnPropertyChanged(); } }
        public CotizadorViewModel VmCotizador { get => _vmCotizador; set { _vmCotizador = value; OnPropertyChanged(); } }
        public BuscadorCotizacionesView VwBuscadorCot { get => _vwBuscadorCot; set { _vwBuscadorCot = value; OnPropertyChanged(); } }
        public BuscadorCotizacionesViewModel VmBuscadorCot { get => _vmBuscadorCot; set { _vmBuscadorCot = value; OnPropertyChanged(); } }
        public ObservableCollection<AccionesDefinidas> ListaAcciones { get => _listaAcciones; set { _listaAcciones = value; OnPropertyChanged(); } }
        public GestionPermisosView VwGestionP { get => _vwGestionP; set { _vwGestionP = value; OnPropertyChanged(); } }
        public GestionPermisosViewModel VmGestionP { get => _vmGestionP; set { _vmGestionP = value; OnPropertyChanged(); } }
        public FichaTecnicaView VwFichaT { get => _vwFichaT; set { _vwFichaT = value; OnPropertyChanged(); } }
        public FichaTecnicaViewModel VmFichaT { get => _vmFichaT; set { _vmFichaT = value; OnPropertyChanged(); } }
        public HistorialClienteView VwHistorialCte { get => _vwHistorialCte; set { _vwHistorialCte = value; OnPropertyChanged(); } }
        public HistorialClienteViewModel VmHistorialCte { get => _vmHistorialCte; set { _vmHistorialCte = value; OnPropertyChanged(); } }
        public ReporteVendedorView VwRptVendedor { get => _vwRptVendedor; set { _vwRptVendedor = value; OnPropertyChanged(); } }
        public ReporteVendedorViewModel VmRptVendedor { get => _vmRptVendedor; set { _vmRptVendedor = value; OnPropertyChanged(); } }
        public ReporteVendedor2View VwRptVendedor2 { get => _vwRptVendedor2; set { _vwRptVendedor2 = value; OnPropertyChanged(); } }
        public ReporteVendedor2ViewModel VmRptVendedor2 { get => _vmRptVendedor2; set { _vmRptVendedor2 = value; OnPropertyChanged(); } }

        //public int IdVentana { get => _idVentana; set { _idVentana = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public InicioViewModel(ApiKey apiKey, Usuario usuario, string localhost, ObservableCollection<AccionesDefinidas> listaAcciones)
        {
            AppKey = apiKey;
            Usuario = usuario;
            Localhost = localhost;
            ListaAcciones = listaAcciones;
            //IdVentana = 0;
            CargarMenuInicial();
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
                Localhost = Localhost,
                ListaAcciones = ListaAcciones
            };
            //para no hacer un constructor con paso de parametros.
            VmCotizador.MostrarSucursal();
            VmCotizador.CargarEstatusCotizacion();
            VmCotizador.MostrarCondicionesComerciales();
            VwCotizador = new CotizadorView
            {
                DataContext = VmCotizador
            };
            // BUSCADOR DE COTIZACIONES
            VmBuscadorCot = new BuscadorCotizacionesViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost,
                ListaAcciones = ListaAcciones
            };
            VmBuscadorCot.CargarEstatusCotizacion();
            VmBuscadorCot.CargarSucursales();
            VwBuscadorCot = new BuscadorCotizacionesView
            {
                DataContext = VmBuscadorCot
            };
            // GESTION DE PERMISOS
            VmGestionP = new GestionPermisosViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost
            };
            VwGestionP = new GestionPermisosView
            {
                DataContext = VmGestionP
            };
            // FICHA TÉCNICA
            VmFichaT = new FichaTecnicaViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost
            };
            VwFichaT = new FichaTecnicaView
            {
                DataContext = VmFichaT
            };
            // HISTORIAL DEL CLIENTE
            VmHistorialCte = new HistorialClienteViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost,
                ListaAcciones = ListaAcciones
            };
            VwHistorialCte = new HistorialClienteView
            {
                DataContext = VmHistorialCte
            };
            //REPORTE DE VENDEDOR
            VmRptVendedor = new ReporteVendedorViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost,
                ListaAcciones = ListaAcciones
            };
            VmRptVendedor.ObtenerReporte(true);
            VwRptVendedor = new ReporteVendedorView
            {
                DataContext = VmRptVendedor
            };
            //REPORTE DE VENDEDOR2
            VmRptVendedor2 = new ReporteVendedor2ViewModel
            {
                Usuario = Usuario,
                AppKey = AppKey,
                Localhost = Localhost,
                ListaAcciones = ListaAcciones
            };
            VmRptVendedor2.ObtenerReporte(true);
            VwRptVendedor2 = new ReporteVendedor2View
            {
                DataContext = VmRptVendedor2
            };
            // OPCIONES DEL MENU
            List<MenuOpciones> listaMenu = new List<MenuOpciones>
            {
                new MenuOpciones("Cart", "Cotizador", VwCotizador),
                new MenuOpciones("Magnify", "Buscar Cotizaciones", VwBuscadorCot),
                new MenuOpciones("AccountSettingsVariant", "Gestión de Permisos", VwGestionP),
                new MenuOpciones("FilePdfBox", "Fichas Técnicas", VwFichaT),
                new MenuOpciones("ClipboardAccount", "Historial del Cliente", VwHistorialCte),
                new MenuOpciones("ChartBar", "Reporte Desempeño", VwRptVendedor),
                new MenuOpciones("ChartPie", "Reporte 2 Desempeño", VwRptVendedor2)
            };
            // SE VERIFICA SI EL USUARIO TIENE AUTORIZADO GESTIONAR PERMISOS
            var permiso = ListaAcciones.Single(x => x.Constante.Equals("PERMISOS_COTIZADOR") == true);
            if (permiso.Activo == false)
            {
                var opc = listaMenu.Single(x => x.Icono.Equals("AccountSettingsVariant") == true);
                listaMenu.Remove(opc);
            }
            // SE VERIFICA SI EL USUARIO TIENE AUTORIZADO BUSCAR FICHAS TECNICAS
            permiso = ListaAcciones.Single(y => y.Constante.Equals("BUSCAR_FICHASTECNICAS") == true);
            if (permiso.Activo == false)
            {
                var opc = listaMenu.Single(y => y.Icono.Equals("FilePdfBox") == true);
                listaMenu.Remove(opc);
            }
            // SE VERIFICA SI EL USUARIO TIENE AUTORIZADO CONSULTAR EL HISTORIAL DE UN CLIENTE
            permiso = ListaAcciones.Single(y => y.Constante.Equals("HISTORIAL_CLIENTE") == true);
            if (permiso.Activo == false)
            {
                var opc = listaMenu.Single(y => y.Icono.Equals("ClipboardAccount") == true);
                listaMenu.Remove(opc);
            }
            // SE VERIFICA SI EL USUARIO TIENE AUTORIZADO CONSULTAR EL REPORTE DE DESEMPEÑO DE UN VENDEDOR
            permiso = ListaAcciones.Single(y => y.Constante.Equals("REPORTE_COTIZACIONES") == true);
            if (permiso.Activo == false)
            {
                var opc = listaMenu.Single(y => y.Icono.Equals("ChartBar") == true);
                listaMenu.Remove(opc);
            }
            // AL CONVERTIR LA LISTA EN ARREGLO, SE VISUALIZA EN PANTALLA EL MENÚ
            MenuOpcion = listaMenu.ToArray();
        }

        private async void CerrarSesion(object parameter)
        {
            var vmMensaje = new MensajeViewModel
            {
                TituloMensaje = "Advertencia",
                CuerpoMensaje = "¿Desea cerrar sesión?",
                MostrarCancelar = true
            };
            var vwMensaje = new MensajeView
            {
                DataContext = vmMensaje
            };
            var result = await DialogHost.Show(vwMensaje, "Cotizador");
            if (result.Equals("OK") == true)
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
                TituloMensaje = "Advertencia",
                CuerpoMensaje = "¿Desea salir de la aplicación?",
                MostrarCancelar = true
            };
            var vwMensaje = new MensajeView
            {
                DataContext = vmMensaje
            };
            var result = await DialogHost.Show(vwMensaje, "Cotizador");
            if (result.Equals("OK") == true)
            {
                Application.Current.MainWindow.Close();
            }
        }
        #endregion
    }
}
