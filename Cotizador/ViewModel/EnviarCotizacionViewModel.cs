using Cotizador.Common;
using Cotizador.Model;
using System;
using System.Collections.ObjectModel;

namespace Cotizador.ViewModel
{
    public class EnviarCotizacionViewModel : Notificador
    {
        #region Variables
        private string _numCotizacion;
        private bool _ActivoEnviar;
        private string _tituloEnvio;
        private String _correosElectronicos;
        private ObservableCollection<ProductoSeleccionado> _listaProductosFT;
        private ProductoSeleccionado _ftProducto;

        public string NumCotizacion { get => _numCotizacion; set { _numCotizacion = value; OnPropertyChanged(); } }
        public bool ActivoEnviar { get => _ActivoEnviar; set { _ActivoEnviar = value; OnPropertyChanged(); } }
        public string TituloEnvio { get => _tituloEnvio; set { _tituloEnvio = value; OnPropertyChanged(); } }
        public string CorreosElectronicos
        {
            get => _correosElectronicos;
            set
            {
                _correosElectronicos = value;
                OnPropertyChanged();
                ActivarBtnEnviar();
            }
        }
        public ObservableCollection<ProductoSeleccionado> ListaProductosFT { get => _listaProductosFT; set { _listaProductosFT = value; OnPropertyChanged(); } }
        public ProductoSeleccionado FtProducto { get => _ftProducto; set { _ftProducto = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public EnviarCotizacionViewModel()
        {
        }
        #endregion

        #region Métodos
        private void ActivarBtnEnviar()
        {
            if (string.IsNullOrEmpty(CorreosElectronicos) == true || string.IsNullOrWhiteSpace(CorreosElectronicos) == true)
                ActivoEnviar = false;
            else
                ActivoEnviar = true;
        }
        #endregion
    }
}
