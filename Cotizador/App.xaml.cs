using Cotizador.Common;
using Cotizador.View;
using System.Windows;

namespace Cotizador
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainView main = new MainView();
            Navigator.NavigationService = main.Navegador.NavigationService;
            main.Show();

            LoginView login = new LoginView();
            Navigator.NavigationService.Navigate(login);
        }
    }
}
