using System;
using System.Collections.Generic;

namespace areas_deportivas.Models;

public partial class Usuario
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Role Role { get; set; }

    public Guid? ReservaId { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
