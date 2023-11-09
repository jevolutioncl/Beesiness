using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;

namespace Beesiness.Controllers
{
    public class InspeccionController : Controller
    {
        private readonly AppDbContext _context;

        public InspeccionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult InspeccionGeneral()
        {
            return View();
        }

        public async Task<IActionResult> InspeccionIndex(DateTime filtro)
        {
            if (User.Identity.IsAuthenticated)
            {
                var variable1 = await _context.tblInspecciones.ToListAsync();
                /*if (filtro != null)
                {
                    variable1 = await _context.tblInspecciones
                        .Where(d => d.Fecha.Equals(filtro))
                        .ToListAsync();
                }*/
                return View(variable1);
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public IActionResult InspeccionCrear()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = new InspeccionCrearViewModel
                {
                    UsuariosDisponibles = _context.tblUsuarios.Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.Nombre
                    }).ToList()
                };

                return View(model);
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public async Task<IActionResult> InspeccionEditar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return View(); } 
                else
                {
                    var variable1 = await _context.tblInspecciones.FirstOrDefaultAsync(x => x.Id == id);
                    var model = new InspeccionCrearViewModel
                    {
                        Id = id,
                        Fecha = variable1.Fecha,
                        Observaciones = variable1.Observaciones,
                        IdUsuario = variable1.IdUsuario,
                        UsuariosDisponibles = _context.tblUsuarios.Select(r => new SelectListItem
                        {
                            Value = r.Id.ToString(),
                            Text = r.Nombre
                        }).ToList()
                    };                 
                    
                    return View(model);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> InspeccionBorrar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return NotFound(); }
                else
                {
                    var variable1 = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == id);
                    if (variable1 == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(variable1);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("InspeccionIndex");
                    }

                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> InspeccionInfo(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = await _context.tblInformacionColmenas.ToListAsync();
                if (id != null)
                {
                    model = await _context.tblInformacionColmenas
                        .Where(d => d.IdInspeccion.Equals(id))
                        .ToListAsync();
                }
                return View(model);
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> InspeccionCrear(InspeccionCrearViewModel ivm)
        {
            if (User.Identity.IsAuthenticated)
            {
                ModelState.Remove("UsuariosDisponibles");
                if (ModelState.IsValid)
                {
                    Inspeccion inspeccion = new Inspeccion();
                    inspeccion.Fecha = ivm.Fecha;
                    inspeccion.Observaciones = ivm.Observaciones;
                    inspeccion.IdUsuario = ivm.IdUsuario;

                    _context.Add(inspeccion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("InspeccionIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    return View(ivm);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> InspeccionEditar(InspeccionCrearViewModel ivm)
        {
            if (User.Identity.IsAuthenticated)
            {
                ModelState.Remove("UsuariosDisponibles");
                if (ModelState.IsValid)
                {
                    var inspeccion = await _context.tblInspecciones.FirstOrDefaultAsync(x => x.Id == ivm.Id);
                    inspeccion.Fecha = ivm.Fecha;
                    inspeccion.Observaciones = ivm.Observaciones;
                    inspeccion.IdUsuario = ivm.IdUsuario;

                    _context.Update(inspeccion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("InspeccionIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Editar");
                    return View(ivm);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }
        
        /*
        public async Task<IActionResult> InspeccionDetalle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = await _context.tblInformacionColmenas.ToListAsync();
                if (id != null)
                {
                    model = await _context.tblInformacionColmenas
                        .Where(d => d.IdInspeccion.Equals(id))
                        .ToListAsync();
                }
                return View(model);
            }
            return RedirectToAction("LoginIn", "Auth");
        }*/
        
    }
}
