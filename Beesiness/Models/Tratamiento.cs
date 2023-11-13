using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class Tratamiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(250)]
        public string Detalle { get; set; }

        [Required]
        [ForeignKey("EnfermedadColmena")]
        public int idEnfermedadColmena { get; set; }

        // Navigation property
        public EnfermedadColmena EnfermedadColmena { get; set; }
    }
}
