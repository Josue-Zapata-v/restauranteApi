using System;
using System.Collections.Generic;

namespace Restaurante.Entidades.Models;

public partial class Reserva
{
    public int Id { get; set; }

    public string NombreCliente { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public DateTime FechaHora { get; set; }

    public int CantidadPersonas { get; set; }

    public string? Observaciones { get; set; }

    public int MesaId { get; set; }

    public virtual Mesa Mesa { get; set; } = null!;
}
