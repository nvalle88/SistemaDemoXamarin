using System.Collections.Generic;

namespace AppDemo.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }


        public virtual List<UsuarioTarjetaPrepago> UsuarioTarjetaPrepago { get; set; }
    }
}