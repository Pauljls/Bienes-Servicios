using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class EstadosRequerimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<HistorialRequerimiento> HistorialRequerimientos { get; set; } = new List<HistorialRequerimiento>();
}
