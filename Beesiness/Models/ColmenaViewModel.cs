using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class ColmenaViewModel
    {
        [Key]
        public int Id { get; set; }
        public int numIdentificador { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TipoColmena { get; set; }
        public string Descripcion { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string UbicacionPredeterminadaJson { get; set; }
        public UbicacionViewModel UbicacionPredeterminada { get; set; }
        public SelectList UbicacionesPredeterminadas { get; set; }
    }
}
