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
        public ActionResult Index()
        {
            return View();
        }

        // POST: LoginController/Buscar
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> index( Login usuario)
        {
            try
            {
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(t => t.Correo == usuario.correo);

                if (user == null || !BCrypt.Net.BCrypt.Verify(usuario.contraseña, user.Contrasena))
                {
                    return BadRequest("Usuario no encontrado o contraseña invalida");
                }
                var token = GenerateJwtToken(user.Correo);
                var cookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(120),
                    SameSite = SameSiteMode.Strict
                };
                Response.Cookies.Append("SesionId", token, cookieOptions);
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Data });
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

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
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

        private string GenerateJwtToken(string correo)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration");
            var secretKey = Encoding.UTF8.GetBytes(key);

            var user = _context.Usuarios.Include( u => u.RolUsuario)
                        .FirstOrDefault(u => u.Correo == correo);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, correo),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("userId", user.Id.ToString()),
        new Claim("nombre", user.Nombre),
        new Claim("apellidos", user.Apellidos),
        new Claim("rol", user.RolUsuario.Nombre.ToString()),
            };

            var symmetricKey = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not found"),
                audience: jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not found"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

