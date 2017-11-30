using System.Windows;
using System.Windows.Navigation;

namespace Cotizador.Common
{
    public class Navigator
    {
        public static NavigationService NavigationService { get; set; }

        public static void Cancel()
        {
            MessageBoxResult result = MessageBox.Show("¿Seguro que quiere cancelar?", "Cancelar", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                App.Current.Shutdown(1);
        }
    }
}
