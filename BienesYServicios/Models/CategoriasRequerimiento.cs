using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class CategoriasRequerimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<SubcategoriaRequerimiento> SubcategoriaRequerimientos { get; set; } = new List<SubcategoriaRequerimiento>();
}
