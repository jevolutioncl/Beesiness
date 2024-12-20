using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class Tarea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        [Required]
        public string CorreoAviso { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime FechaRealizacion { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        //Propiedad de navegacion
        public Usuario Usuario { get; set; }

    }
}