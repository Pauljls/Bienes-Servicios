using BienesYServicios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BienesYServicios.Controllers
{
    [Authorize]
    public class OficinaController : Controller
    {
        private readonly ILogger<OficinaController> _logger;
        private readonly RequerimientosDbContext _context;


        public OficinaController(RequerimientosDbContext context, ILogger<OficinaController> logger) { 
            _context = context;
            _logger = logger;
        }

        // GET: OficinaController
        [Authorize(Roles = "Usuario")]
        public async Task<ActionResult> Index()
        {
            try
            {

                var oficinas = await _context.Oficinas.ToListAsync();
                ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
                ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return View(oficinas);
            }
            catch (Exception ex) {
                return BadRequest(new { Error = ex.Message});
            }
            
        }

        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ControlOficinas()
        {
            try
            {
                var oficinas = await _context.Oficinas
                    .Include(o => o.Usuarios)
                    .ToListAsync();
                ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
                ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return View("ControlOficinas",oficinas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // GET: OficinaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OficinaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OficinaController/Create
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
    }
}
