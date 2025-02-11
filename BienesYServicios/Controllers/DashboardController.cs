using BienesYServicios.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BienesYServicios.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly RequerimientosDbContext _context;

        public DashboardController(
            ILogger<DashboardController> logger,
            RequerimientosDbContext context
            ) {
            _logger = logger;
            _context = context;
        }
        
        [Authorize(Roles = "Usuario")]
        // GET: DashboardController
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["SesionId"]; // 🏷 Obtener la cookie

            if (!string.IsNullOrEmpty(token))
            {
                // ✅ Extraer información del usuario desde Claims
                ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
                ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return View("usuario"); // 🔥 Mostrar vista del usuario
            }
            else
            {
                // 🚨 Forzar cierre de sesión y redirección si no hay cookie
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Login");
            }
        }

       
        [Authorize(Roles = "Administrador")]

        public async Task<ActionResult> AdminPanel() {
            ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
            ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.categoria = new SelectList(await _context.CategoriasRequerimientos.ToListAsync(), "Id","Nombre");
            ViewBag.subcategoria = new SelectList(await _context.SubcategoriaRequerimientos.ToListAsync(), "Id", "Nombre");
            return View("administrador");
        }

        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult DebugClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Json(claims);
        }

        public IActionResult test() {
            return Ok(new { message = "Hola soy un test" });
        }

    }
}
