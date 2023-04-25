namespace SalaJuntas.Models
{
    public class Reservaciones
    {
        public int Id { get; set; }
        public int Id_sala { get; set; }
        public DateTime fecha_hora_inicial { get; set; }
        public DateTime fecha_hora_final { get; set; }
        public Boolean liberado { get; set; }
    }
}
