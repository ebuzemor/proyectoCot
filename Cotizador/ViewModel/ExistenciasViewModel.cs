using Cotizador.Common;
using Cotizador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotizador.ViewModel
{
    public class ExistenciasViewModel : Notificador
    {
        #region Commands
        
        #endregion

        #region Variables
        private ObservableCollection<Existencias> _listaExistencias;
        private string _txtProducto;

        public ObservableCollection<Existencias> ListaExistencias { get => _listaExistencias; set { _listaExistencias = value; OnPropertyChanged("ListaExistencias"); } }
        public string TxtProducto { get => _txtProducto; set { _txtProducto = value; OnPropertyChanged("TxtProducto"); } }
        #endregion

        #region Constructor
        public ExistenciasViewModel() { }
        #endregion

        #region Metodos

        #endregion
    }
}
