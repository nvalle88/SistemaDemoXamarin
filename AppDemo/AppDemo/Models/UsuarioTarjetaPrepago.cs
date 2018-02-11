namespace AppDemo.Models
{
    public class UsuarioTarjetaPrepago
    {
        public int UsuarioTarjetaPrepagoId { get; set; }
        public int UsuarioId { get; set; }
        public int TarjetaPrepagoId { get; set; }

        public virtual TarjetaPrepago TarjetaPrepago { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}