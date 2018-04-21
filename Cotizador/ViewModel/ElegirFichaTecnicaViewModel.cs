using System;
using System.Collections.ObjectModel;
using Cotizador.Common;
using Cotizador.Model;

namespace Cotizador.ViewModel
{
    public class ElegirFichaTecnicaViewModel : Notificador
    {
        #region Variables
        private ObservableCollection<ProductoSeleccionado> _listaProductosFT;
        private ProductoSeleccionado _ftProducto;

        public ObservableCollection<ProductoSeleccionado> ListaProductosFT { get => _listaProductosFT; set { _listaProductosFT = value; OnPropertyChanged(); } }
        public ProductoSeleccionado FtProducto { get => _ftProducto; set { _ftProducto = value; OnPropertyChanged(); } }
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public ElegirFichaTecnicaViewModel()
        {
            ListaProductosFT = new ObservableCollection<ProductoSeleccionado>();
            FtProducto = new ProductoSeleccionado();
        }
        #endregion

        #region Métodos

        #endregion
    }
}