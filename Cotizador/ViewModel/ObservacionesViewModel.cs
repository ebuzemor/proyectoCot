using Cotizador.Common;

namespace Cotizador.ViewModel
{
    public class ObservacionesViewModel : Notificador
    {
        #region Variables
        private string _observaciones;
        private bool _definitivaSeleccionado;

        public string Observaciones { get => _observaciones; set { _observaciones = value; OnPropertyChanged(); } }
        public bool DefinitivaSeleccionado { get => _definitivaSeleccionado; set { _definitivaSeleccionado = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public ObservacionesViewModel() { }
        #endregion
    }
}
