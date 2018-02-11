namespace AppDemo.Models
{
    public class Saldo
    {
        public int SaldoId { get; set; }
        public decimal? Saldo1 { get; set; }
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}