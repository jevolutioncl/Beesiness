namespace Beesiness.Models
{
    public class RequestRegistrationIndexViewModel
    {
        public List<UsuarioTemporal> Usuarios { get; set; }
        public string CurrentSort { get; set; } // Actual criterio de ordenamiento
        public string NombreSort { get; set; }
        public string CorreoSort { get; set; }
        public string RolSort { get; set; }
        public string FechaSort { get; set; }
        public string IdSort { get; set; }
    }



}
