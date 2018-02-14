using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public partial class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Ruc { get; set; }
        public string Direccion { get; set; }
        public string PersonaContacto { get; set; }
    }

}
