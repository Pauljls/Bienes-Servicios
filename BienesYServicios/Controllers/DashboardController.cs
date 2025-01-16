using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BienesYServicios.Controllers
{
    public class DashboardController : Controller
    {
        // GET: DashboardController
        public ActionResult Index()
        {
            ViewBag.nombre = User.FindFirst("nombre")?.Value;
            ViewBag.apellido = User.FindFirst("apellido")?.Value;
            ViewBag.sub = User.FindFirst("sub")?.Value;
            if (User.FindFirst("rol").Value == "Administrador")
            {
                return View("administrador");
            }
            else { 
                return View("usuario");
            }
            
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
    }
}
