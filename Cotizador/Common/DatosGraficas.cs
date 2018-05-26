using System.Collections.ObjectModel;

namespace Cotizador.Common
{
    public class DatosGraficas : Notificador
    {
        private string _categoria;
        private double _numero;

        public string Categoria { get => _categoria; set { _categoria = value; OnPropertyChanged(); } }
        public double Numero { get => _numero; set { _numero = value; OnPropertyChanged(); } }
    }

    public class DatosSeries : DatosGraficas
    {
        private string _mostrarNombre;
        private string _descripcion;
        private ObservableCollection<DatosGraficas> _items;

        public string MostrarNombre { get => _mostrarNombre; set { _mostrarNombre = value; OnPropertyChanged(); } }
        public string Descripcion { get => _descripcion; set { _descripcion = value; OnPropertyChanged(); } }
        public ObservableCollection<DatosGraficas> Items { get => _items; set { _items = value; OnPropertyChanged(); } }
    }
}