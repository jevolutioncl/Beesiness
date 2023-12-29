using Beesiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Beesiness.Controllers
{
    public class TareaController : Controller
    {
        private readonly AppDbContext _context;

        public TareaController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> TareaIndex(string filtro)
        {
            if (User.Identity.IsAuthenticated)
            {
                var variable1 = await _context.tblTareas.ToListAsync();
                if (filtro != null)
                {
                    variable1 = await _context.tblTareas
                        .Where(d => d.Nombre.Contains(filtro))
                        .ToListAsync();
                }
                return View(variable1);
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public IActionResult TareaCrear()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public IActionResult TareaGeneral()
        {
            return View();
        }

        public async Task<IActionResult> TareaEditar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return View(); }
                else
                {
                    var variable1 = await _context.tblTareas.FirstOrDefaultAsync(x => x.Id == id);
                    return View(variable1);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> TareaBorrar(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return NotFound(); }
                else
                {
                    var tarea = await _context.tblTareas.FirstOrDefaultAsync(x => x.Id == id);
                    if (tarea == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(tarea);
                        await _context.SaveChangesAsync();

                        //Actualizamos la lista de tareas en Agenda                   
                        await Actualizar2(tarea, "borrar");

                        return RedirectToAction("TareaIndex");
                    }

                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> TareaInfo(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return NotFound(); }
                else
                {
                    var tarea = await _context.tblTareas.FirstOrDefaultAsync(x => x.Id == id);
                    if (tarea == null) { return NotFound(); }
                    else
                    {
                        var colmenasTarea = await _context.tblTareaColmena.Where(x => x.IdTarea == id).ToListAsync();
                        var vm = new TareaViewModel
                        {
                            Id = tarea.Id,
                            Nombre = tarea.Nombre,
                            Descripcion = tarea.Descripcion,
                            FechaRegistro = tarea.FechaRegistro,
                            Status = tarea.Status,
                            FechaRealizacion = tarea.FechaRealizacion
                        };
                        foreach (var item in colmenasTarea)
                        {
                            var colmenas = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == item.IdColmena);
                            var tcvm = new TareaColViewModel
                            {
                                idTarea = item.IdTarea,
                                idColmena = item.IdColmena,
                                numeroColmena = colmenas.numIdentificador
                            };
                            vm.ColmenasTarea.Add(tcvm);
                        }
                        return View(vm);
                    }

                }
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> TareaCrear(Tarea tarea)
        {
            if (User.Identity.IsAuthenticated)
            {
                ModelState.Remove("FechaRegistro");
                if (ModelState.IsValid)
                {
                    //string correo = ViewData["Correo"].ToString();
                    //si la tarea fue ingresada como realizada le asignamos la fecha realización
                    //if (tarea.Status == "Realizada") tarea.FechaRealizacion = DateTime.Now;
                    tarea.FechaRegistro = DateTime.Now;

                    _context.Add(tarea);
                    await _context.SaveChangesAsync();

                    //Actualizamos la lista de tareas en Agenda
                    //await ActualizarAgenda();
                    await Actualizar2(tarea, "crear");

                    return RedirectToAction("TareaIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Guardar");
                    return View(tarea);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        [HttpPost]
        public async Task<IActionResult> TareaEditar(Tarea tarea)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    //si la tarea fue editada como realizada y no tenia fecha de realizacion
                    //le asignamos la fecha realización
                    /*if (tarea.Status == "Realizada" && tarea.FechaRealizacion == null)
                    {
                        tarea.FechaRealizacion = DateTime.Now;
                    }*/

                    _context.Update(tarea);
                    await _context.SaveChangesAsync();

                    //Actualizamos la lista de tareas en Agenda                   
                    await Actualizar2(tarea, "editar");

                    return RedirectToAction("TareaIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Error al Editar");
                    return View(tarea);
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> TareaColmenas(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0) { return View(); }
                else
                {
                    var tarea = await _context.tblTareas.FirstOrDefaultAsync(x => x.Id == id);

                    //creo el viewmodel
                    var vm = new TareaViewModel();
                    //le doy el id y nombre de la tarea
                    vm.Id = id;
                    vm.Nombre = tarea.Nombre;
                    vm.FechaRealizacion = tarea.FechaRealizacion;

                    //creo el listado de colmenas asociada a la tarea
                    var colmenasTarea = await _context.tblTareaColmena.Where(x => x.IdTarea == id).ToListAsync();
                    foreach (var item in colmenasTarea)
                    {
                        var colmena = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == item.IdColmena);
                        if (colmena != null)
                        {
                            var colmenaT = new TareaColViewModel
                            {
                                Id = item.Id,
                                idTarea = item.IdTarea,
                                idColmena = item.IdColmena,
                                numeroColmena = colmena.numIdentificador
                            };
                            vm.ColmenasTarea.Add(colmenaT);
                        }
                    }
                    //creo el listado con todas las colmenas                    
                    var colmenas = await _context.tblColmenas.ToListAsync();
                    if (colmenas != null)
                    {
                        foreach (var item2 in colmenas)
                        {
                            //Console.WriteLine(item2.Id);
                            var col = new Colmena
                            {
                                Id = item2.Id,
                                numIdentificador = item2.numIdentificador,
                                Descripcion = item2.Descripcion,
                                EstadoSalud = item2.EstadoSalud
                            };
                            vm.ColmenasTodas.Add(item2);
                        }
                    }
                    return View(vm);
                }
            }
            return RedirectToAction("LoginIn", "Auth");
        }

        public async Task<IActionResult> TCCrear(int idTarea, int idColmena)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (idTarea == 0) return RedirectToAction("TareaIndex"); //falta una notificación
                if (idColmena == 0) { return NotFound(); }
                else
                {
                    var variable1 = await _context.tblTareaColmena.
                        FirstOrDefaultAsync(x => x.IdTarea == idTarea && x.IdColmena == idColmena);
                    if (variable1 != null)
                    {
                        //ViewBag.result = "Colmena ya asociada";//aun no lo reviso bien
                        return RedirectToAction("TareaColmenas", "Tarea", new { id = idTarea });
                    } //aca tira un bug, condicion resuleta en la vista
                    else
                    {
                        var tareaColmena = new TareaColmena
                        {
                            IdTarea = idTarea,
                            IdColmena = idColmena
                        };
                        _context.Add(tareaColmena);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("TareaColmenas", "Tarea", new { id = idTarea });
                    }
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        public async Task<IActionResult> TCBorrar(int id, int idTarea)
        {
            if (User.Identity.IsAuthenticated)
            {
                //aqui id es el identificador de la tareaColmena
                if (id == 0) { return NotFound(); }
                else
                {
                    var variable1 = await _context.tblTareaColmena.FirstOrDefaultAsync(x => x.Id == id);
                    if (variable1 == null) { return NotFound(); }
                    else
                    {
                        _context.Remove(variable1);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("TareaColmenas", "Tarea", new { id = idTarea });
                    }
                }
            }
            return RedirectToAction("LoginIn", "Auth");

        }

        //codigo nuevo para actualizar la lista de tareas Agenda
        public async Task<int> Actualizar2(Tarea tarea, string accion)
        {

            if (accion == "crear")
            {
                var tvm = new TareaViewModel();
                tvm.Id = tarea.Id;
                tvm.Nombre = tarea.Nombre;
                tvm.Descripcion = tarea.Descripcion;
                tvm.CorreoAviso = tarea.CorreoAviso;
                tvm.FechaRegistro = tarea.FechaRegistro;
                tvm.FechaRealizacion = tarea.FechaRealizacion;
                tvm.Status = tarea.Status;
                AgendaController.Agenda.Add(tvm);
            }
            if (accion == "editar")
            {
                bool agregarNuevamente = true;
                foreach (var item in AgendaController.Agenda)
                {
                    if (item.Id == tarea.Id)
                    {
                        item.Nombre = tarea.Nombre;
                        item.Descripcion = tarea.Descripcion;
                        item.CorreoAviso = tarea.CorreoAviso;
                        item.Status = tarea.Status;
                        item.FechaRegistro = tarea.FechaRegistro;
                        item.FechaRealizacion = tarea.FechaRealizacion;
                        agregarNuevamente = false;
                    }
                }
                //en el caso de que la tarea ya habia sido eliminada de agenda
                if (agregarNuevamente == true) //no nos importa si la fecha aun es antigua pues enviarAvisos la descartara si es asi
                {
                    var tvm = new TareaViewModel();
                    tvm.Id = tarea.Id;
                    tvm.Nombre = tarea.Nombre;
                    tvm.Descripcion = tarea.Descripcion;
                    tvm.CorreoAviso = tarea.CorreoAviso;
                    tvm.FechaRegistro = tarea.FechaRegistro;
                    tvm.FechaRealizacion = tarea.FechaRealizacion;
                    tvm.Status = tarea.Status;
                    AgendaController.Agenda.Add(tvm);
                }
            }
            if (accion == "borrar")
            {
                AgendaController.Agenda.RemoveAll(x => x.Id == tarea.Id); //probar
            }
            return 1;
        }

        //codigo actualizar la lista de tareas Agenda
        /*
        public async Task<int> ActualizarAgenda()
        {
            //llenamos los datos para nuestra agenda            
            var tareas = await _context.tblTareas.
               Where(x => x.FechaRegistro.Date >= DateTime.Now.Date && x.Status == "Pendiente").ToListAsync();

            foreach (var tarea in tareas)
            {
                bool existeTarea = false;
                foreach (var item in AgendaController.Agenda)
                {
                    if (item.Id == tarea.Id) existeTarea = true;                          
                }

                if (existeTarea == false)
                {
                    var tvm = new TareaViewModel();
                    tvm.Id = tarea.Id;
                    tvm.Nombre = tarea.Nombre;
                    tvm.Descripcion = tarea.Descripcion;
                    tvm.FechaRegistro = tarea.FechaRegistro;
                    tvm.Status = tarea.Status;

                    var colmenasTarea = await _context.tblTareaColmena.
                        Where(x => x.IdTarea == tarea.Id).ToListAsync();

                    foreach (var item2 in colmenasTarea)
                    {
                        int numid = 0;
                        var col = await _context.tblColmenas.FirstOrDefaultAsync(x => x.Id == item2.IdColmena);
                        if (col != null) numid = col.numIdentificador;
                        var tcolvm = new TareaColViewModel
                        {
                            Id = item2.IdColmena,
                            idTarea = item2.IdTarea,
                            idColmena = item2.IdColmena,
                            numeroColmena = numid
                        };
                        tvm.ColmenasTarea.Add(tcolvm);
                    }
                    AgendaController.Agenda.Add(tvm);
                }                          
            }
            return 1;
        }*/

    }
}