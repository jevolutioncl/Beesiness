using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        public int IdRolSeleccionado { get; set; }

        public List<SelectListItem> RolesDisponibles { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmNewPassword { get; set; }
    }
}
