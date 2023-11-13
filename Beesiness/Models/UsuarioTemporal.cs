using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class UsuarioTemporal
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
        public string Rol { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public UsuarioTemporal()
        {
            FechaSolicitud = DateTime.Now;
        }
    }
}
