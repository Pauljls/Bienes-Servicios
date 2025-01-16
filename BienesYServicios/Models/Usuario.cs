using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BienesYServicios.Models;

public partial class Usuario
{
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; } = null!;
    [Required]
    public string Apellidos { get; set; } = null!;
    
    public string? Celular { get; set; }
    [Required]
    public string? Correo { get; set; }
    
    public DateOnly? FechaNacimiento { get; set; }
    [Required]
    public string Contrasena { get; set; } = null!;
    [Required]
    public int? RolUsuarioId { get; set; }

    public int? OficinaId { get; set; }

    public virtual ICollection<HistorialRequerimiento> HistorialRequerimientos { get; set; } = new List<HistorialRequerimiento>();

    public virtual Oficina? Oficina { get; set; }

    public virtual RolUsuario? RolUsuario { get; set; }
}
