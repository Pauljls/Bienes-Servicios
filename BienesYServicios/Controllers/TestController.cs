using BienesYServicios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BienesYServicios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly RequerimientosDbContext _context;
        private readonly ILogger<TestController> _logger;

        public TestController(RequerimientosDbContext context, ILogger<TestController> logger) { 
            _context = context;
            _logger = logger;   
        }




    }
}
