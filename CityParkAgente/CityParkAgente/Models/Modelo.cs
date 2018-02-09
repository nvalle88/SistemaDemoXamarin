using System.Collections.Generic;

namespace CityParkAgente.Models
{
    public class Modelo
    {
        public int ModeloId { get; set; }
        public string Nombre { get; set; }
        public int MarcaId { get; set; }

        public virtual List<Carro> Carro { get; set; }
        public virtual Marca Marca { get; set; }
    }
}