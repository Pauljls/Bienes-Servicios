using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class SubcategoriaRequerimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public int? CategoriaId { get; set; }

    public virtual CategoriasRequerimiento? Categoria { get; set; }

    public virtual ICollection<Requerimiento> Requerimientos { get; set; } = new List<Requerimiento>();
}
