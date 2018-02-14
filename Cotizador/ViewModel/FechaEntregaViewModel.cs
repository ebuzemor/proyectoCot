using Cotizador.Common;
using Cotizador.Model;
using System;

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

        public ProductoSeleccionado ProdSeleccionado { get => _prodSeleccionado; set { _prodSeleccionado = value; OnPropertyChanged(); } }
        public DateTime FechaLimite { get => _fechaLimite; set { _fechaLimite = value; OnPropertyChanged(); } }
        public DateTime FechaEntrega { get => _fechaEntrega; set { _fechaEntrega = value; OnPropertyChanged(); } }        
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
