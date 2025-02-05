using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Celular { get; set; }

    public string? Correo { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string Contrasena { get; set; } = null!;

    public int? RolUsuarioId { get; set; }

    public int? OficinaId { get; set; }

    public virtual ICollection<HistorialRequerimiento> HistorialRequerimientos { get; set; } = new List<HistorialRequerimiento>();

    public virtual Oficina? Oficina { get; set; }

    public virtual RolUsuario? RolUsuario { get; set; }
}
