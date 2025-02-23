using BienesYServicios.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BienesYServicios.Controllers
{

    public class LoginController : Controller
    {
        private readonly RequerimientosDbContext _context;
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;

        public LoginController(ILogger<LoginController> logger, RequerimientosDbContext context, IConfiguration configuration) { 
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }
        // GET: LoginController
        public IActionResult Index()
        {
            try
            {
                // Si hay un token inválido, eliminarlo
                if (Request.Cookies.ContainsKey("SesionId"))
                {
                    var token = Request.Cookies["SesionId"];
                    var handler = new JwtSecurityTokenHandler();

                    if (!handler.CanReadToken(token))
                    {
                        Response.Cookies.Delete("SesionId");
                        return View();
                    }
                }

                // Verificar si el usuario está autenticado
                if (User.Identity.IsAuthenticated)
                {
                    var userRole = User.FindFirstValue(ClaimTypes.Role);
                    if (userRole == "Administrador")
                    {
                        return RedirectToAction("AdminPanel", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }

                // Si no está autenticado, mostrar la vista de login
                return View();
            }
            catch (Exception)
            {
                // Si hay algún error en la validación del token, eliminar la cookie
                Response.Cookies.Delete("SesionId");
                return View();
            }
        }

        // POST: LoginController/Buscar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Login usuario)
        {
            try
            {
                var user = await _context.Usuarios
                    .Include(u => u.RolUsuario)
                    .FirstOrDefaultAsync(t => t.Correo == usuario.correo);

                if (user == null)
                {
                    return RedirectToAction("Index", "Login"); // ✅ Redirige en lugar de devolver BadRequest
                }

                if (!BCrypt.Net.BCrypt.Verify(usuario.contraseña, user.Contrasena))
                {
                    return RedirectToAction("Index", "Login");
                }

                // Generar el token JWT
                var token = GenerateJwtToken(user);

                var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

                var cookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = !isDevelopment,  // ✅ Usa Secure=true solo en producción
                    Expires = DateTime.UtcNow.AddMinutes(15),  // ✅ Sincroniza con la expiración del token
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("SesionId", token, cookieOptions);

                if (user.RolUsuario.Nombre == "Administrador")
                {
                    return RedirectToAction("AdminPanel", "Dashboard");
                }

                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Usuario>> register()
        {
            try
            {
                Usuario user = new()
                {
                    Nombre = "Jose",
                    Apellidos = "Barba",
                    Celular = "999999999",
                    Correo = "jose@gmail.com",
                    Contrasena = BCrypt.Net.BCrypt.HashPassword("jose1234"),
                    RolUsuarioId = 1,
                };
                await _context.Usuarios.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private string GenerateJwtToken(Usuario user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration");
            var secretKey = Encoding.UTF8.GetBytes(key);

            var claims = new List<Claim>
    {
        
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Correo.ToString()),
        new Claim(ClaimTypes.Name,user.Nombre),
        new Claim(ClaimTypes.Surname, user.Apellidos),
        new Claim(ClaimTypes.Role, user.RolUsuario.Nombre) // Se almacena el rol en el token
    };

            var symmetricKey = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not found"),
                audience: jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not found"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Expira en 30 minutos
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

