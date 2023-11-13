using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class Alertas
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(250)]
        public string Detalle { get; set; }

        [Required]
        [ForeignKey("Colmena")]
        public int IdColmena { get; set; }

        // Navigation property
        public Colmena Colmena { get; set; }
    }
}
