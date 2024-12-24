using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class TareaColmena
    {
        [Key]
        public int Id { get; set; }        

        [Required]
        [ForeignKey("Colmena")]
        public int IdColmena { get; set; }

        [Required]
        [ForeignKey("Tarea")]
        public int IdTarea { get; set; }

        // Navigation properties
        public Colmena Colmena { get; set; }
        public Tarea Tarea { get; set; }
    }
}