using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class Produccion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaRecoleccion { get; set; }

        public float CantidadMiel { get; set; }

        [StringLength(250)]
        public string Observaciones { get; set; }

        [Required]
        [ForeignKey("Colmena")]
        public int IdColmena { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [ForeignKey("TipoFlor")]
        public int IdTipoFlor { get; set; }

        // Navigation properties
        public Colmena Colmena { get; set; }
        public Usuario Usuario { get; set; }
        public TipoFlor TipoFlor { get; set; }
    }
}
