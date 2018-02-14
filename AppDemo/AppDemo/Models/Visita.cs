using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public  class Visita
    {
        public int Id { get; set; }
        public Nullable<int> IdCliente { get; set; }
        public Nullable<int> IdAgente { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public Nullable<int> Tipo { get; set; }
        public Nullable<double> Valor { get; set; }

    }
}
