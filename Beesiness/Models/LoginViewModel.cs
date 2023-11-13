using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "El correo no existe")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }

    }
    public class RegistrationRequestViewModel
    {
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [Display(Name = "Nombre y apellido")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El rol es requerido.")]
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
