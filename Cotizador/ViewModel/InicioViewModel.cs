using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using System;

namespace Cotizador.ViewModel
{
    public class InicioViewModel : Notificador
    {
        #region Variables
        private ApiToken _apiToken;
        private Usuario _usuario;
        private CotizadorView _vwCotizador;
        private CotizadorViewModel _vmCotizador;
        private String _localhost;

        public MenuOpciones[] MenuOpcion { get; }
        public ApiToken ApiToken { get => _apiToken; set { _apiToken = value; OnPropertyChanged("ApiToken"); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged("Usuario"); } }
        public CotizadorView VwCotizador { get => _vwCotizador; set { _vwCotizador = value; OnPropertyChanged("VwCotizador"); } }
        public CotizadorViewModel VmCotizador { get => _vmCotizador; set { _vmCotizador = value; OnPropertyChanged("VmCotizador"); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged("Localhost"); } }
        #endregion

        public InicioViewModel(ApiToken apitoken, Usuario usuario, String localhost)
        {
            VmCotizador = new CotizadorViewModel
            {
                Usuario = usuario,
                ApiToken = apitoken,
                Localhost = localhost
            };
            VmCotizador.MostrarSucursal(); //para no hacer un constructor con paso de parametros.
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
