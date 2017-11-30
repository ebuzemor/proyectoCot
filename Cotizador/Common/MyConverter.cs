using System;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;

namespace Cotizador.Common
{
    public class MyConverter : IMultiValueConverter
    {
        private List<object> _value;

        public MyConverter()
        {
            _value = new List<object>();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //return values;
            _value.AddRange(values);
            return new RelayCommand(GetCompoundExecute(), GetCompoundCanExecute());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Action<object> GetCompoundExecute()
        {
            return (parameter) =>
            {
                foreach (RelayCommand command in _value)
                {
                    if (command != default(RelayCommand))
                        command.Execute(parameter);
                }
            };
        }

        private Predicate<object> GetCompoundCanExecute()
        {
            return (parameter) =>
            {
                bool res = true;
                foreach(RelayCommand command in _value)
                {
                    if (command != default(RelayCommand))
                        res &= command.CanExecute(parameter);
                }
                return res;
            };
        }
    }
}
