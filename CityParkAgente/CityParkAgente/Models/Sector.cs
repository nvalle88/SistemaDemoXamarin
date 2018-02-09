using System;
using System.Collections.Generic;

namespace CityParkAgente.Models
{
    public partial class Sector
    {
        public int SectorId { get; set; }
        public string NombreSector { get; set; }
        public Nullable<int> EmpresaId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agente> Agente { get; set; }
        public virtual Empresa Empresa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PuntoSector> PuntoSector { get; set; }
    }
}