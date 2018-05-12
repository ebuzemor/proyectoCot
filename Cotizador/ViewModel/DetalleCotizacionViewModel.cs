using Cotizador.Common;
using Cotizador.Model;
using System;
using System.Collections.ObjectModel;

namespace Cotizador.ViewModel
{
    public class DetalleCotizacionViewModel : Notificador
    {
        #region Variables
        private ObservableCollection<ProductoSeleccionado> _listaProductos;
        private string _numCotizacion;
        private double _cantidadTotal;
        private double _importeTotal;
        private double _descuentoTotal;
        private double _sumaTotal;

        public ObservableCollection<ProductoSeleccionado> ListaProductos { get => _listaProductos; set { _listaProductos = value; OnPropertyChanged(); } }
        public string NumCotizacion { get => _numCotizacion; set { _numCotizacion = value; OnPropertyChanged(); } }
        public double CantidadTotal { get => _cantidadTotal; set { _cantidadTotal = value; OnPropertyChanged(); } }
        public double ImporteTotal { get => _importeTotal; set { _importeTotal = value; OnPropertyChanged(); } }
        public double DescuentoTotal { get => _descuentoTotal; set { _descuentoTotal = value; OnPropertyChanged(); } }
        public double SumaTotal { get => _sumaTotal; set { _sumaTotal = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public DetalleCotizacionViewModel() { }
        #endregion

        #region Métodos
        
        #endregion
    }
}