using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class Inspeccion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(250)]
        public string Observaciones { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        // Navigation property
        public Usuario Usuario { get; set; }
    }
}
