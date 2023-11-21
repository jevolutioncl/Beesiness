using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class Colmena
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime FechaIngreso { get; set; }
        [Required, MaxLength(50)]
        public string TipoColmena { get; set; }
        [MaxLength(250)]
        public string Descripcion { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
