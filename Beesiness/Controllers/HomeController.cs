﻿using Beesiness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Beesiness.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        [AllowAnonymous] // Permite el acceso sin autenticación
        public IActionResult Inicio()
        {
            return View();
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Si el usuario está autenticado, obtenemos su nombre. De lo contrario, el nombre será null.
            var userName = User?.FindFirst(ClaimTypes.Name)?.Value;

            ViewData["UserName"] = userName;

            return View();
        }

        public IActionResult Sensor()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}