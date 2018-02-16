namespace Cotizador.Model
{
    public class VigenciaEstatus
    {
        public int IdEstatus { get; set; }
        public string Descripcion { get; set; }

        public VigenciaEstatus(int idestatus, string descripcion)
        {
            IdEstatus = idestatus;
            Descripcion = descripcion;
        }
    }
}
