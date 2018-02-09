using System;
using System.Collections.Generic;

namespace CityParkAgente.Models
{
    public class Agente
    {
        public int AgenteId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contrasena { get; set; }
        public int EmpresaId { get; set; }
        public Nullable<int> SectorId { get; set; }

        public virtual Empresa Empresa { get; set; }
        public virtual Sector Sector { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Multa> Multa { get; set; }
    }
}