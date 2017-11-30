
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Cotizador.ViewModel;

namespace Cotizador.View
{
    /// <summary>
    /// Lógica de interacción para InicioView.xaml
    /// </summary>
    public partial class InicioView : UserControl
    {
        public InicioView()
        {
            InitializeComponent();
            //DataContext = new InicioViewModel();
        }

        private void ListaOpciones_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //mientras está abierto el menú, esto ayudará con los scrollbars del menu
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            BtnMenuToggle.IsChecked = true;
        }
    }
}
