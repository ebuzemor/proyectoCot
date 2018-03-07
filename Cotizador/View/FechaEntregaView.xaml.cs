using System;
using System.Windows.Controls;

namespace Cotizador.View
{
    /// <summary>
    /// Lógica de interacción para FechaEntregaView.xaml
    /// </summary>
    public partial class FechaEntregaView : UserControl
    {
        public FechaEntregaView()
        {
            InitializeComponent();

            var minDate = DateTime.Now.AddYears(-1);
            var maxDate = DateTime.Now.AddYears(1);

            for(var d = minDate; d < maxDate; d = d.AddDays(1))
            {
                if(d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                {
                    FechaEntrega.BlackoutDates.Add(new CalendarDateRange(d));
                }
            }
        }
    }
}
