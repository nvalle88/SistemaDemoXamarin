namespace CityParkAgente.Models
{
    public class Multa
    {
        public int MultaId { get; set; }
        public int SalarioBasicoId { get; set; }
        public decimal Valor { get; set; }
        public System.DateTime Fecha { get; set; }
        public int AgenteId { get; set; }
        public double Longitud { get; set; }
        public double latitud { get; set; }
        public string Placa { get; set; }
        public string Plaza { get; set; }
        public string Foto { get; set; }
        public int EmpresaId { get; set; }
        public string Observacion { get; set; }
        public int TipoMultaId { get; set; }

        public virtual Agente Agente { get; set; }
    }
}