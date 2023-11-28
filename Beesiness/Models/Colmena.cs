using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    [Index(nameof(Colmena.numIdentificador), IsUnique = true)]
    public class Colmena
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int numIdentificador { get; set; }
        [Required]
        public DateTime FechaIngreso { get; set; }
        [Required, MaxLength(50)]
        public string TipoColmena { get; set; }
        [MaxLength(250)]
        public string? Descripcion { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int? UbicacionMapaId { get; set; }
        [StringLength(100)]
        public string EstadoSalud { get; set; } //agregados del difunto infoColmena

        public UbicacionMapa UbicacionMapa { get; set; }
    }
}
