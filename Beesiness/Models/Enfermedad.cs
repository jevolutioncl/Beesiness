using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class Enfermedad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }
    }
}
