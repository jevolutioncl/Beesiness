using Beesiness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Diagnostics;

namespace Beesiness.Controllers
{

    [Authorize]
    public class ColmenaController : Controller
    {
        private readonly AppDbContext _context;

        public ColmenaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ColmenaGeneral()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        // Apartado del mapa de Colmenas
        public async Task<JsonResult> ObtenerDatosColmenas(double? lat, double? lng, double range = 0.01) // range, encargado del rango para el mapa
        {
            var colmenasQuery = _context.tblColmenas.AsQueryable();

            // Filtrar basado en latitud y longitud si están presentes
            if (lat.HasValue && lng.HasValue)
            {
                colmenasQuery = colmenasQuery.Where(c =>
                    (c.Latitude >= lat - range && c.Latitude <= lat + range) &&
                    (c.Longitude >= lng - range && c.Longitude <= lng + range));
            }

            var colmenas = await colmenasQuery.Select(c => new
            {
                c.Id,
                c.numIdentificador,
                c.FechaIngreso,
                c.TipoColmena,
                c.Descripcion,
                c.Latitude,
                c.Longitude
            }).ToListAsync();

            return Json(colmenas);
        }

        public IActionResult MapaColmena()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
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
                                
                var viewModel = new List<ColmenaViewModel>();
                foreach (var item in colmenas)
                {
                    var dato = new ColmenaViewModel();
                    dato.Id = item.Id;
                    dato.numIdentificador = item.numIdentificador;
                    dato.FechaIngreso = item.FechaIngreso;
                    dato.TipoColmena = item.TipoColmena;
                    dato.Descripcion = item.Descripcion;
                    viewModel.Add(dato);
                };                
                //return View(colmenas);
                return View(viewModel);
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public IActionResult ColmenaCrear()
        {
            if (User.Identity.IsAuthenticated)
            {
                var ubicaciones = _context.tblUbicacionMapas
                    .Select(u => new SelectListItem
                    {
                        Value = JsonConvert.SerializeObject(new UbicacionViewModel
                        {
                            Id = u.Id,
                            Latitude = u.Latitude,
                            Longitude = u.Longitude,
                            Zoom = u.ZoomLevel
                        }),
                        Text = u.Nombre
                    }).ToList();

                var viewModel = new ColmenaViewModel
                {
                    UbicacionesPredeterminadas = new SelectList(ubicaciones, "Value", "Text")
                };

                return View(viewModel);
            }
            return RedirectToAction("LoginIn", "Auth");
        }
        public async Task<IActionResult> ColmenaEditar(int colmenaId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (colmenaId == 0)
                {
                    var viewModel = new ColmenaViewModel
                    {
                        UbicacionesPredeterminadas = new SelectList(await GetUbicacionesPredeterminadasAsync(), "Value", "Text")
                    };
                    return View(viewModel);
                }
                else
                {
                    var colmena = await _context.tblColmenas
                        .Include(c => c.UbicacionMapa)
                        .FirstOrDefaultAsync(x => x.Id == colmenaId);

                    if (colmena == null)
                    {
                        return NotFound();
                    }

                    UbicacionViewModel ubicacionViewModel = null;
                    if (colmena.UbicacionMapa != null)
                    {
                        ubicacionViewModel = new UbicacionViewModel
                        {
                            Id = colmena.UbicacionMapaId ?? 0,
                            Latitude = colmena.UbicacionMapa.Latitude,
                            Longitude = colmena.UbicacionMapa.Longitude,
                            Zoom = colmena.UbicacionMapa.ZoomLevel
                        };
                    }

                    var viewModel = new ColmenaViewModel
                    {
                        Id = colmena.Id,
                        numIdentificador = colmena.numIdentificador,
                        FechaIngreso = colmena.FechaIngreso,
                        TipoColmena = colmena.TipoColmena,
                        Descripcion = colmena.Descripcion,
                        Latitude = colmena.Latitude,
                        Longitude = colmena.Longitude,
                        UbicacionPredeterminada = ubicacionViewModel,
                        UbicacionesPredeterminadas = new SelectList(await GetUbicacionesPredeterminadasAsync(), "Value", "Text")
                    };

                    return View(viewModel);
                }
            }
            else
            {
                return RedirectToAction("LoginIn", "Auth");
            }
        }


        private async Task<List<SelectListItem>> GetUbicacionesPredeterminadasAsync()
        {
            return await _context.tblUbicacionMapas
                .Select(u => new SelectListItem
                {
                    Value = JsonConvert.SerializeObject(new UbicacionViewModel
                    {
                        Id = u.Id,
                        Latitude = u.Latitude,
                        Longitude = u.Longitude,
                        Zoom = u.ZoomLevel
                    }),
                    Text = u.Nombre
                }).ToListAsync();
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
        public async Task<IActionResult> ColmenaCrear(ColmenaViewModel viewModel)
        {
            ModelState.Remove("UbicacionPredeterminada");
            ModelState.Remove("UbicacionesPredeterminadas");
            ModelState.Remove("UbicacionPredeterminadaJson");

            if (User.Identity.IsAuthenticated)
            {
                // log para ver si el model es válido
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, se imprime los errores de validación
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                    foreach (var error in errors)
                    {
                        //
                        Console.WriteLine($"Error de validación: {error}");
                    }

                    // 
                    viewModel.UbicacionesPredeterminadas = new SelectList(await GetUbicacionesPredeterminadasAsync(), "Value", "Text");
                    return View(viewModel);
                }

                // creación de colmenas
                try
                {
                    // mostrar ubicacion
                    UbicacionViewModel ubicacionSeleccionada = null;
                    if (!string.IsNullOrWhiteSpace(viewModel.UbicacionPredeterminadaJson))
                    {
                        ubicacionSeleccionada = JsonConvert.DeserializeObject<UbicacionViewModel>(viewModel.UbicacionPredeterminadaJson);
                    }

                    // Crear colmena con viewmodel
                    var nuevaColmena = new Colmena
                    {
                        numIdentificador = viewModel.numIdentificador,
                        FechaIngreso = viewModel.FechaIngreso,
                        TipoColmena = viewModel.TipoColmena,
                        Descripcion = viewModel.Descripcion,
                        Latitude = viewModel.Latitude,
                        Longitude = viewModel.Longitude,
                        UbicacionMapaId = ubicacionSeleccionada?.Id
                    };

                    // Agregamos la nueva colmena al contexto
                    _context.Add(nuevaColmena);
                    // Guardamos los cambios en la base de datos
                    await _context.SaveChangesAsync();

                    // Redireccionamos al índice de colmenas si todo ha ido bien
                    return RedirectToAction("ColmenaIndex");
                }
                catch (Exception ex)
                {
                    // Si hay una excepción, la imprimimos en la consola o en el sistema de log
                    Console.WriteLine($"Excepción al crear colmena: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"Excepción interna: {ex.InnerException.Message}");

                    // Volver a cargar las ubicaciones predeterminadas y mostrar la vista con el mensaje de error
                    viewModel.UbicacionesPredeterminadas = new SelectList(await GetUbicacionesPredeterminadasAsync(), "Value", "Text");
                    // error génerico para la vista
                    ModelState.AddModelError("", "Se produjo un error al crear la colmena. Por favor, intente de nuevo.");
                    return View(viewModel);
                }
            }
            else
            {
                // Si el usuario no está autenticado, redireccionamos a la página de login
                return RedirectToAction("LoginIn", "Auth");
            }
        }



        [HttpPost]
        public async Task<IActionResult> ColmenaEditar(ColmenaViewModel viewModel)
        {
            ModelState.Remove("UbicacionPredeterminada");
            ModelState.Remove("UbicacionesPredeterminadas");
            ModelState.Remove("UbicacionPredeterminadaJson");
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var colmena = await _context.tblColmenas.FirstOrDefaultAsync(c => c.Id == viewModel.Id);
                    if (colmena == null)
                    {
                        return NotFound();
                    }

                    UbicacionViewModel ubicacionSeleccionada = null;
                            colmena.UbicacionMapaId = ubicacionSeleccionada?.Id ?? 0;
                    if (!string.IsNullOrWhiteSpace(viewModel.UbicacionPredeterminadaJson))
                    {
                        ubicacionSeleccionada = JsonConvert.DeserializeObject<UbicacionViewModel>(viewModel.UbicacionPredeterminadaJson);
                    }

                    colmena.numIdentificador = viewModel.numIdentificador;
                    colmena.FechaIngreso = viewModel.FechaIngreso;
                    colmena.TipoColmena = viewModel.TipoColmena;
                    colmena.Descripcion = viewModel.Descripcion;
                    colmena.Latitude = ubicacionSeleccionada?.Latitude ?? viewModel.Latitude;
                    colmena.Longitude = ubicacionSeleccionada?.Longitude ?? viewModel.Longitude;
                    colmena.UbicacionMapaId = ubicacionSeleccionada?.Id;

                    _context.Update(colmena);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ColmenaIndex");
                }
                else
                {
                    viewModel.UbicacionesPredeterminadas = new SelectList(await GetUbicacionesPredeterminadasAsync(), "Value", "Text", viewModel.UbicacionPredeterminada?.Id);
                    return View(viewModel);
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
            return File(stream, "application/pdf", "nombreDelPdf.pdf");
        }

    }
}
