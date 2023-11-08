using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Beesiness.Controllers
{
    public class InformacionColmenaController : Controller
    {
        private readonly AppDbContext _context;

        public InformacionColmenaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> InformacionColmenaIndex(int colmenaId)
        {            
            var colmena = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == colmenaId);
            //despues tiene que cambiarse a LastOrDefaultAsync
            //var infoColmena = await _context.tblInformacionColmenas.OrderBy(x => x.IdInspeccion).LastOrDefaultAsync(x => x.IdColmena == colmenaId);
            var infoColmena = await _context.tblInformacionColmenas.FirstOrDefaultAsync(x => x.IdColmena == colmenaId);
            
            //falta crear la vista cuando no hay info de la colmena
            if (infoColmena == null)
            { 
                return NotFound();
            }

            var inspeccion = await _context.tblInspecciones.FirstOrDefaultAsync(x => x.Id == infoColmena.IdInspeccion);
            var usuario = await _context.tblUsuarios.FirstOrDefaultAsync(x => x.Id == inspeccion.IdUsuario);
            //falta obtener las enfermedades de la colmena con un inner join
            //var enfermedadesColmena = await _context.tblEnfermedadColmena.Include(x => x.IdColmena == colmenaId).ToListAsync();
            var enfermCol = await _context.tblEnfermedadColmena.Where(x => x.IdColmena == colmenaId)
                .Join(_context.tblEnfermedades,
                tabla1 => tabla1.IdEnfermedad,
                tabla2 => tabla2.Id,
                (tabla1, tabla2) => new { tabla2.Nombre, tabla1.FechaDeteccion, tabla1.FechaRecuperacion }
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

        public async Task<IActionResult> InformacionColmenaCrear(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var colmena = await _context.tblInformacionColmenas.FirstOrDefaultAsync(x => x.IdColmena == id);
                InfoColmenaViewModel model = new InfoColmenaViewModel();
                model.Id = id;
                model.ColmenasDisponibles = _context.tblColmenas.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Id.ToString() //tenemos que buscar algo mejor para describirlo
                }).ToList();
                model.InspeccionesDisponibles = _context.tblInspecciones.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Id.ToString() //tenemos que buscar algo mejor para describirlo
                }).ToList();

                return View(model);
            }
            return RedirectToAction("LoginIn", "Auth");
        }
        
        public async Task<IActionResult> InformacionColmenaEditar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return View(); }
                else
                {
                    var model = await _context.tblInformacionColmenas.FirstOrDefaultAsync(x => x.Id == id);
                    return View(model);
                }
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public async Task<IActionResult> InformacionColmenaBorrar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return NotFound(); }
                else
                {
                    var infoColmena = await _context.tblInformacionColmenas.FirstOrDefaultAsync(x => x.Id == id);
                    if (infoColmena == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(infoColmena);
                        await _context.SaveChangesAsync();
                        //aca debemos cambiar para que redireccione a "Informacion de Colmenas" asociada a una Inspeccion especifica
                        return RedirectToAction("InspeccionIndex");
                    }

                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> InformacionColmenaCrear(InfoColmenaViewModel ivm)
        {
            if (User.Identity.IsAuthenticated)
            {
                ModelState.Remove("FechaInforme");
                ModelState.Remove("Inspector");
                ModelState.Remove("Descripcion");
                ModelState.Remove("NombreEnfermedad");
                ModelState.Remove("FechaDeteccionEnfermedad");
                ModelState.Remove("FechaRecuperacionEnfermedad");

                ModelState.Remove("InspeccionesDisponibles");
                ModelState.Remove("ColmenasDisponibles");
                if (ModelState.IsValid)
                {
                    InformacionColmena infoColmena = new InformacionColmena();
                    infoColmena.UbicacionColmena = ivm.Ubicacion;
                    infoColmena.TiempoVida = ivm.TiempoVida;
                    infoColmena.EstadoSalud = ivm.EstadoSalud;
                    infoColmena.IdColmena = ivm.Id;
                    infoColmena.IdInspeccion = ivm.IdInspeccion;

                    _context.Add(infoColmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("InformacionColmenaIndex", new { colmenaId = ivm.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    //return View(ivm);
                    return View();
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        /*
        [HttpPost]
        public async Task<IActionResult> InformacionColmenaCrear(InfoColmenaViewModel ivm)
        {
            if (User.Identity.IsAuthenticated)
            {
                ModelState.Remove("InspeccionesDisponibles");
                ModelState.Remove("ColmenasDisponibles");
                if (ModelState.IsValid)
                {
                    InformacionColmena infoColmena = new InformacionColmena();
                    infoColmena.UbicacionColmena = ivm.Ubicacion;
                    infoColmena.TiempoVida = ivm.TiempoVida;
                    infoColmena.EstadoSalud = ivm.EstadoSalud;
                    infoColmena.IdColmena = ivm.Id;
                    infoColmena.IdInspeccion = ivm.IdInspeccion;

                    _context.Add(infoColmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("InformacionColmenaIndex", new { colmenaId = ivm.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    return View(ivm);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }*/

        [HttpPost]
        public async Task<IActionResult> InformacionColmenaEditar(InformacionColmena infoColmena)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(infoColmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("InformacionColmenaIndex", new { colmenaId = infoColmena.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Error al Editar");
                    return View(infoColmena);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public IActionResult GenerarPdf(InfoColmenaViewModel ivm)
        {            

            var informe = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Height(50).Background(Colors.Yellow.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(5);

                            x.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Id Colmena: " + ivm.Id).Bold().FontSize(25);
                                //r.RelativeItem().Text("1").FontSize(25);
                                r.RelativeItem().Text("Fecha: " + ivm.FechaInforme.Date).Bold().FontSize(25);
                                //r.RelativeItem().Text("11-11-11").FontSize(25);
                            });

                            x.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Inspector: ").Bold().FontSize(25);
                                r.RelativeItem().Text(ivm.Inspector).FontSize(25);
                            });

                            x.Item().Text("Descripción:").Bold().FontSize(20);
                            x.Item().Text(ivm.Descripcion).FontSize(15);

                            x.Item().Text("Ubicacion:").Bold().FontSize(20);
                            x.Item().Text(ivm.Ubicacion).FontSize(15);

                            x.Item().Text("Tiempo de Vida:").Bold().FontSize(20);
                            x.Item().Text(ivm.TiempoVida).FontSize(15);

                            x.Item().Text("Estado de salud actual:").Bold().FontSize(20);
                            x.Item().Text(ivm.EstadoSalud).FontSize(15);

                            x.Item().Text("Enfermedades encontradas:").Bold().FontSize(20);
                            x.Item().Text("En construccion").FontSize(15);                            

                            x.Item().Text("Conclusiones y acciones a realizar:").Bold().FontSize(20);
                            x.Item().Text(Placeholders.LoremIpsum()).FontSize(15);
                        });


                    page.Footer().Height(50).Background(Colors.Grey.Medium);

                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(informe);
            return File(stream, "application/pdf", "informe.pdf");
        }

    }
}
