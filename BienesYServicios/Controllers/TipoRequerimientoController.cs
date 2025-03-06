using BienesYServicios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BienesYServicios.Controllers
{
    [Authorize]
    public class TipoRequerimientoController : Controller
    {
        private readonly RequerimientosDbContext _context;
        private readonly ILogger<TipoRequerimientoController> _logger;

        public TipoRequerimientoController(RequerimientosDbContext context,
            ILogger<TipoRequerimientoController> logger) { 
            _context = context;
            _logger = logger;
        }

        //[Authorize(Roles = "Administrador")]
        // GET: TipoRequerimientoController
        public async Task<ActionResult> ControlCategoria()
        {
            ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
            ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.categorias = new SelectList(await _context.CategoriasRequerimientos.ToListAsync(),"Id", "Nombre");
            
            var subcat = await _context.SubcategoriaRequerimientos.ToListAsync();
            return View("ControlCategoria",subcat);
        }

        // GET: TipoRequerimientoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TipoRequerimientoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoRequerimientoController/Create
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

        // GET: TipoRequerimientoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TipoRequerimientoController/Edit/5
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

        // GET: TipoRequerimientoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TipoRequerimientoController/Delete/5
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
    }
}
