using System;
using System.Collections.Generic;

namespace Restaurante.Entidades.Models;

public partial class Mesa
{
    public int Id { get; set; }

    public int Numero { get; set; }

    public int Capacidad { get; set; }

    public string Ubicacion { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
