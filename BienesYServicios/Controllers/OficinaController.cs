using BienesYServicios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;


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


        //[Authorize(Roles = "Administrador")]
        public  ActionResult ControlOficinas(int? page)
        {
            try
            {
                int pageSize = 6;
                int pageNumber = (page ?? 1);
                var oficinas = _context.Oficinas
                    .Include(o => o.Usuarios)
                    .ToPagedList(pageNumber,pageSize);
                ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
                ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ViewBag.Rol = User.FindFirst(ClaimTypes.Role)?.Value;
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
