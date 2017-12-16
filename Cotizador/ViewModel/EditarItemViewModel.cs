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

        public ProductoSeleccionado ProdSeleccionado { get => _prodSeleccionado; set { _prodSeleccionado = value; OnPropertyChanged("Seleccionado"); } }
        public bool ActivoSeleccionar { get => _activoSeleccionar; set { _activoSeleccionar = value; OnPropertyChanged("ActivoSeleccionar"); } }
        public double TxtCantidad
        {
            get => _txtCantidad;
            set
            {
                _txtCantidad = value;
                OnPropertyChanged("TxtCantidad");
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
                OnPropertyChanged("TxtDescuento");
                CalcularDescuento(_txtDescuento);
            }
        }
        public double TxtImporteDesc { get => _txtImporteDesc; set { _txtImporteDesc = value; OnPropertyChanged("TxtImporteDesc"); } }
        public double TxtImporte { get => _txtImporte; set { _txtImporte = value; OnPropertyChanged("TxtImporte"); } }
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
                    CalcularDescuento(TxtDescuento);
                    if (ProdSeleccionado.Producto.EsFraccionable == 0)
                        TxtImporte = Convert.ToInt32(cantidad) * ProdSeleccionado.Producto.PrecioUnitario - TxtImporteDesc;
                    else
                        TxtImporte = Convert.ToDouble(cantidad) * ProdSeleccionado.Producto.PrecioUnitario - TxtImporteDesc;
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
                    TxtImporte = (ProdSeleccionado.Producto.PrecioUnitario * TxtCantidad) - TxtImporteDesc;
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
            ActivoSeleccionar = (TxtCantidad > 0) ? true : false;
        }

        public void ActualizarProducto()
        {
            ProdSeleccionado.Cantidad = TxtCantidad;
            ProdSeleccionado.Descuento = TxtDescuento;
            ProdSeleccionado.ImporteDesc = Math.Round(TxtImporteDesc, 2);
            ProdSeleccionado.Importe = Math.Round(TxtImporte, 2);
            double impuestos = TxtImporte * (ProdSeleccionado.Producto.SumaImpuestos / 100.0);
            ProdSeleccionado.Impuesto = Math.Round(impuestos, 2);
            ProdSeleccionado.SubTotal = Math.Round(TxtImporte + impuestos, 2);
        }
        #endregion
    }
}
