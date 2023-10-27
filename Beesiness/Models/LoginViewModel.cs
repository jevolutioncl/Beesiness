using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class LoginViewModel
    {
        public string Correo { get; set; }
        public string Password { get; set; }

    }
    public class RegistrationRequestViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required]
        [Display(Name = "Nombre y apellido")]
        public string NombreCompleto { get; set; }

        [Required]
        // Si decides agregar roles:
        [Display(Name = "Rol")]
        public string RolSeleccionado { get; set; } // para almacenar el rol seleccionado por el usuario
    }
    public class LoginRegistrationViewModel
    {
        public LoginViewModel Login { get; set; }
        public RegistrationRequestViewModel Registration { get; set; }
    }
}
