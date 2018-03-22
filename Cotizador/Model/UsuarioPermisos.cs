namespace Cotizador.Model
{
    public class UsuarioPermisos
    {
        private long _claveEFEmpresa;
        private long _claveEFUsuario;
        private string _listaPermisos;
        
        public long ClaveEFUsuario { get => _claveEFUsuario; set => _claveEFUsuario = value; }
        public long ClaveEFEmpresa { get => _claveEFEmpresa; set => _claveEFEmpresa = value; }
        public string ListaPermisos { get => _listaPermisos; set => _listaPermisos = value; }
    }
}
