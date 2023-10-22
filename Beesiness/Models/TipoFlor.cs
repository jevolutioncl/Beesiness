using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class TipoFlor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
}
