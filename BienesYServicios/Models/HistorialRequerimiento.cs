using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BienesYServicios.Models;

public partial class HistorialRequerimiento
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdRequerimiento { get; set; }

    public DateTime FechaModificacion { get; set; }

    public int IdEstado { get; set; }

    public int? PresupuestoId { get; set; }

    public virtual EstadosRequerimiento IdEstadoNavigation { get; set; } = null!;

    public virtual Requerimiento IdRequerimientoNavigation { get; set; } = null!;
    
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual Presupuesto? Presupuesto { get; set; }
}
