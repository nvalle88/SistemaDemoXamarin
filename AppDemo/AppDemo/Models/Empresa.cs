namespace AppDemo.Models
{
    public partial class Empresa
    {
        public int EmpresaId { get; set; }
        public string RazonSocial { get; set; }
        public string Ruc { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string PersonaDeContacto { get; set; }
    }
}