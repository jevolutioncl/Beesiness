namespace Beesiness.Models
{
    public class RequestRegistrationIndexViewModel
    {
        public IEnumerable<UsuarioTemporal> Usuarios { get; set; }
        public int CurrentPage{ get; set; } // Actual criterio de ordenamiento
        public int TotalPages { get; set; } 
        public string NombreSort { get; set; }
        public string CorreoSort { get; set; }
        public string RolSort { get; set; }
        public string FechaSort { get; set; }
        public string IdSort { get; set; }

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
