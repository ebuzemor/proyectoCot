using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cotizador.Common
{
    public class Notificador : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //reducción de if (handler != null)
        }
    }
}
