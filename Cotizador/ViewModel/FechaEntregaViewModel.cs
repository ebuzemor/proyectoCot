using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotizador.ViewModel
{
    public class FechaEntregaViewModel : Notificador
    {
        #region Commands

        #endregion

        #region Variables
        private ProductoSeleccionado _prodSeleccionado;
        private DateTime _fechaLimite;
        private DateTime _fechaEntrega;

        public ProductoSeleccionado ProdSeleccionado { get => _prodSeleccionado; set { _prodSeleccionado = value; OnPropertyChanged("ProdSeleccionado"); } }
        public DateTime FechaLimite { get => _fechaLimite; set { _fechaLimite = value; OnPropertyChanged("FechaLimite"); } }
        public DateTime FechaEntrega { get => _fechaEntrega; set { _fechaEntrega = value; OnPropertyChanged("FechaEntrega"); } }        
        #endregion

        #region Constructor
        public FechaEntregaViewModel()
        {

        }        
        #endregion

        #region Metodos

        #endregion
    }
}
