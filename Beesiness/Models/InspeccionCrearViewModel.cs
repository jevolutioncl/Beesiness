using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Beesiness.Models
{
    public class InspeccionCrearViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public int IdUsuario { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> UsuariosDisponibles { get; set; }
    }
}
