using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
