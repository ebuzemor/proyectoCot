using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using System;

namespace Cotizador.ViewModel
{
    public class InicioViewModel : Notificador
    {
        #region Variables
        private ApiKey _appKey;
        private Usuario _usuario;
        private CotizadorView _vwCotizador;
        private CotizadorViewModel _vmCotizador;
        private String _localhost;

        public MenuOpciones[] MenuOpcion { get; }
        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged("AppKey"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public CotizadorView VwCotizador { get => _vwCotizador; set { _vwCotizador = value; OnPropertyChanged("VwCotizador"); } }
        public CotizadorViewModel VmCotizador { get => _vmCotizador; set { _vmCotizador = value; OnPropertyChanged("VmCotizador"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        
        #endregion

        public InicioViewModel(ApiKey appkey, Usuario usuario, String localhost)
        {
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
            MenuOpcion = new[]
            {
                new MenuOpciones("Cotizador", VwCotizador)
            };
        }
    }
}
