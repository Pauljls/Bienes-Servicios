using BienesYServicios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

namespace BienesYServicios.Controllers
{
    [Authorize]
    public class RequerimientosController : Controller
    {
        private readonly RequerimientosDbContext _context;
        private readonly ILogger<RequerimientosController> _logger;

        public RequerimientosController(RequerimientosDbContext context, ILogger<RequerimientosController> logger) { 
            _context = context;
            _logger = logger;
        }

        // GET: RequerimientosController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RequerimientosController/Details/5
        public async Task<ActionResult> verRequerimiento(int id)
        {
            ViewBag.nombre = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.apellidos = User.FindFirst(ClaimTypes.Surname)?.Value;
            ViewBag.id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var requerimiento = await _context.Requerimientos
                .Include(r => r.SubCategoriaRequerimiento)
                    .ThenInclude(s => s.Categoria)
                .Include(r => r.HistorialRequerimientos)
                    .ThenInclude(h => h.IdUsuarioNavigation)
                        .ThenInclude(u => u.Oficina)
                .Include(r => r.HistorialRequerimientos)
                    .ThenInclude(h => h.IdEstadoNavigation) // Asegurar que existe en el modelo
                .FirstOrDefaultAsync(r => r.Id == id);

            return View("verRequerimiento",requerimiento);
        }

        // POST: RequerimientosController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromBody] HistorialDTO modificacion)
        {
            try
            {

                Console.WriteLine($"Objeto recibido: {JsonConvert.SerializeObject(modificacion)}");

                DateTime fechaActual = DateTime.Now;

                HistorialRequerimiento actualizacion = new()
                {
                    IdEstado = Convert.ToInt32(modificacion.IdEstado),
                    IdUsuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                    IdRequerimiento = Convert.ToInt32(modificacion.IdRequerimiento),
                    FechaModificacion = fechaActual 
                };

                await _context.HistorialRequerimientos.AddAsync(actualizacion);
                await _context.SaveChangesAsync();
                return RedirectToAction("verRequerimiento", "Requerimientos", new { id = modificacion.IdRequerimiento });
            }
            catch (Exception ex)
            {
                // Registra el error para depuración
                _logger.LogError(ex, "Error al crear historial: {Message}", ex.Message);
                return BadRequest(new { message = "Error al guardar", error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var requerimiento = await _context.Requerimientos
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (requerimiento == null)
                {
                    return NotFound();
                }

                // Eliminar registros relacionados en HistorialRequerimiento
                var historial = _context.HistorialRequerimientos
                    .Where(h => h.IdRequerimiento == id);

                _context.HistorialRequerimientos.RemoveRange(historial);
                await _context.SaveChangesAsync();

                // Ahora eliminar el Requerimiento
                _context.Requerimientos.Remove(requerimiento);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Requerimiento eliminado con éxito" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando requerimiento: {ex.Message}");
                return BadRequest(new { message = "No se pudo eliminar el requerimiento" });
            }
        }


    }
}
