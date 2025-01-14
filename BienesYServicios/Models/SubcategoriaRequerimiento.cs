using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class SubcategoriaRequerimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public virtual ICollection<CategoriasRequerimiento> CategoriasRequerimientos { get; set; } = new List<CategoriasRequerimiento>();
}
