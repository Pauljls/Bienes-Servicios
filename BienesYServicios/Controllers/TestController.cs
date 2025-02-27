using BienesYServicios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BienesYServicios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly RequerimientosDbContext _context;
        //private readonly ILogger<TestController> _logger;

        public TestController( RequerimientosDbContext context){
            _context = context;
            
        }

        // GET: api/<TestController>
       /* [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
  
        }
       */
        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
               var requerimiento = await _context.Requerimientos
                    .Include( r=> r.HistorialRequerimientos)
                    .FirstAsync( r => r.Id == id);

                return Ok(requerimiento);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
            
        }

        // POST api/<TestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
