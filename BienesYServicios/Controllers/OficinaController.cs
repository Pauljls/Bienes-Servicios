using BienesYServicios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BienesYServicios.Controllers
{
    [Authorize]
    public class OficinaController : Controller
    {
        private readonly ILogger<OficinaController> _logger;
        private readonly RequerimientosDbContext _context;
        // GET: OficinaController
        public ActionResult Index()
        {
            return View();
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
