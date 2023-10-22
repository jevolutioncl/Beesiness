using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Correo { get; set; }

        [Required]
        [StringLength(100)]  // Store a hashed version!
        public string Contraseña { get; set; }

        [Required]
        [ForeignKey("Rol")]
        public int IdRol { get; set; }

        // Navigation property
        public Rol Rol { get; set; }
    }
}
