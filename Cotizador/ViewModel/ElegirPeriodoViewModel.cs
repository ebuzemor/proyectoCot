using Cotizador.Common;
using System;

namespace Cotizador.ViewModel
{
    public class ElegirPeriodoViewModel : Notificador
    {
        #region Commands

        #endregion

        #region Variables
        private DateTime _fechaInicial;
        private DateTime _fechaFinal;

        public DateTime FechaInicial { get => _fechaInicial; set { _fechaInicial = value; OnPropertyChanged(); } }
        public DateTime FechaFinal { get => _fechaFinal; set { _fechaFinal = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public ElegirPeriodoViewModel()
        {
            DateTime hoy = DateTime.Now;
            FechaInicial = new DateTime(hoy.Year, hoy.Month, 1);
            FechaFinal = FechaInicial.AddMonths(1).AddDays(-1);
        }
        #endregion

        #region Métodos

        #endregion
    }
}