using System;

namespace AppDemo.Classes
{
    public class LivePositionRequest
    {
        public int EmpresaId { get; set; }

        public string Nombre { get; set; }

        public int AgenteId { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }

        public DateTime fecha { get; set; }
    }
}