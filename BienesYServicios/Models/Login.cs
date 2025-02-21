using System.ComponentModel.DataAnnotations;

namespace BienesYServicios.Models
{
    public class Login
    {
        [Required]
        public string? correo { get; set; }
        
        [Required]
        
        public string? contraseña { get; set; }
    }
}
