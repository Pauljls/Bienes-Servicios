using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class CategoriasRequerimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdSubcategoria { get; set; }

    public virtual SubcategoriaRequerimiento? IdSubcategoriaNavigation { get; set; }

    public virtual ICollection<Requerimiento> Requerimientos { get; set; } = new List<Requerimiento>();
}
