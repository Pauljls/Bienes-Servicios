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
            var token = Request.Cookies["SesionId"]; //  Obtener la cookie

            if (!string.IsNullOrEmpty(token))
            {
                //  Extraer información del usuario desde Claims
                ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
                ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return View("usuario"); //  Mostrar vista del usuario
            }
            else
            {
                //  Forzar cierre de sesión y redirección si no hay cookie
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Login");
            }
        }

       
        [Authorize(Roles = "Administrador")]

        public async Task<ActionResult> AdminPanel() {
            ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
            ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.categorias = new SelectList(await _context.CategoriasRequerimientos.ToListAsync(), "Id","Nombre");
            ViewBag.subcategoria = new SelectList(
            await _context.SubcategoriaRequerimientos.ToListAsync(),"Id","Nombre");

            return View("administrador");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerSubcategorias(int categoriaId)
        {
            var subcategorias = await _context.SubcategoriaRequerimientos
                .Where(s => s.CategoriaId == categoriaId)
                .Select(s => new { s.Id, s.Nombre })
                .ToListAsync();

            return Json(subcategorias);
        }


        //CREACION DE ESTADOS EN REQUERIMIENTOS

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> crearEstados() {
            EstadosRequerimiento estado = new EstadosRequerimiento()
            {
                Nombre = "Pendiente"
            };

            
            EstadosRequerimiento estado1 = new EstadosRequerimiento()
            {
                Nombre = "Observado"
            };
            
            EstadosRequerimiento estado2 = new EstadosRequerimiento()
            {
                Nombre = "Tramitado"
            };

            await _context.EstadosRequerimientos.AddAsync(estado);
            await _context.EstadosRequerimientos.AddAsync(estado1);
            await _context.EstadosRequerimientos.AddAsync(estado2);
            await _context.SaveChangesAsync();


            var estados = await _context.Requerimientos.ToListAsync();
            return Json(new { 
                mensaje = "Creacion completada" 
            });
        }


        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRequerimiento(Requerimiento collection)
        {
            try
            {
                Requerimiento requerimiento = new Requerimiento()
                {
                    SubCategoriaRequerimientoId = collection.SubCategoriaRequerimientoId,
                    Nombre = collection.Nombre,
                    Descripcion = collection.Descripcion
                };

                HistorialRequerimiento crearRequerimiento = new ()
                {
                    IdUsuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ,
                    IdRequerimiento = requerimiento.Id,
                    FechaModificacion = DateTime.Now,
                    IdEstado =  1
                };
                return RedirectToAction("AdminPanel","Dashboard");
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
