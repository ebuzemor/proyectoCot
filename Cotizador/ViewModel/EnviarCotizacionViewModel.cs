﻿using Cotizador.Common;
using System;

namespace Cotizador.ViewModel
{
    public class EnviarCotizacionViewModel : Notificador
    {
        #region Variables
        private String _numCotizacion;
        private Boolean _ActivoEnviar;
        private String _correosElectronicos;

        public string NumCotizacion { get => _numCotizacion; set { _numCotizacion = value; OnPropertyChanged(); } }
        public bool ActivoEnviar { get => _ActivoEnviar; set { _ActivoEnviar = value; OnPropertyChanged(); } }
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
