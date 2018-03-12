namespace Cotizador.Model
{
    public class VigenciaEstatus
    {
        public int IdEstatus { get; set; }
        public string Descripcion { get; set; }
        public string InfoEstatus { get; set; }

        public VigenciaEstatus(int idestatus, string descripcion, string infoEstatus)
        {
            IdEstatus = idestatus;
            Descripcion = descripcion;
            InfoEstatus = infoEstatus;
        }
    }
}
