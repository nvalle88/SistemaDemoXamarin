namespace AppDemo.Models
{
    public partial class TipoMultas
    {
        public int TipoMultaId { get; set; }
        public string Multa { get; set; }
        public string Descripcion { get; set; }
        public int EmpresaId { get; set; }
        public double Porcentaje { get; set; }
    }
}