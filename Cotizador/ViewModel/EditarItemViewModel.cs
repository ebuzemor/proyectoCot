using System;
using Cotizador.Common;
using Cotizador.Model;

namespace Cotizador.ViewModel
{
    public class EditarItemViewModel : Notificador
    {
        #region Variables
        private ProductoSeleccionado _prodSeleccionado;
        private Boolean _activoSeleccionar;
        private Double _txtCantidad;
        private Double _txtDescuento;
        private Double _txtImporteDesc;
        private Double _txtImporte;

        public ProductoSeleccionado ProdSeleccionado { get => _prodSeleccionado; set { _prodSeleccionado = value; OnPropertyChanged(); } }
        public bool ActivoSeleccionar { get => _activoSeleccionar; set { _activoSeleccionar = value; OnPropertyChanged(); } }
        public double TxtCantidad
        {
            get => _txtCantidad;
            set
            {
                if (ProdSeleccionado.Producto.EsFraccionable == 0)
                    _txtCantidad = Convert.ToInt32(value);
                else
                    _txtCantidad = Convert.ToDouble(value);
                OnPropertyChanged();
                CalcularImporte(_txtCantidad);
                ActivarBtnActualizar();
            }
        }
        public double TxtDescuento
        {
            get => _txtDescuento;
            set
            {
                _txtDescuento = value;
                OnPropertyChanged();
                CalcularDescuento(_txtDescuento);
                ActivarBtnActualizar();
            }
        }
        public double TxtImporteDesc { get => _txtImporteDesc; set { _txtImporteDesc = value; OnPropertyChanged(); } }
        public double TxtImporte { get => _txtImporte; set { _txtImporte = value; OnPropertyChanged(); } }
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public EditarItemViewModel()
        { }
        #endregion

        #region Métodos
        private void CalcularImporte(double? cantidad)
        {
            try
            {
                if (cantidad > 0)
                {                    
                    TxtImporte = Convert.ToDouble(cantidad) * ProdSeleccionado.Producto.PrecioUnitario;
                    CalcularDescuento(TxtDescuento);
                }
                else
                    TxtImporte = 0;
            }
            catch (Exception)
            {
                TxtImporte = 0;
            }
        }

        private void CalcularDescuento(double? descuento)
        {
            try
            {
                if (descuento > 0 && TxtCantidad > 0)
                {
                    TxtImporteDesc = Convert.ToDouble(descuento) * ProdSeleccionado.Producto.PrecioUnitario * TxtCantidad;
                    TxtImporte = (ProdSeleccionado.Producto.PrecioUnitario * TxtCantidad);
                }
                else if (descuento == 0 && TxtCantidad > 0)
                {
                    TxtImporte = ProdSeleccionado.Producto.PrecioUnitario * TxtCantidad;
                    TxtImporteDesc = 0;
                }
                else
                {
                    TxtImporte = 0;
                    TxtImporteDesc = 0;
                }
            }
            catch (Exception)
            {
                TxtImporte = 0;
                TxtImporteDesc = 0;
            }
        }

        private void ActivarBtnActualizar()
        {
            bool desctoValido = (TxtDescuento >= 0 && TxtDescuento <= 0.9999) ? true : false;
            ActivoSeleccionar = (TxtCantidad > 0 && desctoValido == true && TxtImporte > 0) ? true : false;
        }

        public void ActualizarProducto()
        {
            ProdSeleccionado.Cantidad = Math.Round(TxtCantidad, 2);
            ProdSeleccionado.Descuento = Math.Round(TxtDescuento, 4);
            ProdSeleccionado.ImporteDesc = Math.Round(TxtImporteDesc, 2);
            ProdSeleccionado.Importe = Math.Round(TxtImporte, 2);
            double importeNeto = Math.Round(TxtImporte - TxtImporteDesc, 2);
            double impuestos = importeNeto * (ProdSeleccionado.Producto.SumaImpuestos / 100.0);
            ProdSeleccionado.Impuesto = Math.Round(impuestos, 2);
            ProdSeleccionado.SubTotal = Math.Round(importeNeto + impuestos, 2);
        }
        #endregion
    }
}
