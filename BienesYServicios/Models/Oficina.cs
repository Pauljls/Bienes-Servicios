using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class Oficina
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
