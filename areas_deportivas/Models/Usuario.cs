using System;
using System.Collections.Generic;

namespace areas_deportivas.Models;

public partial class Usuario
{
    public Guid Id { get; set; }

    public required virtual Persona Persona { get; set; }

    public required virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
