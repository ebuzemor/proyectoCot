using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cotizador.Common;
using Cotizador.Model;

namespace Cotizador.ViewModel
{
    public class EnviarCotizacionViewModel : Notificador
    {
        #region Variables
        private String _numCotizacion;
        private Boolean _ActivoEnviar;
        private String _correosElectronicos;

        public string NumCotizacion { get => _numCotizacion; set { _numCotizacion = value; OnPropertyChanged("NumCotizacion"); } }
        public bool ActivoEnviar { get => _ActivoEnviar; set { _ActivoEnviar = value; OnPropertyChanged("ActivoEnviar"); } }
        public string CorreosElectronicos
        {
            get => _correosElectronicos;
            set
            {
                _correosElectronicos = value;
                OnPropertyChanged("CorreosElectronicos");
                ActivoEnviar = (value != string.Empty) ? true : false;
            }
        }        
        #endregion

        #region Constructor
        public EnviarCotizacionViewModel()
        {            
        }
        #endregion
    }
}
