using System;
using Cotizador.Common;

namespace Cotizador.ViewModel
{
    public class MensajeViewModel : Notificador
    {
        #region Variables
        private String _tituloMensaje;
        private String _cuerpoMensaje;
        private Boolean _mostrarCancelar;

        public string TituloMensaje { get => _tituloMensaje; set { _tituloMensaje = value; OnPropertyChanged("TituloMensaje"); } }
        public string CuerpoMensaje { get => _cuerpoMensaje; set { _cuerpoMensaje = value; OnPropertyChanged("CuerpoMensaje"); } }
        public bool MostrarCancelar { get => _mostrarCancelar; set { _mostrarCancelar = value; OnPropertyChanged("MostrarCancelar"); } }
        #endregion

        #region Constructor
        public MensajeViewModel() { }
        #endregion
    }
}
