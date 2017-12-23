using System;
using System.Globalization;
using System.Windows.Data;

namespace Cotizador.Common
{
    public class ConvertidorPorcentaje : IValueConverter
    {
        //Ejemplo DB 0.042367 --> UI "4.24 %"
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fraction = double.Parse(value.ToString());
            return fraction.ToString("P2");
        }

        //Ejemplo UI "4.2367 %" --> DB 0.042367
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //Trim any trailing percentage symbol that the user MAY have included
                var valueWithoutPercentage = value.ToString().TrimEnd(' ', '%');
                return double.Parse(valueWithoutPercentage) / 100;
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
}
