﻿using System.ComponentModel.DataAnnotations;

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
    }
}
