using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Cotizador.View;
using Cotizador.Common;

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
