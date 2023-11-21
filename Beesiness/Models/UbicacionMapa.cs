using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class UbicacionMapa
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        [MaxLength(255)]
        public string Descripcion { get; set; }
        [Required]
        public float Latitude { get; set; }
        [Required]
        public float Longitude { get; set; }
        [Required]
        public int ZoomLevel { get; set; }
    }
}
