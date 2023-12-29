using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class ColmenaViewModel
    {
        [Key]
        public IEnumerable<Colmena> Colmenas { get; set; }
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
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

        // Calcula si hay una página anterior basado en la página actual.
        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }
    }
}
