using Cotizador.Common;
using Cotizador.Model;
using System.Collections.ObjectModel;

namespace Cotizador.ViewModel
{
    public class ExistenciasViewModel : Notificador
    {
        #region Commands
        
        #endregion

        #region Variables
        private ObservableCollection<Existencias> _listaExistencias;
        private string _txtProducto;

        public ObservableCollection<Existencias> ListaExistencias { get => _listaExistencias; set { _listaExistencias = value; OnPropertyChanged(); } }
        public string TxtProducto { get => _txtProducto; set { _txtProducto = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public ExistenciasViewModel() { }
        #endregion

        #region Metodos

        #endregion
    }
}
