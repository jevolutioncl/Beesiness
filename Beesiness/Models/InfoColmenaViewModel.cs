using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Beesiness.Models
{
    public class InfoColmenaViewModel
    {
        public int Id { get; set; }
        public DateTime FechaInforme { get; set; }
        public string Inspector { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public string TiempoVida { get; set; }
        public string EstadoSalud { get; set; }
        public List<string> NombreEnfermedad { get; set; }
        public List<DateTime> FechaDeteccionEnfermedad { get; set; }
        public List<DateTime?> FechaRecuperacionEnfermedad { get; set; }
        //voy a crear una  sola variable para los 3 campos anteriores
        public List<EnfermedadColmena> Enfermedades { get; set; } 


        //agregado nuevo, probar si genera errores
        public int IdInspeccion { get; set; }
        [BindNever]
        public IEnumerable<SelectListItem> InspeccionesDisponibles { get; set; }
        [BindNever]
        public IEnumerable<SelectListItem> ColmenasDisponibles { get; set; }
    }
}
