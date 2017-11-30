using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotizador.Model
{
    public class ListaProductos
    {
        #region Variables
        private Producto _producto;
        private decimal _cantidad;
        private decimal _descuento;
        private decimal _importe;

        public Producto Producto { get => _producto; set => _producto = value; }
        public decimal Cantidad { get => _cantidad; set => _cantidad = value; }
        public decimal Descuento { get => _descuento; set => _descuento = value; }
        public decimal Importe { get => _importe; set => _importe = value; }
        #endregion

        #region Constructor
        public ListaProductos(Producto producto, decimal cantidad, decimal descuento, decimal importe)
        {
            Producto = producto;
            Cantidad = cantidad;
            Descuento = descuento;
            Importe = importe;
        }
        #endregion
    }
}
