using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cotizador.Common;

namespace Cotizador.ViewModel
{
    public class MensajeViewModel : Notificador
    {
        #region Variables
        private String _tituloMensaje;
        private String _cuerpoMensaje;

        public string TituloMensaje { get => _tituloMensaje; set { _tituloMensaje = value; OnPropertyChanged("TituloMensaje"); } }
        public string CuerpoMensaje { get => _cuerpoMensaje; set { _cuerpoMensaje = value; OnPropertyChanged("CuerpoMensaje"); } }
        #endregion

        #region Constructor
        public MensajeViewModel() { }
        #endregion
    }
}
