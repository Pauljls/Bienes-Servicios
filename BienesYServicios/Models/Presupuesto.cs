using System;
using System.Collections.Generic;

namespace BienesYServicios.Models;

public partial class Presupuesto
{
    public int Id { get; set; }

    public decimal ValorReferencial { get; set; }

    public string Divisa { get; set; } = null!;

    public virtual ICollection<HistorialRequerimiento> HistorialRequerimientos { get; set; } = new List<HistorialRequerimiento>();
}
