using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class Requerimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int? SubCategoriaRequerimientoId { get; set; }

    public virtual ICollection<HistorialRequerimiento> HistorialRequerimientos { get; set; } = new List<HistorialRequerimiento>();

    public virtual SubcategoriaRequerimiento? SubCategoriaRequerimiento { get; set; }
}
