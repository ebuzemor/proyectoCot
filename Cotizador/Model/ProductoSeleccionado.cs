using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cotizador.Model
{
    public class ProductoSeleccionado : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public Producto Producto { get => _producto; set { _producto = value; NotifyPropertyChanged(); } }
        public double Cantidad { get => _cantidad; set { _cantidad = value; NotifyPropertyChanged(); } }
        public double Descuento { get => _descuento; set { _descuento = value; NotifyPropertyChanged(); } }
        public double ImporteDesc { get => _importeDesc; set { _importeDesc = value; NotifyPropertyChanged(); } }
        public double Importe { get => _importe; set { _importe = value; NotifyPropertyChanged(); } }
        public double Impuesto { get => _impuesto; set { _impuesto = value; NotifyPropertyChanged(); } }
        public double SubTotal { get => _subTotal; set { _subTotal = value; NotifyPropertyChanged(); } }
        public int Estatus { get => _estatus; set { _estatus = value; NotifyPropertyChanged(); } }
        public string ClaveDetalleDeComprobante { get => _claveDetalleDeComprobante; set { _claveDetalleDeComprobante = value; NotifyPropertyChanged(); } }
        public DateTime FechaEntrega { get => _fechaEntrega; set { _fechaEntrega = value; NotifyPropertyChanged(); } }
        public int DiasEntrega { get => _diasEntrega; set { _diasEntrega = value; NotifyPropertyChanged(); } }
    }
}
