using Cotizador.Common;
using Cotizador.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace Cotizador.ViewModel
{
    public class SeleccionarSucursalViewModel : Notificador
    {
        #region Commands
        
        #endregion

        #region Variables
        private List<Usuario> _listaUsuarios;
        private Usuario _usuarioSel;
        private String _txtFiltro;
        private ICollectionView _icvUsuarios;

        public List<Usuario> ListaUsuarios { get => _listaUsuarios; set { _listaUsuarios = value; OnPropertyChanged(); } }
        public Usuario UsuarioSel { get => _usuarioSel; set { _usuarioSel = value; OnPropertyChanged(); } }
        public string TxtFiltro
        {
            get => _txtFiltro;
            set {
                if (value != _txtFiltro)
                {
                    _txtFiltro = value;
                    FiltrarSucursales(_txtFiltro);
                    OnPropertyChanged();
                }
            }
        }
        public ICollectionView IcvUsuarios { get => _icvUsuarios; set { _icvUsuarios = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public SeleccionarSucursalViewModel() { }
        #endregion

        #region Metodos
        private void FiltrarSucursales(string txtfiltro)
        {
            IcvUsuarios = CollectionViewSource.GetDefaultView(ListaUsuarios);
            IcvUsuarios.Filter = (x => string.IsNullOrEmpty(TxtFiltro) ? true : Convert.ToString(((Usuario)x).Sucursal).Contains(TxtFiltro.ToUpper()));
            IcvUsuarios.Refresh();
        }
        #endregion
    }
}