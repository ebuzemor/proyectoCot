using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Cotizador.Common
{
    public class VigenciaEstatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var estatus = int.Parse(value.ToString());
            switch (estatus)
            {
                case 1: return new SolidColorBrush(Colors.Red);
                case 2: return new SolidColorBrush(Colors.Yellow);
                case 3: return new SolidColorBrush(Colors.LimeGreen);
                default: return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
