using BienesYServicios.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;


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
        public async Task<IActionResult> Index(int? page)
        {
            var token = Request.Cookies["SesionId"]; //  Obtener la cookie

            if (!string.IsNullOrEmpty(token))
            {
                //  Extraer información del usuario desde Claims
                ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
                ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ViewBag.categorias = new SelectList(await _context.CategoriasRequerimientos.ToListAsync(), "Id", "Nombre");
                ViewBag.subcategoria = new SelectList(
                await _context.SubcategoriaRequerimientos.ToListAsync(), "Id", "Nombre");
                int pageSize = 6;
                int pageNumber = (page ?? 1);
                var requerimientos = _context.Requerimientos
                    .Include(r => r.SubCategoriaRequerimiento)
                        .ThenInclude(s => s.Categoria)
                    .Include(r => r.HistorialRequerimientos)
                        .ThenInclude(h => h.IdUsuarioNavigation)
                    .Include(r => r.HistorialRequerimientos)
                        .ThenInclude(h => h.IdEstadoNavigation)
                    .OrderBy(r => r.Id)
                    .ToPagedList(pageNumber, pageSize);

                return View("usuario",requerimientos); //  Mostrar vista del usuario
            }
            else
            {
                //  Forzar cierre de sesión y redirección si no hay cookie
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Login");
            }
        }


        [Authorize(Roles = "Administrador")]
       
        public async Task<ActionResult> AdminPanel(int? page) {

            ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
            ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.categorias = new SelectList(await _context.CategoriasRequerimientos.ToListAsync(), "Id","Nombre");
            ViewBag.subcategoria = new SelectList(
            await _context.SubcategoriaRequerimientos.ToListAsync(),"Id","Nombre");
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var requerimientos = _context.Requerimientos
                .Include(r => r.SubCategoriaRequerimiento)
                    .ThenInclude(s => s.Categoria)
                .Include(r => r.HistorialRequerimientos)
                    .ThenInclude(h => h.IdUsuarioNavigation)
                .Include(r => r.HistorialRequerimientos)
                    .ThenInclude(h => h.IdEstadoNavigation)
                .OrderBy(r => r.Id)
                .ToPagedList(pageNumber, pageSize);
            return View("administrador",requerimientos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSubcategorias(int categoriaId)
        {
            var subcategorias = await _context.SubcategoriaRequerimientos
                .Where(s => s.CategoriaId == categoriaId)
                .Select(s => new { s.Id, s.Nombre })
                .ToListAsync();

            return Json(subcategorias);
        }

        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRequerimiento([FromForm] Requerimiento collection)
        {
            try
            {
                Requerimiento requerimiento = new Requerimiento()
                {
                    SubCategoriaRequerimientoId = collection.SubCategoriaRequerimientoId,
                    Nombre = collection.Nombre,
                    Descripcion = collection.Descripcion
                };
                _context.Requerimientos.Add(requerimiento);
                await _context.SaveChangesAsync();

                // Verifica que el ID del requerimiento se ha generado correctamente
                if (requerimiento.Id <= 0)
                {
                    throw new Exception("El ID del requerimiento no se generó correctamente");
                }

                HistorialRequerimiento crearRequerimiento = new()
                {
                    IdUsuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                    IdRequerimiento = requerimiento.Id,
                    FechaModificacion = DateTime.Now,
                    IdEstado = 1
                };

                _context.HistorialRequerimientos.Add(crearRequerimiento);
                await _context.SaveChangesAsync();

                return RedirectToAction("AdminPanel", "Dashboard");
            }
            catch (Exception ex)
            {
                // Registra el error en consola o en un log
                Console.WriteLine($"Error al crear requerimiento: {ex.Message}");
                // También puedes pasar el error a la vista para mostrarlo
                TempData["ErrorMessage"] = $"Error al crear requerimiento: {ex.Message}";

                return RedirectToAction("AdminPanel", "Dashboard");
            }
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

        // POST: DashboardController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
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
