using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cotizador.Common
{
    public class Notificador : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChange([CallerMemberName]string propertyName = "")
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    handler?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //reducción de if (handler != null)
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
