using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class EnfermedadColmena
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaDeteccion { get; set; }

        public DateTime? FechaRecuperacion { get; set; } // Nullable

        [Required]
        [ForeignKey("Colmena")]
        public int IdColmena { get; set; }

        [Required]
        [ForeignKey("Enfermedad")]
        public int IdEnfermedad { get; set; }

        // Navigation properties (will be used when setting up relationships)
        public Colmena Colmena { get; set; }
        public Enfermedad Enfermedad { get; set; }
    }
}
