namespace Cotizador.Model
{
    public class EnvioFichaTecnica
    {
        #region Variables
        private long _claveProducto;
        private int _envioFicha;

        public long ClaveProducto { get => _claveProducto; set => _claveProducto = value; }
        public int EnvioFicha { get => _envioFicha; set => _envioFicha = value; }
        #endregion

        #region Constructor
        public EnvioFichaTecnica() { }
        #endregion
    }
}