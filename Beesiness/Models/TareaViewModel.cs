namespace Beesiness.Models
{
    public class TareaViewModel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? CorreoAviso { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? Status { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public List<TareaColViewModel>? ColmenasTarea { get; set; }
        public List<Colmena>? ColmenasTodas { get; set; }

        public TareaViewModel()
        {
            ColmenasTarea = new List<TareaColViewModel>();
            ColmenasTodas = new List<Colmena>();
        }
    }
}