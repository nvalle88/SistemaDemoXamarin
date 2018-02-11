using System.Collections.Generic;

namespace AppDemo.Models
{
    public class Marca
    {
        public int MarcaId { get; set; }
        public string Nombre { get; set; }

        public virtual List<Modelo> Modelo { get; set; }
    }
}