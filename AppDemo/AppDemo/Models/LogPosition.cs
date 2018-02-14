using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public class LogPosition
    {
        public int id { get; set; }
        public int idAgente { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public DateTime Fecha { get; set; }
    }
}
