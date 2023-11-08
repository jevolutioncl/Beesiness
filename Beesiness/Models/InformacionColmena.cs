using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class InformacionColmena
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string UbicacionColmena { get; set; }

        [StringLength(50)]
        public string TiempoVida { get; set; }

        [StringLength(100)]
        public string EstadoSalud { get; set; }

        [Required]
        [ForeignKey("Inspeccion")]
        public int IdInspeccion { get; set; }

        [Required]
        [ForeignKey("Colmena")]
        public int IdColmena { get; set; }

        // Navigation properties
        public Inspeccion Inspeccion { get; set; }
        public Colmena Colmena { get; set; }

    }
}
