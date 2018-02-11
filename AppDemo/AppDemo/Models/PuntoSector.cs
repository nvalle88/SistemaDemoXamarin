using System;

namespace AppDemo.Models
{
    public partial class PuntoSector
    {
        public int PuntoSectorId { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public Nullable<int> SectorId { get; set; }

        public virtual Sector Sector { get; set; }
    }
}
