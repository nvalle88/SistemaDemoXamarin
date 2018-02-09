using System.Collections.Generic;

namespace CityParkAgente.Models
{
    public class TarjetaCredito
    {
        public int TarjetaCreditoId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Numero { get; set; }
        public int CVV_CVC { get; set; }
        public int? UsuarioId { get; set; }

        public virtual List<Parqueo> Parqueo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}