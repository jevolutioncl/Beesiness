using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Beesiness.Controllers
{
    public class ColmenaController : Controller
    {
        private readonly AppDbContext _context;

        public ColmenaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ColmenaIndex(string filtro)
        {
            if (User.Identity.IsAuthenticated)
            {                
                var colmenas = await _context.tblColmenas.ToListAsync();
                if (filtro != null)
                {
                    colmenas = await _context.tblColmenas
                        .Where(d => d.TipoColmena.Contains(filtro))
                        .ToListAsync();
                }
                return View(colmenas);
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public IActionResult ColmenaCrear()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public async Task<IActionResult> ColmenaEditar(int colmenaId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (colmenaId == 0) { return View(); } //en vez de hacer un return view podriamos mandar un mensaje o volver al index
                else
                {
                    var colmena = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == colmenaId);
                    return View(colmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> ColmenaBorrar(int colmenaId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (colmenaId == 0) { return NotFound(); }
                else
                {
                    var colmena = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == colmenaId);
                    if (colmena == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(colmena);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("ColmenaIndex");
                    }

                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }


        [HttpPost]
        public async Task<IActionResult> ColmenaCrear(Colmena colmena)
        {
            if (User.Identity.IsAuthenticated)
            {                
                if (ModelState.IsValid)
                {                    
                    _context.Add(colmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ColmenaIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    return View(colmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> ColmenaEditar(Colmena colmena)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(colmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ColmenaIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Editar");
                    return View(colmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> ColmenaInfo(int colmenaId) 
        {
            var colmena = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == colmenaId);
            var infoColmena = await _context.tblInformacionColmenas.LastOrDefaultAsync(x => x.IdColmena == colmenaId);
            var inspeccion = await _context.tblInspecciones.FirstOrDefaultAsync(x => x.Id == infoColmena.IdInspeccion);
            var usuario = await _context.tblUsuarios.FirstOrDefaultAsync(x => x.Id == inspeccion.IdUsuario);
            //falta obtener las enfermedades de la colmena con un inner join
            //var enfermedadesColmena = await _context.tblEnfermedadColmena.Include(x => x.IdColmena == colmenaId).ToListAsync();
            var enfermCol = await _context.tblEnfermedadColmena.Where(x => x.IdColmena == colmenaId)
                .Join(_context.tblEnfermedades,
                tabla1 => tabla1.IdEnfermedad,
                tabla2 => tabla2.Id,
                (tabla1,tabla2) => new { tabla2.Nombre, tabla1.FechaDeteccion, tabla1.FechaRecuperacion }
                ).ToListAsync();

            InfoColmenaViewModel model = new InfoColmenaViewModel();
            model.Id = colmenaId;
            model.Descripcion = colmena.Descripcion;
            model.FechaInforme = inspeccion.Fecha;
            model.Inspector = usuario.Nombre;
            model.Ubicacion = infoColmena.UbicacionColmena;
            model.TiempoVida = infoColmena.TiempoVida;
            model.EstadoSalud = infoColmena.EstadoSalud;
            foreach (var item in enfermCol)
            {
                model.NombreEnfermedad.Add(item.Nombre);
                model.FechaDeteccionEnfermedad.Add(item.FechaDeteccion);
                model.FechaRecuperacionEnfermedad.Add(item.FechaRecuperacion);
            }
            return View(model);
        }

        public IActionResult ColmenaResumen()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");            
        }        

        //Los siguientes métodos no redireccionan a una vista, sino que otorgan funcionalidades
        public async Task<IActionResult> DatosColmenasFecha()
        {
            var colmenas = await _context.tblColmenas.ToListAsync();
            
            //uso la palabra year en vez de año para no tener problemas con la ñ
            var query = colmenas.GroupBy(
                x => x.FechaIngreso.Year,
                (year, cantidad) => new
                {
                    Year = year,
                    Cantidad = cantidad.Count()
                }
                ).OrderBy(x => x.Year);
            
            return Json(query);
        }
        
        public IActionResult DescargarPdf()
        {
            var hola = Document.Create(holapdf => 
            {
                holapdf.Page(pagina1 => 
                {
                    pagina1.Header().Text("El PDF mas basico que hay!").SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);
                    
                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(hola);
            return File(stream,"application/pdf","nombreDelPdf.pdf");
        }

    }
}
