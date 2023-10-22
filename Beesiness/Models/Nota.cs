using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class Nota
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [StringLength(250)]
        public string Observaciones { get; set; }

        [Required]
        [ForeignKey("Colmena")]
        public int IdColmena { get; set; }

        // Navigation property
        public Colmena Colmena { get; set; }
    }
}
