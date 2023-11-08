using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class Polinizacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(200)]
        public string Lugar { get; set; }

        [Required]
        [ForeignKey("TipoFlor")]
        public int IdTipoFlor { get; set; }

        // Navigation property
        public TipoFlor TipoFlor { get; set; }
    }
}
