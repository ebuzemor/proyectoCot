using Cotizador.Common;
using System;

namespace Cotizador.Model
{
    public class ProductoSeleccionado : Notificador
    {        
        private Producto _producto;
        private double _cantidad;
        private double _descuento;
        private double _importeDesc;        
        private double _importe;
        private double _impuesto;
        private double _subTotal;
        private int _estatus; // 0-borrar, 1-actualizar, 2-agregar, 3-sincambios
        private string _claveDetalleDeComprobante;
        private DateTime _fechaEntrega;
        private int _diasEntrega;
        private double _desctoUnitario;

        public Producto Producto { get => _producto; set { _producto = value; OnPropertyChanged(); } }
        public double Cantidad { get => _cantidad; set { _cantidad = value; OnPropertyChanged(); } }
        public double Descuento { get => _descuento; set { _descuento = value; OnPropertyChanged(); } }
        public double ImporteDesc { get => _importeDesc; set { _importeDesc = value; OnPropertyChanged(); } }
        public double Importe { get => _importe; set { _importe = value; OnPropertyChanged(); } }
        public double Impuesto { get => _impuesto; set { _impuesto = value; OnPropertyChanged(); } }
        public double SubTotal { get => _subTotal; set { _subTotal = value; OnPropertyChanged(); } }
        public int Estatus { get => _estatus; set { _estatus = value; OnPropertyChanged(); } }
        public string ClaveDetalleDeComprobante { get => _claveDetalleDeComprobante; set { _claveDetalleDeComprobante = value; OnPropertyChanged(); } }
        public DateTime FechaEntrega { get => _fechaEntrega; set { _fechaEntrega = value; OnPropertyChanged(); } }
        public int DiasEntrega { get => _diasEntrega; set { _diasEntrega = value; OnPropertyChanged(); } }
        public double DesctoUnitario { get => _desctoUnitario; set { _desctoUnitario = value; OnPropertyChanged(); } }
    }
}
