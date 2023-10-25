using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Beesiness.Models
{
    public class CreateUserViewModel
    {
        public string Nombre { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }  
        public int IdRolSeleccionado { get; set; }
        
        [BindNever]
        public IEnumerable<SelectListItem> RolesDisponibles { get; set; }

    }
}
